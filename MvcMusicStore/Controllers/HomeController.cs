using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MvcMusicStore.Models;
using System;
using System.Web;
using MvcMusicStore.Infrastructure;
using System.Web.Caching;

namespace MvcMusicStore.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        MusicStoreEntities storeDB = new MusicStoreEntities();

        [OutputCache(CacheProfile = "Home")]
        public ActionResult Index()
        {
                // Get most popular albums
                var query = GetTopSellingAlbums(5);


                var albums = query.ToList();

                return View(albums);

                                    
        }

        private IQueryable<Album> GetTopSellingAlbums(int count)
        {
            // Group the order details by album and return
            // the albums with the highest count

            return storeDB.Albums
                .OrderByDescending(a => a.OrderDetails.Count())
                .Take(count);

        }
    }
}