using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using BusinessObjectsLibrary.BusinessObjects;
using DatabaseUpdater;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;

namespace WindowsFormsApplication {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			TypesInfo typesInfo = new TypesInfo();
			RegisterEntities(typesInfo);
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null);
			CreateDemoData(typesInfo, dataStoreProvider);

			AuthenticationStandard authentication = new AuthenticationStandard();
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), 
				authentication, typesInfo);
			security.RegisterXPOAdapterProviders();
			SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, dataStoreProvider, typesInfo, null);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm mainForm = new MainForm(security, objectSpaceProvider);
			Application.Run(mainForm);
		}
		private static void RegisterEntities(TypesInfo typesInfo) {
			typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
			typesInfo.RegisterEntity(typeof(Employee));
			typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
			typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
		}
		private static void CreateDemoData(TypesInfo typesInfo, IXpoDataStoreProvider dataStoreProvider) {
			using (var objectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, typesInfo, null)) {
				using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
					new Updater(objectSpace).UpdateDatabase();
				}
			}
		}
	}
}
