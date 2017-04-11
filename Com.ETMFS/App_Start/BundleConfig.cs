using System.Web;
using System.Web.Optimization;

namespace Com.ETMFS
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js", "~/Scripts/jquery.easyui.min.js", "~/Scripts/knockout-3.4.0.js", "~/Scripts/Common.js", "~/Scripts/jquery.circliful.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/accountjs").Include(
                        "~/Scripts/Permission/AccountJs.js"));

            bundles.Add(new ScriptBundle("~/bundles/AppConfigSettingjs").Include(
              "~/Scripts/Permission/AppConfigSetting.js"));
            bundles.Add(new ScriptBundle("~/bundles/Countryjs").Include(
                       "~/Scripts/MasterData/Country.js"));
            bundles.Add(new ScriptBundle("~/bundles/groupjs").Include(
                       "~/Scripts/Permission/Group.js"));
            bundles.Add(new ScriptBundle("~/bundles/TMFModeljs").Include(
                 "~/Scripts/MasterData/TMFModel.js"));
            bundles.Add(new ScriptBundle("~/bundles/StudyControllerjs").Include(
                 "~/Scripts/MasterData/StudyController.js"));

            bundles.Add(new ScriptBundle("~/bundles/HighCharts").Include(
             "~/Scripts/highcharts.js", "~/Scripts/highcharts-more.js", "~/Scripts/highcharts-3d.js"));

            bundles.Add(new ScriptBundle("~/bundles/Documentjs").Include(
                 "~/Scripts/MasterData/DocumentController.js", "~/Scripts/MasterData/StudyController.js"));
          
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
           

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/themes/default/easyui.css", "~/Content/themes/icon.css", "~/Content/Site.css"));
        }
    }
}
