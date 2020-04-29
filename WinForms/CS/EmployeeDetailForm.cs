using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
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
		private Dictionary<string, string> visibleMembers;
		public EmployeeDetailForm(Employee employee) {
			InitializeComponent();
			this.employee = employee;
			visibleMembers = new Dictionary<string, string>();
			visibleMembers.Add(nameof(Employee.FirstName), "First Name:");
			visibleMembers.Add(nameof(Employee.LastName), "Last Name:");
			visibleMembers.Add(nameof(Employee.Department), "Department:");
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
				deleteBarButtonItem.Enabled = security.CanDelete(employee);
			}
			employeeBindingSource.DataSource = employee;
			AddControls();
		}
		private void AddControls() {
			foreach(KeyValuePair<string, string> pair in visibleMembers) {
				string memberName = pair.Key;
				string caption = pair.Value;
                AddControl(dataLayoutControl1.AddItem(), employee, memberName, caption);
            }
		}
        private void AddControl(LayoutControlItem layout, object targetObject, string memberName, string caption) {
            layout.Text = caption;
            Type type = targetObject.GetType();
            BaseEdit control;
			if(security.CanRead(targetObject, memberName)) {
				control = GetControl(type, memberName);
				if(control != null) {
					control.DataBindings.Add(new Binding("EditValue", employeeBindingSource, memberName, true, DataSourceUpdateMode.OnPropertyChanged));
					control.Enabled = security.CanWrite(targetObject, memberName);
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
			ITypeInfo typeInfo = securedObjectSpace.TypesInfo.FindTypeInfo(type);
			IMemberInfo memberInfo = typeInfo.FindMember(memberName);
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
