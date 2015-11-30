using System.Web.Optimization;

namespace LykkeWallet
{
    public class BundleConfig
    {
        public const string Css = "~/Content/css";
        public const string Js = "~/Content/js";
        public const string Lk = "~/Content/lk";

        public const string MobileCss = "~/Content/mcss";
        public const string MobileJs = "~/Content/mjs";




        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle(Css).Include(
                "~/Content/bootstrap.css",
                "~/Content/common.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle(Js).Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/Requests.js",
                "~/Scripts/UI.js",

                "~/Scripts/utils.js"
                ));

            bundles.Add(new StyleBundle(MobileCss).Include(
                "~/Content/bootstrap.css",
                "~/Content/common.css",
                "~/Content/mobile.css"));

            bundles.Add(new ScriptBundle(MobileJs).Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/bootstrap.js",
                "~/Scripts/Requests.js",
                "~/Scripts/MobileUi.js",
                "~/Scripts/WalletMobile.js",
                "~/Scripts/utils.js"
                ));


            bundles.Add(new ScriptBundle(Lk).Include(
                "~/Scripts/LkMarket.js"
                ));

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
                      "~/Scripts/respond.js"));


        }
    }
}
