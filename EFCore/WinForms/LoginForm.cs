using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.XtraEditors;

namespace WindowsFormsApplication {
    public partial class LoginForm : XtraForm {
        private readonly SecurityStrategyComplex security;
        private readonly IObjectSpaceProvider objectSpaceProvider;
        public LoginForm(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider, string userName) {
            InitializeComponent();
            this.security = security;
            this.objectSpaceProvider = objectSpaceProvider;
            userNameEdit.Text = userName;
        }
        private void Login_Click(object sender, EventArgs e) {
            IObjectSpace logonObjectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
            string userName = userNameEdit.Text;
            string password = passwordEdit.Text;
            security.Authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            try {
                security.Logon(logonObjectSpace);
                DialogResult = DialogResult.OK;
            }
            catch(Exception ex) {
                XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Cancel_Click(object sender, EventArgs e) {
            Application.Exit();
        }

        private void UserNameEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            string message = string.IsNullOrEmpty(userNameEdit.Text) ? "The user name must not be empty. Try Admin or User." : string.Empty;
            dxErrorProvider.SetError(userNameEdit, message);
        }
    }
}
