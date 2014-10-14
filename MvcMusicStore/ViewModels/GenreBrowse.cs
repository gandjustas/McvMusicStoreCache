using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcMusicStore.ViewModels
{
    public class GenreBrowse
    {
        public string Name { get; set; }
        public IEnumerable<AlbumSummary> Albums { get; set; }
    }

    public class AlbumSummary
    {
        public int AlbumId { get; set; }
        public string Title { get; set; }
        public string AlbumArtUrl { get; set; }
    }
}