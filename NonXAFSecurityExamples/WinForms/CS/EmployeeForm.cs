using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using XafSolution.Module.BusinessObjects;

namespace WindowsFormsApplication {
	public partial class EmployeeForm : Form {
		WinApplication winApplication;
		public EmployeeForm(WinApplication winApplication) {
			InitializeComponent();
			this.winApplication = winApplication;
			GetData();
		}
		void GetData() {
			IObjectSpace securedObjectSpace = winApplication.osProvider.CreateObjectSpace();
			int counter = 0;
			foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
				employeeGrid.Rows.Add();
				employeeGrid.Rows[counter].Cells["Employee"].Value = employee.FullName;
				if(winApplication.security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, employee, "Department"))) {
					employeeGrid.Rows[counter].Cells["Department"].Value = employee.Department.Title;
				}
				else {
					employeeGrid.Rows[counter].Cells["Department"].Value = "Protected content";
				}
				counter++;
			}
		}

		private void Logoff_button_Click(object sender, EventArgs e) {
			winApplication.security.Logoff();
			winApplication.loginForm.Show();
			Dispose();
		}
	}
}
