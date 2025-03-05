using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using WindowsFormsApplication.Utils;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.ApplicationBuilder;

namespace WindowsFormsApplication {
	public partial class EmployeeListForm : DevExpress.XtraBars.Ribbon.RibbonForm {
		private IObjectSpace securedObjectSpace;
		private RepositoryItemProtectedContentTextEdit protectedContentTextEdit;
		private readonly IMiddleTierClient<ApplicationDbContext> middleTierClient;

        public EmployeeListForm(IMiddleTierClient<ApplicationDbContext> middleTierClient) {
			InitializeComponent();
			this.middleTierClient = middleTierClient;
            this.securedObjectSpace = middleTierClient.CreateObjectSpace();
			this.Disposed += EmployeeListForm_Disposed;
        }

        private void EmployeeListForm_Disposed(object sender, EventArgs e) {
			securedObjectSpace.Dispose();
        }

        private void EmployeeListForm_Load(object sender, EventArgs e) {
			employeeGrid.DataSource = securedObjectSpace.GetBindingList<Employee>();
			newBarButtonItem.Enabled = middleTierClient.Security.CanCreate<Employee>(securedObjectSpace);
			protectedContentTextEdit = new RepositoryItemProtectedContentTextEdit();
		}
		private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
			string fieldName = e.Column.FieldName;
            object targetObject = employeeGridView.GetRow(e.RowHandle);
            // The targetObject is null for some rows (column header row, auto filter row and others).
            if((targetObject != null) && !middleTierClient.Security.CanRead(securedObjectSpace, targetObject, fieldName)) {
				e.RepositoryItem = protectedContentTextEdit;
			}
		}
		private void CreateDetailForm(Employee employee = null) {
            EmployeeDetailForm detailForm = new EmployeeDetailForm(employee, middleTierClient) {
                MdiParent = MdiParent,
                WindowState = FormWindowState.Maximized
            };
            detailForm.Show();
            detailForm.FormClosing += (s, e) => { 
                securedObjectSpace.Refresh();
				int rowHandle = employeeGridView.FocusedRowHandle;
                employeeGrid.DataSource = securedObjectSpace.GetBindingList<Employee>();
				employeeGridView.FocusedRowHandle = rowHandle;
            };
		}
        private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
			if(e.Clicks == 2) {
				EditEmployee();
			}
		}
		private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
			deleteBarButtonItem.Enabled = e.Row != null && middleTierClient.Security.CanDelete(securedObjectSpace, e.Row);
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
