using System.Web;
using System.Web.Optimization;

namespace RSSFeed
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/splitter").Include(
                       "~/Scripts/jquery.splitter-{version}.js"
                      ));
            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
           "~/Scripts/custom.js"
          ));
            bundles.Add(new ScriptBundle("~/bundles/jstree").Include(
           "~/Scripts/jstree*"
          ));
            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
                "~/Scripts/jquery.ui.widget.js",
                "~/Scripts/jquery.iframe-transport.js",
           "~/Scripts/jquery.fileupload.js"
          ));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-rtl.css",
                      "~/Content/site.css",
                      "~/Content/jquery.splitter.css",
                      "~/Content/themes/default/style.css",
                      "~/Content/themes/default-dark/style.css",
                      "~/Content/font-awesome.css"
                      ));
        }
    }
}
