using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraLayout;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using WindowsFormsApplication.Utils;

namespace WindowsFormsApplication {
	public partial class EmployeeDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm {
		private IObjectSpace securedObjectSpace;
		private Employee employee;
		private readonly Dictionary<string, string> visibleMembers;
		private readonly SecurityStrategyComplex security;
		private readonly IObjectSpaceProvider objectSpaceProvider;
		public EmployeeDetailForm(Employee employee, SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
			InitializeComponent();
			this.employee = employee;
			this.security = security;
			this.objectSpaceProvider = objectSpaceProvider;
            visibleMembers = new Dictionary<string, string> {
                { nameof(Employee.FirstName), "First Name:" },
                { nameof(Employee.LastName), "Last Name:" },
                { nameof(Employee.Department), "Department:" }
            };
        }
		private void EmployeeDetailForm_Load(object sender, EventArgs e) {
			securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
			if(employee == null) {
				employee = securedObjectSpace.CreateObject<Employee>();
			}
			else {
				employee = securedObjectSpace.GetObject(employee);
				deleteBarButtonItem.Enabled = security.CanDelete(employee);
			}
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
					control.DataBindings.Add(new Binding(nameof(BaseEdit.EditValue), targetObject, memberName, true, DataSourceUpdateMode.OnPropertyChanged));
					control.Enabled = security.CanWrite(targetObject, memberName);
				}
			}
			else {
				control = new ProtectedContentEdit();
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
					control = new LookUpEdit();
					((LookUpEdit)control).Properties.DataSource = securedObjectSpace.GetBindingList<Department>();
					((LookUpEdit)control).Properties.DisplayMember = nameof(Department.Title);
					LookUpColumnInfo column = new LookUpColumnInfo(nameof(Department.Title));
					((LookUpEdit)control).Properties.Columns.Add(column);
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
