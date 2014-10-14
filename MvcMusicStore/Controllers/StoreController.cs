using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using System.Web.UI;
using MvcMusicStore.Infrastructure;
using MvcMusicStore.Models;
using MvcMusicStore.ViewModels;

namespace MvcMusicStore.Controllers
{
    public class StoreController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();

        //
        // GET: /Store/
        public ActionResult Index()
        {
            var genres = storeDB.Genres.ToList();

            return View(genres);
        }

        //
        // GET: /Store/Browse?genre=Disco
        //[OutputCache(Location = OutputCacheLocation.Any, SqlDependency = "MusicStoreEntities:Genres;MusicStoreEntities:Albums", Duration = 86400)]

        //[OutputCache(CacheProfile = "Catalog")]
        public ActionResult Browse(string genre)
        {
            var cacheKey = "catalog-" + genre;
            var genreModel = Redis.GetCached(cacheKey, () =>
                (from g in storeDB.Genres
                 where g.Name == genre
                 select new GenreBrowse
                 {
                     Name =  g.Name,
                     Albums = from a in g.Albums
                              select new AlbumSummary
                              {
                                  Title =  a.Title,
                                  AlbumId =  a.AlbumId,
                                  AlbumArtUrl = a.AlbumArtUrl
                              }
                 }
                     ).Single()
                );

            this.Response.AddCacheItemDependency(cacheKey);
            this.Response.Cache.SetLastModifiedFromFileDependencies();
            this.Response.Cache.AppendCacheExtension("max-age=0");
            this.Response.Cache.VaryByParams["genre"] = true;
            this.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            return View(genreModel);
        }

        //
        // GET: /Store/Details/5

        //[OutputCache(CacheProfile = "Catalog")]
        public ActionResult Details(int id)
        {
            var cacheKey = "product-" + id;

            var album = Redis.GetCached(cacheKey, () => 
                (from a in storeDB.Albums
                     where a.AlbumId == id
                     select  new AlbumFull
                     {
                         AlbumId =  a.AlbumId,
                         AlbumArtUrl =  a.AlbumArtUrl,
                         Title =  a.Title,
                         ArtistName = a.Artist.Name,
                         GenreName = a.Genre.Name,
                         Price =  a.Price
                     })
                .Single());

            this.Response.AddCacheItemDependency(cacheKey);
            this.Response.Cache.SetLastModifiedFromFileDependencies();
            this.Response.Cache.AppendCacheExtension("max-age=0");
            this.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            return View(album);
        }

        //
        // GET: /Store/GenreMenu

        [ChildActionOnly]
        public ActionResult GenreMenu()
        {
            var cacheKey = "Nav";
            var genres = this.HttpContext.Cache.Get(cacheKey);

            if (genres == null)
            {
                genres = storeDB.Genres.ToList();
                this.HttpContext.Cache.Insert(cacheKey, genres, new SqlCacheDependency("MusicStore","Genres"));
            }

            return PartialView(genres);
        }

    }
}