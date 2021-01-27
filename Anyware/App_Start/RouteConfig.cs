using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Anyware
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );


            routes.MapRoute(
                "ProductSort",
                "Products/{searchString}/{inStock}/{sortString}",
                new{
                    controller = "Products",
                    action = " Index",
                    sortString = UrlParameter.Optional,
                    inStock = UrlParameter.Optional,
                    searchString =UrlParameter.Optional
                    
                }
                );

        }



    }
}
