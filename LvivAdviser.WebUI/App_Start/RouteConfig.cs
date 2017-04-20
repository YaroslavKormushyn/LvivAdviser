using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace LvivAdviser.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(null, "", new {controller = "Content", action = "List", type = (string)null, page = 1 });

            routes.MapRoute(null, "Page{page}", new {controller = "Content", action = "List", type = (string)null}, new { page = @"\d+" });

            routes.MapRoute(null, "{type}", new { controller = "Content", action = "List", page = 1 });

            routes.MapRoute(null, "{type}/Page{page}", new { controller = "Content", action = "List" }, new { page = @"\d+" });

            routes.MapRoute(null, "{controller}/{action}");
        }
    }
}