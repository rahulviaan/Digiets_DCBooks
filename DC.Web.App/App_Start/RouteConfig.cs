using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace DC.Web.App
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
             
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.Add("ManageBook", new SeoFriendlyRoute("Book/{_detail}",
             new RouteValueDictionary(new { controller = "Books", action = "Manage", catid = UrlParameter.Optional, id = UrlParameter.Optional, _detail = UrlParameter.Optional }),
             new MvcRouteHandler()));

            routes.Add("ManageUsers", new SeoFriendlyRoute("ManageUser/{id}",
            new RouteValueDictionary(new { controller = "ICUser", action = "Index", id = UrlParameter.Optional }),
            new MvcRouteHandler()));

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional }
            );

        }
        public class SeoFriendlyRoute : Route
        {
            public SeoFriendlyRoute(string url, RouteValueDictionary defaults, IRouteHandler routeHandler) : base(url, defaults, routeHandler)
            {

            }

            public override RouteData GetRouteData(HttpContextBase httpContext)
            {
                var routeData = base.GetRouteData(httpContext);

                if (routeData != null)
                {

                    if (routeData.Values.ContainsKey("id"))
                        routeData.Values["id"] = GetIdValue(routeData.Values["id"]);
                }

                return routeData;
            }

            private object GetIdValue(object id)
            {
                if (id != null)
                {
                    //string idValue = id.ToString();
                    // return idValue;
                    //var regex = new Regex(@"^(?<id>\d+).*$");
                    //var match = regex.Match(idValue);

                    //if (match.Success)
                    //{
                    //    return match.Groups["id"].Value;
                    //}
                }
                else
                {
                    id = "_";
                }
                return id;
            }
        }
    }
}
