using System.Web.Optimization;

namespace DC.Web.App
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/Common.js"
                ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.min.css", 
                "~/Content/site.css",
                "~/Content/style.css",
                "~/Content/Common.css"
                ));
              


            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
               "~/vendor/jquery/jquery.min.js",
               "~/vendor/bootstrap/js/bootstrap.bundle.min.js",
               "~/vendor/jquery-easing/jquery.easing.min.js",
               "~/Scripts/sb-admin.min.js",
               "~/Scripts/Common.js",
               "~/Scripts/bootbox.min.js"
                ));
            bundles.Add(new StyleBundle("~/Content/admincss").Include(
                  "~/Content/bootstrap/css/bootstrap.min.css",
                  "~/Content/font-awesome.min.css",
                   "~/Content/sb-admin.min.css"
                  ));
            //BundleTable.EnableOptimizations = true;
        }
    }
}
