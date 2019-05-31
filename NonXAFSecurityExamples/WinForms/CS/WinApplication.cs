using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XafSolution.Module.BusinessObjects;

namespace WindowsFormsApplication {
	public class WinApplication {
		public LoginForm loginForm;
		public AuthenticationStandard auth;
		public SecuredObjectSpaceProvider osProvider;
		public SecurityStrategyComplex security;
		public void Start() {
			RegisterEntities();
			auth = new AuthenticationStandard();
			security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
			SecurityAdapterHelper.Enable();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			loginForm = new LoginForm(this);
			Application.Run(loginForm);
		}
		private static void RegisterEntities() {
			XpoTypesInfoHelper.GetXpoTypeInfoSource();
			XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
			XafTypesInfo.Instance.RegisterEntity(typeof(Person));
			XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
			XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
		}
	}
}
