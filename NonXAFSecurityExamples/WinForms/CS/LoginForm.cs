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


namespace WindowsFormsApplication {
	public partial class LoginForm : Form {
		WinApplication winApplication;

		public LoginForm(WinApplication winApplication) {
			InitializeComponent();
			this.winApplication = winApplication;

			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
		}
		private void Login_button_Click(object sender, EventArgs e) {
			IObjectSpace nonSecuredObjectSpace = winApplication.osProvider.CreateNonsecuredObjectSpace();
			string userName = loginBox.Text;
			string password = passwordBox.Text;
			winApplication.auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
			try {
				winApplication.security.Logon(nonSecuredObjectSpace);
				EmployeeForm employeeForm = new EmployeeForm(winApplication);
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
