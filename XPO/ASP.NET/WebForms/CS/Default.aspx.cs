using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using XafSolution.Module.BusinessObjects;

namespace WebFormsApplication {
	public partial class _Default : System.Web.UI.Page {
		private IObjectSpace objectSpace;
		private SecurityStrategyComplex security;
		private XPObjectSpaceProvider objectSpaceProvider;
		protected void Page_Init(object sender, EventArgs e) {
			security = ConnectionHelper.GetSecurity(typeof(IdentityAuthenticationProvider).Name, HttpContext.Current.User.Identity);
			objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(security);
			IObjectSpace logonObjectSpace = objectSpaceProvider.CreateObjectSpace();
			security.Logon(logonObjectSpace);
			objectSpace = objectSpaceProvider.CreateObjectSpace();
			EmployeeDataSource.Session = ((XPObjectSpace)objectSpace).Session;
			DepartmentDataSource.Session = ((XPObjectSpace)objectSpace).Session;
			EmployeeGrid.SettingsText.PopupEditFormCaption = "Employee";
			EmployeeGrid.SettingsPopup.EditForm.Width = 1000;
		}
		protected void Page_Unload(object sender, EventArgs e) {
			objectSpace.Dispose();
			security.Dispose();
			objectSpaceProvider.Dispose();
		}
		protected void EmployeeGrid_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e) {
			objectSpace.CommitChanges();
		}
		protected void EmployeeGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e) {
			objectSpace.CommitChanges();
		}
		protected void EmployeeGrid_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e) {
			objectSpace.CommitChanges();
		}
		protected void EmployeeGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e) {
			Employee employee = objectSpace.GetObjectByKey<Employee>(e.KeyValue);
            string memberName = GetMemberName(e.Column);
            if (!security.CanRead(employee, memberName)) {
				e.Editor.Value = "*******";
				e.Editor.Enabled = false;
			}
			else if(!security.CanWrite(employee, memberName)) {
				e.Editor.Enabled = false;
			}
		}
		protected void EmployeeGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e) {
			if(e.ButtonType == ColumnCommandButtonType.New) {
				if(!security.CanCreate<Employee>()) {
					e.Text = string.Empty;
				}
			}
			if(e.ButtonType == ColumnCommandButtonType.Delete) {
				Employee employee = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Employee;
				e.Visible = security.CanDelete(employee);
			}
		}
		protected void EmployeeGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e) {
			Employee employee = objectSpace.GetObjectByKey<Employee>(e.KeyValue);
            string memberName = GetMemberName(e.DataColumn);
			if(!security.CanRead(employee, memberName)) {
				e.Cell.Text = "*******";
			}
		}
		protected void LogoutButton_Click(object sender, EventArgs e) {
			FormsAuthentication.SignOut();
			FormsAuthentication.RedirectToLoginPage();
		}
        private string GetMemberName(GridViewDataColumn column) {
            return column?.FieldName.Split('!')[0];
        }
	}
}