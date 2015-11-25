using System.Web.Optimization;

namespace BackEnd
{
    public  class BundleConfig
    {
        public const string BasicScripts = "~/Content/BasicScripts";
        public const string BackOfficeScripts = "~/Content/BackOfficeScripts";
        public const string BackOfficeMobileScripts = "~/Content/BackOfficeMobileScripts";

        public const string BackOfficeCss = "~/Content/SpaCss";


        public const string MobileScripts = "~/Content/MobileScripts";
        public const string MobileCss = "~/Content/MobileCss";

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle(BasicScripts).Include(
                        "~/Scripts/jquery-{version}.js"
                                        ));

            bundles.Add(new ScriptBundle(BackOfficeScripts).Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/date.js",
                        "~/Scripts/UI.js",
                        "~/Scripts/Requests.js",
                        "~/Scripts/Backoffice.js",
                        "~/Scripts/Layout.js",
                        "~/Content/Scrollbar/jquery.mousewheel.js",
                        "~/Content/Scrollbar/perfect-scrollbar.js",
                        "~/Scripts/UiControls/GroupSelection.js",
                        "~/Scripts/UiControls/MultiSelectWindow.js",
                        "~/Scripts/WebSite.js"

                                        ));

            bundles.Add(new StyleBundle(BackOfficeCss).Include(
                      "~/Content/bootstrap.css",
                      //"~/Content/bootstrap-theme.css",
                                    "~/Content/material-fullpalette.css",
                      "~/Content/Scrollbar/perfect-scrollbar.css",
                      "~/Content/site.css",
                      "~/Content/Other.css",
                      "~/Content/SideMenu.css",
                      "~/Content/monthpicker.css",
                      "~/Content/Countries.css"
                      ));



            bundles.Add(new ScriptBundle(MobileScripts).Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                          "~/Scripts/UiMobile.js",
                        "~/Scripts/Requests.js",
                        "~/Scripts/Backoffice.js",
                        "~/Scripts/Layout.js",
                        "~/Scripts/UiControls/GroupSelection.js",
                        "~/Scripts/UiControls/MultiSelectWindow.js",
                        "~/Scripts/WebSite.js"
                        ));


            bundles.Add(new StyleBundle(MobileCss).Include(
                "~/Content/bootstrap.css",

                "~/Content/material-fullpalette.css",
                "~/Content/mobile.css",
                "~/Content/Countries.css"
                ));



        }

    }
}
