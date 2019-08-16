This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF Windows Forms application. Also, with this example you can see how to perform create, write and delete operations with data taking into account security permissions.
>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.


### Prerequisites
* Run the XafSolution.Win project and log in with the 'User' or 'Admin' username and empty password to generate a database with business objects from the XafSolution.Module project.
* Add the XafSolution.Module reference to use these classes in your application.

***
### Main implementation steps

1. Initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system and register the business objects that you will access from your code.
	
	[](#tab/tabid-csharp)
	
	```csharp
	using DevExpress.ExpressApp;
	using DevExpress.Persistent.BaseImpl.PermissionPolicy;
	//...
	XpoTypesInfoHelper.GetXpoTypeInfoSource();
	XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
	XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
	XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
	```
2. Open the application configuration file. It is an XML file located in the application folder. The Windows Forms application configuration file is _App.config_. Add the following line in this file.
	
	[](#tab/tabid-xml)
	
	```xml
	<add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"/>
	```
	
	Substitute "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.
	
3. Initialize the Security System.
	
	[](#tab/tabid-csharp)
	
	```csharp
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
    security.RegisterXPOAdapterProviders();
	```
4. Create a `SecuredObjectSpaceProvider` object. It allows you to create a `SecuredObjectSpace` instances to ensure a secured data access.
	[](#tab/tabid-csharp)
	
	```csharp
	string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
	```

5. Create [MainForm](MainForm.cs) and run application
	```csharp
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);
	MainForm mainForm = new MainForm(security, objectSpaceProvider);
	Application.Run(mainForm);
	```
	The `MainForm` is the MDI parent form for the [EmployeeListForm](EmployeeListForm.cs) and the [EmployeeDetailForm](EmployeeDetailForm.cs). The `MainForm` shows the [LoginForm](LoginForm.cs) on the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event as a dialog and if the dialog returned [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult), the `EmployeeListForm` will be created and shown.
	```csharp
	private void MainForm_Load(object sender, EventArgs e) {
		ShowLoginForm();
	}
	private void ShowLoginForm() {
		LoginForm loginForm = new LoginForm(Security, ObjectSpaceProvider);
		DialogResult dialogResult = loginForm.ShowDialog();
		if(dialogResult == DialogResult.OK) {
			CreateListForm();
			Show();
		}
		else {
			Close();
		}
	}
	private void CreateListForm() {
		EmployeeListForm employeeForm = new EmployeeListForm();
		employeeForm.MdiParent = this;
		employeeForm.WindowState = FormWindowState.Maximized;
		employeeForm.Show();
	}
	```
	Also, it contains the main RibbonControl with the Logoff ribbon item.
	```csharp
	private void LogoffButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
		foreach(Form form in MdiChildren) {
			form.Close();
		}
		Security.Logoff();
		Hide();
		ShowLoginForm();
	}
	```
	
6. The [LoginForm](LoginForm.cs) contains two TextBox controls so the user can enter the username and the password, and the Login button that performs login into the security and returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult?view=netframework-4.8) if the user logs in successfully.
	```csharp
	private void Login_button_Click(object sender, EventArgs e) {
		IObjectSpace logonObjectSpace = objectSpaceProvider.CreateObjectSpace();
        	string userName = userNameEdit.Text;
        	string password = passwordEdit.Text;
		security.Authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
		try {
			security.Logon(logonObjectSpace);
			DialogResult = DialogResult.OK;
			Close();
        	}
        	catch(Exception ex) {
			MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
	```

7. The [EmployeeListForm](EmployeeListForm.cs) contains the [DevExpress Grid View](https://docs.devexpress.com/WindowsForms/3464/Controls-and-Libraries/Data-Grid/Views/Grid-View) which displays a list of all Employees. 
	
	7.1. Handle the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event and: 
	- Create the `SecuredObjectSpace` instance to access the protected data
	- Set [XPBindingSource.DataSource](https://docs.devexpress.com/XPO/DevExpress.Xpo.XPBindingSource.DataSource) with the Employees collection obtained from the secured object space
	- Perform the [IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) request to check the create operation and define if the New button will be enabled or not.
		
		```csharp
		private void EmployeeListForm_Load(object sender, EventArgs e) {
			security = ((MainForm)MdiParent).Security;
			objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
			securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
			employeeBindingSource.DataSource = securedObjectSpace.GetObjects<Employee>();
			newBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Create));
		}
		```	
	7.2. Handle the [GridView.CustomRowCellEdit](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Grid.GridView.CustomRowCellEdit) and perform IsGranted request to check the read operation. In case it returns false, replace default value of protected cells to "Protected content".
		
		```csharp
		private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
			string fieldName = e.Column.FieldName;
			object targetObject = employeeGridView.GetRow(e.RowHandle);
			if(!security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, targetObject, fieldName))) {
				e.RepositoryItem = new RepositoryItemProtectedContentTextEdit();
			}
		}
		```
	7.3. Handle the [FocusedRowObjectChanged](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Base.ColumnView.FocusedRowObjectChanged) event and perform the IsGranted request to check the delete operation and define if the Delete button will be enabled or not.
		
		```csharp
		private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
			deleteBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Delete, e.Row));
		}
		```
	Delete the current object on the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler.
	
		```csharp
		private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			object cellObject = employeeGridView.GetRow(employeeGridView.FocusedRowHandle);
			securedObjectSpace.Delete(cellObject);
			securedObjectSpace.CommitChanges();
		}
		```
		
	7.4. Create and show `EmployeeDetailForm`
		
		```csharp
		private void CreateDetailForm(Employee employee = null) {
			EmployeeDetailForm detailForm = new EmployeeDetailForm(employee);
			detailForm.MdiParent = MdiParent;
			detailForm.WindowState = FormWindowState.Maximized;
			detailForm.Show();
			detailForm.FormClosing += DetailForm_FormClosing;
		}
		```
		
	We need to create `EmployeeDetailForm` in two cases: 
	- When the user clicks on the New button
			
			```csharp
			private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
				CreateDetailForm();
			}	
			```
			
	- When the user performs a row double-click
		
			```csharp
			private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
				if(e.Clicks == 2) {
					Employee employee = employeeGridView.GetRow(employeeGridView.FocusedRowHandle) as Employee;
					CreateDetailForm(employee);
				}
			}
			```			
	Pass the current row handle as a parameter to the `CreateDetailForm` method
	
8. [EmployeeDetailForm](EmployeeDetailForm.cs) contains detailed representation of the Employee object
	
	8.1. Handle the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event and: 
		
	- Create the `SecuredObjectSpace` instance to get the current Employee object or create the new one
	- Perform the [IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) request to check the delete operation and define if the Delete button will be enabled or not. The Delete button is always disabled when you create new object.
	- Set [XPBindingSource.DataSource](https://docs.devexpress.com/XPO/DevExpress.Xpo.XPBindingSource.DataSource) with the Employee object
		
		```csharp
		private void EmployeeDetailForm_Load(object sender, EventArgs e) {
			security = ((MainForm)MdiParent).Security;
			objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
			securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
			if(employee == null) {
				employee = securedObjectSpace.CreateObject<Employee>();
			}
			else {
				employee = securedObjectSpace.GetObject(employee);
				deleteButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Delete, employee));
			}
			employeeBindingSource.DataSource = employee;
			CreateControls();
		}
		```	
	8.2. The `CreateControls` method creates controls for all members which is declared in the `visibleMembers` collection
		
		```csharp
		private List<string> visibleMembers = new List<string>() {
				nameof(Employee.FirstName),
				nameof(Employee.LastName),
				nameof(Employee.Department)
			};
		// ...
		private void CreateControls() {
			foreach(string memberName in visibleMembers) {
				CreateControl(dataLayoutControl1.AddItem(), employee, memberName);
			}
		}
		```
		
	8.3. The `CreateControl` method creates a control for the specific member
		
	Perform the IsGranted request to check the read operation. If it is denied, create and disable the `ProtectedContentEdit` control which displayed the "Protected Content" placeholder, 
		otherwise: 
		
	- Call the `GetControl` method to create the appropriate control depending of the member type. We use the [ComboBoxEdit](https://docs.devexpress.com/WindowsForms/614/controls-and-libraries/editors-and-simple-controls/simple-editors/concepts/dropdown-editors/combo-box-editors#comboboxedit-control) control for the Department associated property.
	- Add a binding to the [Control.DataBindings](https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control.databindings?view=netframework-4.8) collection.
	- Perform the IsGranted request to check the write operation and define if the control will be enabled or not.
		
		```csharp
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
		```
		
	8.4. Use `SecuredObjectSpace` to commit all changes to database on the [saveBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler and delete the current object on the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler
		
		```csharp
		private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			securedObjectSpace.CommitChanges();
			Close();
		}
		private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
			securedObjectSpace.Delete(employee);
			securedObjectSpace.CommitChanges();
			Close();
		}
		```

> Make sure that the static [EnableRfc2898 and SupportLegacySha512 properties](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System) in your non-XAF application have same values as in the XAF application where passwords were set. Otherwise you won't be able to login.
