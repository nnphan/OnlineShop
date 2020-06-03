using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Shop
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            

            routes.MapRoute(
                name: "Product Category",
                url: "product/{metatitle}-{cateId}/",
                defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
                namespaces: new[] { "Shop.Controllers" }
            );

            routes.MapRoute(
              name: "Product Detail",
              url: "detail/{metatitle}-{productId}",
              defaults: new { controller = "Product", action = "Detail", id = UrlParameter.Optional },
              namespaces: new[] { "OnlineShop.Controllers" }
          );

            routes.MapRoute(
            name: "Add Cart",
            url: "them-gio-hang",
            defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
            namespaces: new[] { "OnlineShop.Controllers" }
        );
               routes.MapRoute(
               name: "Cart",
               url: "cart",
               defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Shop.Controllers" }
        );

            routes.MapRoute(
              name: "payment",
              url: "payment",
              defaults: new { controller = "Cart", action = "Payment", id = UrlParameter.Optional },
              namespaces: new[] { "Shop.Controllers" }
       );
            routes.MapRoute(
              name: "Finish Payment",
              url: "finish-payment",
              defaults: new { controller = "Cart", action = "Success", id = UrlParameter.Optional },
              namespaces: new[] { "Shop.Controllers" }
       );
            
            routes.MapRoute(
              name: "Sign In",
              url: "sign-in",
              defaults: new { controller = "User", action = "Register", id = UrlParameter.Optional },
              namespaces: new[] { "Shop.Controllers" }
       );
            routes.MapRoute(
              name: "User Login",
              url: "login",
              defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional },
              namespaces: new[] { "Shop.Controllers" }
       );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Shop.Controllers" }
            );

            
        }
    }
}
