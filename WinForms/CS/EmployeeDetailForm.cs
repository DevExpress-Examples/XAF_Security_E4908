using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Xpo;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using XafSolution.Module.BusinessObjects;

namespace WindowsFormsApplication {
	public partial class EmployeeDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm {
		private IObjectSpace securedObjectSpace;
		private SecurityStrategyComplex security;
		private IObjectSpaceProvider objectSpaceProvider;
		private Employee employee;
		private List<string> visibleMembers = new List<string>() {
				nameof(Employee.FirstName),
				nameof(Employee.LastName),
				nameof(Employee.Department)
			};
		public EmployeeDetailForm(Employee employee) {
			InitializeComponent();
			this.employee = employee;
		}
		private void EmployeeDetailForm_Load(object sender, EventArgs e) {
			security = ((MainForm)MdiParent).Security;
			objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
			securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
			if(employee == null) {
				employee = securedObjectSpace.CreateObject<Employee>();
			}
			else {
				employee = securedObjectSpace.GetObject(employee);
				deleteBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Delete, employee));
			}
			employeeBindingSource.DataSource = employee;
			CreateControls();
		}
		private void CreateControls() {
			foreach(string memberName in visibleMembers) {
				CreateControl(dataLayoutControl1.AddItem(), employee, memberName);
			}
		}
		private void CreateControl(LayoutControlItem layout, object targetObject, string memberName) {
			layout.Text = memberName;
			Type type = targetObject.GetType();
			BaseEdit control;
			if(security.IsGranted(new PermissionRequest(securedObjectSpace, type, SecurityOperations.Read, targetObject, memberName))) {
				control = GetControl(type, memberName);
				if(control != null) {
					control.DataBindings.Add(new Binding("EditValue", employeeBindingSource, memberName, true, DataSourceUpdateMode.OnPropertyChanged));
					control.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, type, SecurityOperations.Write, targetObject, memberName));
				}
			}
			else {
				control = new ProtectedContentEdit();
                control.Enabled = false;
			}
			dataLayoutControl1.Controls.Add(control);
			layout.Control = control;
		}
		private BaseEdit GetControl(Type type, string memberName) {
			BaseEdit control = null;
			ITypeInfo typeInfo = securedObjectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == type.Name);
			IMemberInfo memberInfo = typeInfo.Members.FirstOrDefault(t => t.Name == memberName);
			if(memberInfo != null) {
				if(memberInfo.IsAssociation) {
					control = new ComboBoxEdit();
					((ComboBoxEdit)control).Properties.Items.AddRange(securedObjectSpace.GetObjects<Department>() as XPCollection<Department>);
				}
				else {
					control = new TextEdit();
				}
			}
			return control;
		}
		private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			securedObjectSpace.CommitChanges();
			Close();
		}
		private void CloseBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			Close();
		}
		private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			securedObjectSpace.Delete(employee);
			securedObjectSpace.CommitChanges();
			Close();
		}
	}
}
