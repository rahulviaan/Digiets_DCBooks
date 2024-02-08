﻿using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DC.Web.App
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        protected void Application_BeginRequest()
        {
            bool isLocal = Request.IsLocal;
            //if (!Request.IsSecureConnection && !isLocal)
            //{
            //    Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"));
            //}
        }
    }
}
