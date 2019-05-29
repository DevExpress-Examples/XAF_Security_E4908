using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Persistent.BaseImpl;
using XafSolution.Module.BusinessObjects;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using System.Configuration;

namespace NonXAFSecurityWindowsFormsApp {
	public partial class LoginForm : Form {
		AuthenticationStandard auth;
		SecuredObjectSpaceProvider osProvider;
		SecurityStrategyComplex security;
		public LoginForm() {
			InitializeComponent();
			RegisterEntities();
			auth = new AuthenticationStandard();
			security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
			SecurityAdapterHelper.Enable();
		}
		private void Login_button_Click(object sender, EventArgs e) {
			IObjectSpace nonSecuredObjectSpace = osProvider.CreateNonsecuredObjectSpace();
			string userName = loginBox.Text;
			string password = passwordBox.Text;
			auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
			try {
				security.Logon(nonSecuredObjectSpace);
				EmployeeForm employeeForm = new EmployeeForm(security, osProvider);
				employeeForm.Show();
				Hide();
			}
			catch(Exception ex) {
				MessageBox.Show(
					ex.Message,
					"Error",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
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
