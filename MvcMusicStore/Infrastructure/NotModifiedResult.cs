using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMusicStore.Infrastructure
{
    public class NotModifiedResult: ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.StatusCode = 304;
            response.StatusDescription = "Not Modified";
            response.SuppressContent = true;
        }
    }
}