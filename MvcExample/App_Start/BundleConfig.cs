using System.Web;
using System.Web.Optimization;

namespace MvcExample
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery","https://code.jquery.com/jquery-1.12.4.js").Include(
						"~/Scripts/jquery-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqueryui", "https://code.jquery.com/ui/1.12.1/jquery-ui.js").Include(
						"~/Scripts/jquery-ui-{version}.js"));
			bundles.Add(new ScriptBundle("~/bundles/jqueryval","https://cdnjs.cloudflare.com/ajax/libs/jquery-validate/1.16.0/jquery.validate.js").Include(
						"~/Scripts/jquery.validate*"));
			bundles.Add(new ScriptBundle("~/bundles/toastr", "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js").Include(
						"~/Scripts/toastr.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));
			bundles.Add(new StyleBundle("~/Content/themes/base/jquery-ui", "https://code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css")
				   .Include("~/Content/themes/base/jquery-ui.css"));
			bundles.Add(new StyleBundle("~/Content/toastr", "https://cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css").Include(
					  "~/Content/toastr.css"));
		}
	}
}
