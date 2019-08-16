using DevExpress.ExpressApp;
using System;
using System.Windows.Forms;
using DevExpress.ExpressApp.Security;

namespace WindowsFormsApplication {
	public partial class LoginForm : Form {
		private SecurityStrategyComplex security;
		private IObjectSpaceProvider objectSpaceProvider;
		public LoginForm(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			InitializeComponent();
			this.security = security;
			this.objectSpaceProvider = objectSpaceProvider;
		}
		private void Login_button_Click(object sender, EventArgs e) {
			IObjectSpace logonObjectSpace = objectSpaceProvider.CreateObjectSpace();
            string userName = userNameEdit.Text;
            string password = passwordEdit.Text;
			security.Authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
			try {
				security.Logon(logonObjectSpace);
				DialogResult = DialogResult.OK;
				Close();
            }
            catch(Exception ex) {
                MessageBox.Show(
                    ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
	}
}
