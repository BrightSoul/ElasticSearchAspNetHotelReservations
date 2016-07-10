using System.Web;
using System.Web.Optimization;

namespace AspNetElasticSearchDemo
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/knockout-{version}.js",
                        "~/Scripts/bootstrap-slider.js",
                        "~/Scripts/daterangepicker.js"
                        ));

            bundles.Add(new StyleBundle("~/Styles").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-slider/bootstrap-slider.css",
                      "~/Content/daterangepicker-bs3.css",
                      "~/Content/site.css"
                      ));
        }
    }
}
