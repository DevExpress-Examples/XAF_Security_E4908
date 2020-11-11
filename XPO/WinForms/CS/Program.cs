using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Configuration;
using System.Linq;
using System.Windows.Forms;
using XafSolution.Module.BusinessObjects;

namespace WindowsFormsApplication {
	static class Program {
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() {
			RegisterEntities();
			AuthenticationStandard authentication = new AuthenticationStandard();
			SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
			security.RegisterXPOAdapterProviders();
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			IObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			MainForm mainForm = new MainForm(security, objectSpaceProvider);
			Application.Run(mainForm);
		}
		private static void RegisterEntities() {
			XpoTypesInfoHelper.GetXpoTypeInfoSource();
			XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
			XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
			XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
		}
	}
}
