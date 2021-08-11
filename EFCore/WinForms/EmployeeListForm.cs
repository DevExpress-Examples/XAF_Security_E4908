using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Windows.Forms;
using WindowsFormsApplication.Utils;

namespace WindowsFormsApplication {
	public partial class EmployeeListForm : DevExpress.XtraBars.Ribbon.RibbonForm {
		private IObjectSpace securedObjectSpace;
		private RepositoryItemProtectedContentTextEdit protectedContentTextEdit;
		private readonly SecurityStrategyComplex security;
		private readonly IObjectSpaceProvider objectSpaceProvider;
		public EmployeeListForm(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			InitializeComponent();
			this.security = security;
			this.objectSpaceProvider = objectSpaceProvider;
		}
		private void EmployeeListForm_Load(object sender, EventArgs e) {
			securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
			employeeGrid.DataSource = securedObjectSpace.GetBindingList<Employee>();
			newBarButtonItem.Enabled = security.CanCreate<Employee>();
			protectedContentTextEdit = new RepositoryItemProtectedContentTextEdit();
		}
		private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
			string fieldName = e.Column.FieldName;
            object targetObject = employeeGridView.GetRow(e.RowHandle);
            if (!security.CanRead(targetObject, fieldName)) {
				e.RepositoryItem = protectedContentTextEdit;
			}
		}
		private void CreateDetailForm(Employee employee = null) {
            EmployeeDetailForm detailForm = new EmployeeDetailForm(employee, security, objectSpaceProvider) {
                MdiParent = MdiParent,
                WindowState = FormWindowState.Maximized
            };
            detailForm.Show();
            detailForm.FormClosing += (s, e) => { 
                securedObjectSpace.Refresh();
                employeeGrid.DataSource = securedObjectSpace.GetBindingList<Employee>();
            };
		}
        private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
			if(e.Clicks == 2) {
				EditEmployee();
			}
		}
		private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
			deleteBarButtonItem.Enabled = security.CanDelete(e.Row);
		}
		private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			CreateDetailForm();
		}
		private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			object cellObject = employeeGridView.GetRow(employeeGridView.FocusedRowHandle);
			securedObjectSpace.Delete(cellObject);
			securedObjectSpace.CommitChanges();
		}
        private void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			EditEmployee();
		}
		private void EditEmployee() {
			Employee employee = employeeGridView.GetRow(employeeGridView.FocusedRowHandle) as Employee;
			CreateDetailForm(employee);
		}
    }
}
