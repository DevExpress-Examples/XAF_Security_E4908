using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.XtraEditors;

namespace WindowsFormsApplication {
    public partial class LoginForm : XtraForm {
        public LoginForm(string userName) {
            InitializeComponent();
            userNameEdit.Text = userName;
        }
        public string UserName => userNameEdit.Text;
        public string Password => passwordEdit.Text;
        private void Login_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }
        private void Cancel_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.Cancel;
        }

        private void UserNameEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e) {
            string message = string.IsNullOrEmpty(userNameEdit.Text) ? "The user name must not be empty. Try Admin or User." : string.Empty;
            dxErrorProvider.SetError(userNameEdit, message);
        }
    }
}
