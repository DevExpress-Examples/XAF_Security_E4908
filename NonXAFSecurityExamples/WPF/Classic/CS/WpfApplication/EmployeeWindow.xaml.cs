using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using System;
using System.Collections;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using XafSolution.Module.BusinessObjects;
using Window = System.Windows.Window;

namespace WpfApplication {
	/// <summary>
	/// Interaction logic for EmployeeWindow.xaml
	/// </summary>
	public partial class EmployeeWindow : Window {
		LoginWindow loginWindow;
		List<PrintingData> printingDataList = new List<PrintingData>();
		public EmployeeWindow(LoginWindow loginWindow) {
			InitializeComponent();
			this.loginWindow = loginWindow;
			GetData();
		}
		void GetData() {
			IObjectSpace securedObjectSpace = loginWindow.osProvider.CreateObjectSpace();
			foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
				string department = "Protected content";
				if(loginWindow.security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, employee, "Department"))) {
					department = employee.Department.Title;
				}
				employeeGrid.Items.Add(new PrintingData(employee.FullName, department));
			}
		}

		private void Logoff_Button_Click(object sender, RoutedEventArgs e) {
			loginWindow.security.Logoff();
			loginWindow.Show();
			Close();
		}
	}
}
