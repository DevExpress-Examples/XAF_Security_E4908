using DatabaseUpdater;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace WebFormsApplication {
	public class Global : HttpApplication {
		void Application_Start(object sender, EventArgs e) {
			// Code that runs on application startup
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			CreateDemoData(connectionString);
		}
		private static void CreateDemoData(string connectionString) {
			using(var objectSpaceProvider = new XPObjectSpaceProvider(connectionString)) {
				ConnectionHelper.RegisterEntities(objectSpaceProvider);
				using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
					new Updater(objectSpace).UpdateDatabase();
				}
			}
		}
	}
}