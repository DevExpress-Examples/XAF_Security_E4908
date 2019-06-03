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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XafSolution.Module.BusinessObjects;
using Window = System.Windows.Window;

namespace WpfApplication {
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window {
		public AuthenticationStandard auth;
		public SecuredObjectSpaceProvider osProvider;
		public SecurityStrategyComplex security;
		public LoginWindow() {
			InitializeComponent();
			RegisterEntities();
			auth = new AuthenticationStandard();
			security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
			string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
			osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
			SecurityAdapterHelper.Enable();

			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;
		}

		private void LoginButton_Click(object sender, RoutedEventArgs e) {
			IObjectSpace nonSecuredObjectSpace = osProvider.CreateNonsecuredObjectSpace();
			string userName = LoginBox.Text;
			string password = PasswordBox.Text;
			auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
			try {
				security.Logon(nonSecuredObjectSpace);
				//EmployeeWindow employeeForm = new EmployeeWindow(security, osProvider);
				EmployeeWindow employeeForm = new EmployeeWindow(this);
				employeeForm.Show();
				Hide();
			}
			catch(Exception ex) {
				MessageBox.Show(
					ex.Message,
					"Error",
					MessageBoxButton.OK,
					MessageBoxImage.Error);
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