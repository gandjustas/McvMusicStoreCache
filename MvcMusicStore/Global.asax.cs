using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace MvcMusicStore
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }


        public static void EnableSqlCacheDependency()
        {            
            String connStr = System.Configuration.ConfigurationManager.ConnectionStrings["MusicStoreEntities"].ConnectionString;
            System.Web.Caching.SqlCacheDependencyAdmin.EnableNotifications(connStr);
            System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connStr, "Genres");
            System.Web.Caching.SqlCacheDependencyAdmin.EnableTableForNotifications(connStr, "Albums");


                    
            //SqlDependency.Start(connStr);
        }

        protected void Application_Start()
        {
            System.Data.Entity.Database.SetInitializer(new MvcMusicStore.Models.SampleData());

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            EnableSqlCacheDependency();

        }

        protected void Application_End()
        {
            
            SqlDependency.Stop(System.Configuration.ConfigurationManager.ConnectionStrings["MusicStoreEntities"].ConnectionString);
        }

        public override string GetVaryByCustomString(HttpContext context, string custom)
        {
            if (custom == "sessionId")
            {
                var sessionCookie = context.Request.Cookies["ASP.NET_SessionId"];
                if (sessionCookie != null)
                {
                    return sessionCookie.Value;
                }
            }
            return base.GetVaryByCustomString(context, custom);
        }
    }
}