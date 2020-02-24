using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WebApplicationFPTGroup14
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}",
                defaults: new { controller = "Home", action = "Index" }
            );

            routes.MapRoute(
                name: "SinglePost",
                url: "{controller}/{action}/{postID}",
                defaults: new { controller = "Posts", action = "SinglePost", postID = UrlParameter.Optional }
            );

            
        }
    }
}
