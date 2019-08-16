This example demonstrates how to implement CRUD operations on data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from your Non-XAF application.

### Prerequisites
* Run the XafSolution.Win project and log in with the 'User' or 'Admin' username and empty password to generate a database with business objects from the XafSolution.Module project.
* Add the XafSolution.Module reference to use these classes in your application.

***

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
2. Open the application configuration file. It is an XML file located in the application folder. The Console application configuration file is _App.config_. Add the following line in this file.
	
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
4. Create a **SecuredObjectSpaceProvider** object. It allows you to create a **SecuredObjectSpace** to ensure a secured data access.
	[](#tab/tabid-csharp)
	
	```csharp
	string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
	SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
	```
  
5. Create MainForm and run application
	```csharp
	Application.EnableVisualStyles();
	Application.SetCompatibleTextRenderingDefault(false);
	MainForm mainForm = new MainForm(security, objectSpaceProvider);
	Application.Run(mainForm);
	```
6. To authenticate, create LoginForm as dialog window.
```csharp
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
```
7. Create LoginForm. In this form will heppen authentication. Create userNameEdit and passwordEdit for user input and login_button to implement authentication in security.
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
        	MessageBox.Show(
                ex.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
}
```

8. To show data, create list form as mdi child of MainForm
```csharp
private void CreateListForm() {
	EmployeeListForm employeeForm = new EmployeeListForm();
	employeeForm.MdiParent = this;
	employeeForm.WindowState = FormWindowState.Maximized;
	employeeForm.Show();
}
```

9. Create EmployeeListForm. To access protected data create secured object space
```csharp
securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
```
Bind Xpcollection of Employees to datasourse
```csharp
employeeBindingSource.DataSource = securedObjectSpace.GetObjects<Employee>();
```
Use IsGranted to replase default value on protected cells to Protected content
```csharp
private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
	string fieldName = e.Column.FieldName;
	object targetObject = employeeGridView.GetRow(e.RowHandle);
	if(!security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, targetObject, fieldName))) {
		e.RepositoryItem = new RepositoryItemProtectedContentTextEdit();
	}
}
```

Also, use IsGranted to set Enabled property on New and Delete actions
```csharp
newBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Create));
```
```csharp
deleteBarButtonItem.Enabled = security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Delete, e.Row));
```

10. To implement New and Edit actions create EmployeeDetailForm. Logic to enable/disable actions is same with logic in list form.
To enable/disable editors for every member or replace them with protected content use IsGranted
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
```

> Make sure that the static [EnableRfc2898 and SupportLegacySha512 properties](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System) in your non-XAF application have same values as in the XAF application where passwords were set. Otherwise you won't be able to login.
