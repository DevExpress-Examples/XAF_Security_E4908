using System;
using System.Windows.Forms;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;

namespace WindowsFormsApplication {
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm {
        private readonly SecurityStrategyComplex security;
        private readonly IObjectSpaceProvider objectSpaceProvider;
        public MainForm(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
            InitializeComponent();
            this.security = security;
            this.objectSpaceProvider = objectSpaceProvider;
        }
        private void MainForm_Load(object sender, EventArgs e) {
            ShowLoginForm();
        }
        private void LogoutButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            foreach(Form form in MdiChildren) {
                form.Close();
            }
			string userName = security.UserName;
            security.Logoff();
            Hide();
            ShowLoginForm(userName);
        }
        private void ShowLoginForm(string userName = "User") {
            using(LoginForm loginForm = new LoginForm(security, objectSpaceProvider, userName)) {
                DialogResult dialogResult = loginForm.ShowDialog();
                if(dialogResult == DialogResult.OK) {
                    CreateListForm();
                    Show();
                }
                else {
                    Close();
                }
            }
        }
        private void CreateListForm() {
            EmployeeListForm employeeForm = new EmployeeListForm(security, objectSpaceProvider);
            employeeForm.MdiParent = this;
            employeeForm.WindowState = FormWindowState.Maximized;
            employeeForm.Show();
        }
    }
}
