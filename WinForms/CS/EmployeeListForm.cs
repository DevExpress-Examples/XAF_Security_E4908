using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Linq;
using System.Windows.Forms;
using XafSolution.Module.BusinessObjects;

namespace WindowsFormsApplication {
	public partial class EmployeeListForm : DevExpress.XtraBars.Ribbon.RibbonForm {
		private IObjectSpace securedObjectSpace;
		private SecurityStrategyComplex security;
		private IObjectSpaceProvider objectSpaceProvider;
		public EmployeeListForm() {
			InitializeComponent();
		}
		private void EmployeeListForm_Load(object sender, EventArgs e) {
			security = ((MainForm)MdiParent).Security;
			objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
			securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
			employeeBindingSource.DataSource = securedObjectSpace.GetObjects<Employee>();
			newBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Create));
		}
		private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
			string fieldName = e.Column.FieldName;
			object targetObject = employeeGridView.GetRow(e.RowHandle);
			if(!security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, targetObject, fieldName))) {
				e.RepositoryItem = new RepositoryItemProtectedContentTextEdit();
			}
		}
		private void CreateDetailForm(Employee employee = null) {
			EmployeeDetailForm detailForm = new EmployeeDetailForm(employee);
			detailForm.MdiParent = MdiParent;
			detailForm.WindowState = FormWindowState.Maximized;
			detailForm.Show();
            detailForm.FormClosing += DetailForm_FormClosing;
		}
        private void DetailForm_FormClosing(object sender, FormClosingEventArgs e) {
            XPBaseCollection collection = (XPBaseCollection)employeeBindingSource.DataSource;
            collection.Reload();
        }
        private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
			if(e.Clicks == 2) {
				Employee employee = employeeGridView.GetRow(employeeGridView.FocusedRowHandle) as Employee;
				CreateDetailForm(employee);
			}
		}
		private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
			deleteBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Delete, e.Row));
		}
		private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			CreateDetailForm();
		}
		private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			object cellObject = employeeGridView.GetRow(employeeGridView.FocusedRowHandle);
			securedObjectSpace.Delete(cellObject);
			securedObjectSpace.CommitChanges();
		}
	}
}
