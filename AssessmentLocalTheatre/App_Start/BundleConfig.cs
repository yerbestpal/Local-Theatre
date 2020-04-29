using System.Web;
using System.Web.Optimization;

namespace AssessmentLocalTheatre
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                       "~/vendor/jquery/jquery-min.js",
                       "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/vendor/bootstrap/js/bootstrap.bundle.min.js", 
                "~/Scripts/clean-blog.js", 
                "~/Scripts/clean-blog.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/vendor/bootstrap/css/bootstrap.css",
                 "~/vendor/bootstrap/css/bootstrap.min.css",
                 "~/Content/clean-blog.css",
                 "~/Content/clean-blog.min.css",
                 "~/vendor/fontawesome-free/css/all.min.css"
                ));
        }
    }
}
