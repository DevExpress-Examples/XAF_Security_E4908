using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.XtraEditors;
using System.Configuration;

namespace WindowsFormsApplication {
    public partial class MainForm : DevExpress.XtraBars.Ribbon.RibbonForm {
        IMiddleTierClient<ApplicationDbContext> middleTierClient;
        public MainForm() {
            InitializeComponent();
        }
        private void CreateMiddleTierClient(string userName, string password) {
            middleTierClient?.Dispose();
            middleTierClient = new MiddleTierClientBuilder<ApplicationDbContext>()
                .UseServer(ConfigurationManager.AppSettings["MiddleTierServerEndpoint"])
                .ConfigureOptions(t => t.WaitForMiddleTierServerReady())
                .UsePasswordAuthentication(userName, password)
                .Build();
        }
        private void MainForm_Load(object sender, EventArgs e) {
            ShowLoginForm();
        }
        private void LogoutButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            foreach(Form form in MdiChildren) {
                form.Close();
            }
            Hide();
            string userName = (middleTierClient != null) ? middleTierClient.Security.UserName : "User";
            ShowLoginForm(userName);
        }
        private void ShowLoginForm(string userName = "User") {
            using (LoginForm loginForm = new LoginForm(userName)) {
                while (true) {
                    DialogResult dialogResult = loginForm.ShowDialog();
                    if (dialogResult == DialogResult.OK) {
                        try {
                            CreateMiddleTierClient(loginForm.UserName, loginForm.Password);
                        }
                        catch (Exception ex) {
                            XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            continue;
                        }
                        CreateListForm();
                        Show();
                    }
                    else {
                        Close();
                    }
                    break;
                }
            }
        }
        private void CreateListForm() {
            EmployeeListForm employeeForm = new EmployeeListForm(middleTierClient);
            employeeForm.MdiParent = this;
            employeeForm.WindowState = FormWindowState.Maximized;
            employeeForm.Show();
        }
    }
}
