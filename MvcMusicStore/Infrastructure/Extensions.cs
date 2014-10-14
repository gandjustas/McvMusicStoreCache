using MvcMusicStore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Infrastructure
{
    public static class Extensions
    {
        public static ActionResult NotModified(this Controller controller)
        {
            return new NotModifiedResult();
        }

        public static CachedResult Cached(this ActionResult result,
                                               HttpCacheability cacheability = HttpCacheability.Private,
                                               DateTime? lastModified = null,
                                               TimeSpan? maxAge = null,
                                               String etag = "",
                                               DateTime? expires = null)
        {
            return new CachedResult(result)
            {
                Cacheability = cacheability,
                ETag = etag,
                Expires = expires,
                LastModified = lastModified,
                MaxAge = maxAge
            };
        }

                                    

    }
}