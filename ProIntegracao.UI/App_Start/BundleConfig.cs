using System.Web;
using System.Web.Optimization;

namespace ProIntegracao.UI
{

    /// <summary> Bundle Config
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Register Bundles
        /// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        /// </summary>
        /// <param name="bundles">Bundle Collections</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.3.js"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            //            "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datepicker.js",
                      "~/Scripts/respond.min.js",
                      "~/Scripts/jquery.maskeditinput.js",
                      "~/Scripts/helper.js",
                      "~/Scripts/formathelper.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/DataTable/jquery.dataTables.css",
                      "~/Content/site.css"));
        }
    }
}
