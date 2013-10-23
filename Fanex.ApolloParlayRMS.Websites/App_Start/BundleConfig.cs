

namespace Fanex.ApolloParlayRMS.Websites
{
    using System.Diagnostics.CodeAnalysis;
    using System.Web;
    using System.Web.Optimization;
    using BundleTransformer.Core.Orderers;
    using BundleTransformer.Core.Transformers;

    /// <summary>
    /// Bundle Configuration.
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers the bundles.
        /// </summary>
        /// <param name="bundles">The bundle collections.</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterCommonBundles(bundles);
            RegisterMemberManagementBundles(bundles);
            RegisterReportBundles(bundles);
            RegisterToolsBundles(bundles);
        }

        /// <summary>
        /// Registers the common bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        private static void RegisterCommonBundles(BundleCollection bundles)
        {
            bundles.AddAndInclude(
                "~/Themes/Styles",
                "~/Content/themes/reset.css",
                "~/Content/themes/jquery-ui-1.8.23.smoothness.css",
                "~/Scripts/lib/tipsy/tipsy-1.0.0.css",
                "~/Scripts/lib/msgbox/msgbox-0.0.1.css",
                "~/Scripts/lib/Fanex.Grid/Fanex.Grid.css",
                "~/Scripts/lib/Fanex.Grid/Fanex.Pagination.css",
                "~/Scripts/lib/Multiselect/jquery.multiselect.css");

            bundles.AddAndInclude(
                "~/Themes/Layout",
                "~/Content/themes/layout/layout.less");

            bundles.Add(new ScriptBundle("~/Scripts/jQuery").Include(
                "~/Scripts/lib/jquery-1.8.2.js"));

            bundles.Add(new ScriptBundle("~/Scripts/CommonJS").Include(
                "~/Scripts/lib/jquery.easing.1.3.js",
                "~/Scripts/lib/jquery-ui-1.8.24.js",
                "~/Scripts/lib/jquery.unobtrusive-ajax.js",
                "~/Scripts/lib/jquery.validate*",
                "~/Scripts/lib/tipsy/tipsy-1.0.0.js",
                "~/Scripts/lib/msgbox/msgbox-0.0.1.js",
                "~/Scripts/lib/jquery.cookie.js",
                "~/Scripts/lib/Namespace.js",
                "~/Scripts/lib/moment.min.js",
                "~/Scripts/ParlayRMS.js",
                "~/Scripts/_Layout.js",
                "~/Scripts/Validator.js",
                "~/Scripts/lib/Fanex.Grid/numeral.js",
                "~/Scripts/lib/Fanex.Grid/Fanex.Grid.js",
                "~/Scripts/lib/Fanex.Grid/Fanex.Pagination.js",
                "~/Scripts/lib/Multiselect/jquery.multiselect.js"));
        }

        /// <summary>
        /// Registers the member management bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        private static void RegisterMemberManagementBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/MemberManagement").Include(
                "~/Areas/MemberManagement/Views/MemberManagement/Scripts/MemberManagement.js",
                "~/Scripts/LevelUpdater.js"));
            bundles.AddAndInclude(
                "~/Themes/MemberManagement",
                "~/Content/themes/level-updater/layout.less");
        }

        /// <summary>
        /// Registers the report bundles.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        private static void RegisterReportBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Reports/TopWinners").Include(
                "~/Areas/Reports/Views/TopWinners/Scripts/TopWinnerReport.js",
                "~/Scripts/LevelUpdater.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Reports/WinLoss").Include(
                "~/Areas/Reports/Views/WinLoss/Scripts/WinLossReport.js"));
            bundles.Add(new ScriptBundle("~/Scripts/Reports/MemberWinLoss").Include(
                "~/Areas/Reports/Views/MemberWinLoss/Scripts/MemberWinLossReport.js"));
        }


        /// <summary>
        /// Registers the bundles for tools menu.
        /// </summary>
        /// <param name="bundles">The bundle collection.</param>
        private static void RegisterToolsBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/Scripts/Tools/MixParlayMonitor").Include(
                "~/Areas/Tools/Views/MixParlayMonitor/Scripts/MixParlayMonitor.js"));
        }
    }

    /// <summary>
    /// Bundle Collection extension.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Need to review here.")]
    internal static class BundleExtensions
    {
        /// <summary>
        /// Creates a bundle, include the virtual paths to the created bundle, 
        /// and add it to the bundle collection.
        /// </summary>
        /// <param name="bundles">The bundle collection which the virtual paths are included.</param>
        /// <param name="bundleVirtualPath">Virtual path of the created bundle.</param>
        /// <param name="virtualPaths">The virtual paths that needed to add to bundle collection.</param>
        public static void AddAndInclude(
            this BundleCollection bundles,
            string bundleVirtualPath,
            params string[] virtualPaths)
        {
            bundles.Add(new Bundle(bundleVirtualPath)
            {
                Transforms = { new CssTransformer(), new CssMinify() },
                Orderer = new NullOrderer()
            }.Include(virtualPaths));
        }

        /// <summary>
        /// Gets relative path from absolute path.
        /// </summary>
        /// <param name="context">The context using to get the path.</param>
        /// <param name="path">The absolute path.</param>
        /// <returns>The relative path.</returns>
        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
    }
}