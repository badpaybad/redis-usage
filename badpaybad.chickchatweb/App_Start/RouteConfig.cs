using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace badpaybad.chickchatweb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Login",
              url: "login",
              defaults: new { controller = "Default", action ="Login", id = UrlParameter.Optional }
          );

            routes.MapRoute(
              name: "Logout",
              url: "logout",
              defaults: new { controller = "Default", action = "Logout", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "PushMessage",
              url: "pushmessage",
              defaults: new { controller = "Default", action = "PushMessage", id = UrlParameter.Optional }
          );
            routes.MapRoute(
              name: "JoinChat",
              url: "joinchat",
              defaults: new { controller = "Default", action = "JoinChat", id = UrlParameter.Optional }
          );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Default", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
