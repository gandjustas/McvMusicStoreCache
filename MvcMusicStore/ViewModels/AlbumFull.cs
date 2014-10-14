using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.ViewModels
{
    public class AlbumFull
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string AlbumArtUrl { get; set; }
        public string GenreName { get; set; }
        public string ArtistName { get; set; }
        public decimal Price { get; set; }
    }

}