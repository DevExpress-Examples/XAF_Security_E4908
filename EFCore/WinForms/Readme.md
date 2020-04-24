This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Windows Forms application. You will also learn how to execute Create, Read, Update and Delete data operations taking into account security permissions.

>For simplicity, the instructions include only C# code snippets. For the complete C# code, see the [CS](CS) sub-directory.

## Prerequisites
- [.NET Core SDK 3.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Two unified installers for .NET Framework and .NET Core 3.1 Desktop Development](https://www.devexpress.com/Products/Try/).
  - We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.

***

## Step 1. Create a Database and Populate It with User, Role, Permission and Other Data

-  Open the [EFCore/DatabaseUpdater/App.config](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/tree/20.1/EFCore/DatabaseUpdater/App.config) file and modify it so that `DBSERVER` refers to your database server name or its IP address (for a local database server, use `localhost`, `(local)` or `.`):
	
[](#tab/tabid-xml)
	
```xml
<configuration>
  <connectionStrings>
    <add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=EFCoreTestDB;Integrated Security=True"/>
```
    
- Build and run the *DatabaseUpdater.NetCore* project. The console application will generate a database and populate it with business objects, security roles, and users. For more information, see [Predefined Users, Roles and Permissions](https://docs.devexpress.com/eXpressAppFramework/119065/concepts/security-system/predefined-users-roles-and-permissions).


## Step 2. Initialize a Secured Data Store and Authentication Options
- In *YourWinFormsApplication/Program.cs*, create a `SecurityStrategyComplex` instance using [AuthenticationStandard](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.AuthenticationStandard) (a simple Forms Authentication with a login and password) and password options ([EnableRfc2898 and SupportLegacySha512](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System)).
	
	[](#tab/tabid-csharp)
	
``` csharp
static void Main() {
    AuthenticationStandard authentication = new AuthenticationStandard();
    SecurityStrategyComplex security = new SecurityStrategyComplex(
        typeof(PermissionPolicyUser), 
        typeof(PermissionPolicyRole), 
        authentication
    );
    PasswordCryptographer.EnableRfc2898 = true;
    PasswordCryptographer.SupportLegacySha512 = false;
```

- Create a `SecuredEFCoreObjectSpaceProvider` instance using the `EFCoreDatabaseProviderHandler` delegate and the `UseSqlServer` extension

	[](#tab/tabid-csharp)
	
``` csharp
string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, 
typeof(ApplicationDbContext), XafTypesInfo.Instance, connectionString,
	(builder, connectionString) =>
          builder.UseSqlServer(connectionString));
```
- In *YourWinFormsApplication/App.config*, add the same connection string as in **Step 1**.

This provider allows you to create secured [IObjectSpace](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) instances to perform secured CRUD (create-read-update-delete) operations. Object Space is an ORM-independent implementation of the well-known Repository and Unit Of Work design patterns (for instance, `SecuredEFCoreObjectSpace` is an IObjectSpace implementation for EF Core that wraps DbContext).

## Step 3: Implement the Main and Login Forms

- In *YourWinFormsApplication/Program.cs*, create [MainForm](CS/MainForm.cs) using a custom constructor. `MainForm` is the MDI parent form for [EmployeeListForm](CS/EmployeeListForm.cs) and [EmployeeDetailForm](CS/EmployeeDetailForm.cs).

``` csharp
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
MainForm mainForm = new MainForm(security, objectSpaceProvider);
Application.Run(mainForm);
```

- Display the [LoginForm](CS/LoginForm.cs) in the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event handler. If the dialog returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult), `EmployeeListForm` is created and shown.

``` csharp
private void MainForm_Load(object sender, EventArgs e) {
    ShowLoginForm();
}
private void ShowLoginForm(string userName = "User") {
    using(LoginForm loginForm = new LoginForm(Security, ObjectSpaceProvider, userName)) {
	DialogResult dialogResult = loginForm.ShowDialog();
	if(dialogResult == DialogResult.OK) {
	    CreateListForm();
	    Show();
	}
	else {
	    Close();
	}
    }
}
private void CreateListForm() {
    EmployeeListForm employeeForm = new EmployeeListForm(security, objectSpaceProvider) {
	MdiParent = this,
	WindowState = FormWindowState.Maximized
    };
    employeeForm.Show();
}
```
- Handle the RibbonControl's Logout item `ItemClick` event.

``` csharp
private void LogoutButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    foreach(Form form in MdiChildren) {
        form.Close();
    }
    string userName = Security.UserName;
    Security.Logoff();
    Hide();
    ShowLoginForm(userName);
}
```
	
- [LoginForm](CS/LoginForm.cs) contains two `TextEdit` controls for user name and password, and the **Log In** button that attempts to log the user into the security system and returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult?view=netframework-4.8) if logon was successful.

``` csharp
private void Login_Click(object sender, EventArgs e) {
    IObjectSpace logonObjectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
    string userName = userNameEdit.Text;
    string password = passwordEdit.Text;
    security.Authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
    try {
        security.Logon(logonObjectSpace);
        DialogResult = DialogResult.OK;
        Close();
    }
    catch(Exception ex) {
        XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

## Step 4: Implement the List Form
- [EmployeeListForm](CS/EmployeeListForm.cs) contains a [DevExpress Grid View](https://docs.devexpress.com/WindowsForms/3464/Controls-and-Libraries/Data-Grid/Views/Grid-View) that displays a list of all Employees. Handle the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event and do the following: 

   - Create a `SecuredEFCoreObjectSpace` instance to access protected data and use its [data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, `IObjectSpace.GetObjects`).

   - Set the grid's `DataSource` property to [BindingList<Employee>](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.changetracking.localview-1.tobindinglist?view=efcore-2.1). You can see the code of the `GetBindingList<TEntity>` method in the [CS](CS/Utils/ObjectSpaceHelper.cs) file.

   - Call the `CanCreate` method to check Create operation availability and thus determine whether the **New** button can be enabled.
		
``` csharp
private void EmployeeListForm_Load(object sender, EventArgs e) {
    securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
    employeeGrid.DataSource = securedObjectSpace.GetBindingList<Employee>();
    newBarButtonItem.Enabled = security.CanCreate<Employee>();
    protectedContentTextEdit = new RepositoryItemProtectedContentTextEdit();
}
```	

- Handle the [GridView.CustomRowCellEdit](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Grid.GridView.CustomRowCellEdit) event and check Read operation availability. If not available, use the `RepositoryItemProtectedContentTextEdit` object  which displays the 'Protected Content' placeholder.
		
``` csharp
private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
    string fieldName = e.Column.FieldName;
    object targetObject = employeeGridView.GetRow(e.RowHandle);
    if (!security.CanRead(targetObject, fieldName)) {
        e.RepositoryItem = protectedContentTextEdit;
    }
}
```
Note that `SecuredEFCoreObjectSpace` returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the `SecurityStrategy.CanRead` method to determine when to mask default values with the **Protected Content** placeholder in the UI.
		
- Handle the [FocusedRowObjectChanged](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Base.ColumnView.FocusedRowObjectChanged) event and use the `SecurityStrategy.CanDelete` method to check Delete operation availability and thus determine if the **Delete** button can be enabled.
		
``` csharp
private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
    deleteBarButtonItem.Enabled = security.CanDelete(e.Row);
}
```
- Delete the current object in the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler.
	
``` csharp
private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    object cellObject = employeeGridView.GetRow(employeeGridView.FocusedRowHandle);
    securedObjectSpace.Delete(cellObject);
    securedObjectSpace.CommitChanges();
}
```
		
- Create and show `EmployeeDetailForm`.
		
``` csharp
private void CreateDetailForm(Employee employee = null) {
    EmployeeDetailForm detailForm = new EmployeeDetailForm(employee, security, objectSpaceProvider) {
	MdiParent = MdiParent,
	WindowState = FormWindowState.Maximized
    };
    detailForm.Show();
    detailForm.FormClosing += DetailForm_FormClosing;
}
```

We want to create `EmployeeDetailForm` in three cases: 
    
   - When a user clicks the **New** button:
			
``` csharp
private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    CreateDetailForm();
}
```

   - When a user clicks the **Edit** button that passes the current row handle as a parameter to the `CreateDetailForm` method:
   
``` csharp
private void EditEmployee() {
    Employee employee = employeeGridView.GetRow(employeeGridView.FocusedRowHandle) as Employee;
    CreateDetailForm(employee);
}

private void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    EditEmployee();
}
```
   - When a user double-clicks a grid row: 
		
``` csharp
private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
    if(e.Clicks == 2) {
        EditEmployee();
    }
}
```

## Step 5: Implement the Detail Form

- [EmployeeDetailForm](CS/EmployeeDetailForm.cs) contains detailed information on the Employee object. Perform the following operation in the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event handler: 
		
	- Create a `SecuredEFCoreObjectSpace` instance to get the current Employee object or create a new one.
	- Use the `SecurityStrategy.CanDelete` method to check Delete operation availability and thus determine if the **Delete** button can be enabled (note that this button is always disabled if you create new object).
		
``` csharp
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
```	
- The `AddControls` method creates controls for all members declared in the `visibleMembers` dictionary.
		
``` csharp
private Dictionary<string, string> visibleMembers;
// ...
public EmployeeDetailForm(Employee employee) {
    // ...
    visibleMembers = new Dictionary<string, string>();
    visibleMembers.Add(nameof(Employee.FirstName), "First Name:");
    visibleMembers.Add(nameof(Employee.LastName), "Last Name:");
    visibleMembers.Add(nameof(Employee.Department), "Department:");
}
// ...
private void AddControls() {
    foreach(KeyValuePair<string, string> pair in visibleMembers) {
        string memberName = pair.Key;
	string caption = pair.Value;
	AddControl(dataLayoutControl1.AddItem(), employee, memberName, caption);
    }
}
```
- The `AddControl` method creates a control for a specific member. Use the `SecurityStrategy.CanRead` method to check Read operation availability. If not available, create and disable the `ProtectedContentEdit` control which displays the 'Protected Content' placeholder. Otherwise: 
		
	- Call the `GetControl` method to create an appropriate control depending on the member type. We use the [LookUpEdit](https://documentation.devexpress.com/WindowsForms/DevExpress.XtraEditors.LookUpEdit.class) control for the associated property Department.
	- Add a binding to the [Control.DataBindings](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.databindings?view=netframework-4.8) collection.
	- Use the `SecurityStrategy.CanWrite` method to check Write operation availability and thus determine whether the control should be enabled.
		
``` csharp
private void AddControl(LayoutControlItem layout, object targetObject, string memberName, string caption) {
    layout.Text = caption;
    Type type = targetObject.GetType();
    BaseEdit control;
    if(security.CanRead(targetObject, memberName)) {
        control = GetControl(type, memberName);
        if(control != null) {
            control.DataBindings.Add(
	    new Binding(nameof(BaseEdit.EditValue), targetObject, memberName, true, DataSourceUpdateMode.OnPropertyChanged));
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
```
		
- Use `SecuredEFCoreObjectSpace` to save all changes to the database in the [saveBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler and delete the current object in the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler.
		
``` csharp
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

## Step 6: Run and Test the App
 - Log in under **User** with an empty password.
   
   ![](/images/WinForms_LoginForm.png)

 - Notice that secured data is displayed as **Protected Content**.
   ![](/images/WinForms_MainForm.png)

 - Press the **Log Out** button and log in under **Admin** to see all records.