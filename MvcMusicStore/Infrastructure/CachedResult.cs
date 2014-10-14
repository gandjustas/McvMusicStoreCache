using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Infrastructure
{
    public class CachedResult : ActionResult
    {
        public ActionResult Result { get; private set; }

        public HttpCacheability? Cacheability { get; set; }
        
        public DateTime? LastModified { get; set; }
        public string ETag { get; set; }
        public TimeSpan? MaxAge { get; set; }
        public DateTime? Expires { get; set; }

        public CachedResult(ActionResult result)
        {
            this.Result = result;
        }
        public override void ExecuteResult(ControllerContext context)
        {
            var cache = context.HttpContext.Response.Cache;
            if (this.Cacheability.HasValue)
            {
                cache.SetCacheability(Cacheability.Value);
            }

            if (this.LastModified.HasValue)
            {
                cache.SetLastModified(this.LastModified.Value);
            }

            if (!string.IsNullOrWhiteSpace(this.ETag))
            {
                cache.SetETag(this.ETag);
            }

            if (this.MaxAge.HasValue)
            {
                cache.SetMaxAge(this.MaxAge.Value);
            }

            if (this.Expires.HasValue)
            {
                cache.SetExpires(this.Expires.Value);
            }
            
            cache.SetValidUntilExpires(true);
            cache.SetLastModifiedFromFileDependencies();
            cache.SetETagFromFileDependencies();

            this.Result.ExecuteResult(context);
        }
    }
    
}