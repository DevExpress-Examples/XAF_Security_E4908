This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Windows Forms application. You will also learn how to execute Create, Write and Delete data operations taking into account security permissions.

>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.


### Prerequisites
- Build the solution and run the *XafSolution.Win* project to log in under 'User' or 'Admin' with an empty password. The application will generate a database with business objects from the *XafSolution.Module* project. 
- Add the *XafSolution.Module* assembly reference to your application.

> **!NOTE:** If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

***

# Step 1: Initialize Data Store and XAF's Security System

- In the `Main` method of Program.cs, initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system and register the business objects that you will access from your code.
	
	[](#tab/tabid-csharp)
	
``` csharp
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
//...
XpoTypesInfoHelper.GetXpoTypeInfoSource();
XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
```
- Initialize the Security System.
	
	[](#tab/tabid-csharp)
	
``` csharp
AuthenticationStandard authentication = new AuthenticationStandard();
SecurityStrategyComplex security = new SecurityStrategyComplex(
    typeof(PermissionPolicyUser), 
    typeof(PermissionPolicyRole), 
    authentication
);
security.RegisterXPOAdapterProviders();
```
- Create a `SecuredObjectSpaceProvider` object. It allows you to create `SecuredObjectSpace` instances to ensure secured data access.
	[](#tab/tabid-csharp)
	
``` csharp
string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(
    security, 
    connectionString,
    null
);
```

- In _App.config_, add the connection string and replace "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.
	
	[](#tab/tabid-xml)
	
``` xml
<add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"/>
```

> Make sure that the static [EnableRfc2898 and SupportLegacySha512 properties](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System) in your non-XAF application have same values as in the XAF application where passwords were set. Otherwise you won't be able to login.

## Step 2: Implement the Main and Login Forms

- In Program.cs, create [MainForm](CS/MainForm.cs) using a custom constructor. `MainForm` is the MDI parent form for [EmployeeListForm](CS/EmployeeListForm.cs) and [EmployeeDetailForm](CS/EmployeeDetailForm.cs).

``` csharp
Application.EnableVisualStyles();
Application.SetCompatibleTextRenderingDefault(false);
MainForm mainForm = new MainForm(security, objectSpaceProvider);
Application.Run(mainForm);
```

- Display the [LoginForm](CS/LoginForm.cs) on the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event. If the dialog returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult), `EmployeeListForm` is created and shown.

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
    EmployeeListForm employeeForm = new EmployeeListForm();
    employeeForm.MdiParent = this;
    employeeForm.WindowState = FormWindowState.Maximized;
    employeeForm.Show();
}
```
- Handle the RibbonControl's Logout item.

``` csharp
private void LogoutButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    foreach(Form form in MdiChildren) {
        form.Close();
    }
    Security.Logoff();
    Hide();
    ShowLoginForm();
}
```
	
- [LoginForm](CS/LoginForm.cs) contains two TextBox controls for username and password, and the Login button that attempts to log the user into the security system and returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult?view=netframework-4.8) if logon was successful.

``` csharp
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

## Step 3: Implement the List Form
- [EmployeeListForm](CS/EmployeeListForm.cs) contains a [DevExpress Grid View](https://docs.devexpress.com/WindowsForms/3464/Controls-and-Libraries/Data-Grid/Views/Grid-View) that displays a list of all Employees. Handle the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event and: 
	- Create a `SecuredObjectSpace` instance to access protected data and use its data manipulation APIs (for instance, *IObjectSpace.GetObjects*) OR if you prefer, the familiar `UnitOfWork` object accessible through the *SecuredObjectSpace.Session* property.
	- Set [XPBindingSource.DataSource](https://docs.devexpress.com/XPO/DevExpress.Xpo.XPBindingSource.DataSource) to the Employees collection obtained from the secured object space.
	- Perform the [IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) request to check Create operation availability and thus determine whether the New button can be enabled.
		
``` csharp
private void EmployeeListForm_Load(object sender, EventArgs e) {
    security = ((MainForm)MdiParent).Security;
    objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
    securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
    // The XPO way:
    // var session = ((SecuredObjectSpace)securedObjectSpace).Session;
    // 
    // The XAF way:
    employeeBindingSource.DataSource = securedObjectSpace.GetObjects<Employee>();
    newBarButtonItem.Enabled = security.IsGranted(
        new PermissionRequest(
            securedObjectSpace, typeof(Employee), SecurityOperations.Create
        )
    );
    protectedContentTextEdit = new RepositoryItemProtectedContentTextEdit();
}
```	
- Handle the [GridView.CustomRowCellEdit](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Grid.GridView.CustomRowCellEdit) event and check Read operation availability.
		
``` csharp
private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
    string fieldName = e.Column.FieldName;
    object targetObject = employeeGridView.GetRow(e.RowHandle);
    if(!security.IsGranted(
        new PermissionRequest(
            securedObjectSpace, typeof(Employee), SecurityOperations.Read, targetObject, fieldName
        )
    )) {
        e.RepositoryItem = protectedContentTextEdit;
    }
}
```
Note that SecuredObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the SecurityStrategy.IsGranted method to determine when to mask default values with the "Protected Content" placeholder in the UI.
		
- Handle the [FocusedRowObjectChanged](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Base.ColumnView.FocusedRowObjectChanged) event and use the IsGranted request to check Delete operation availability and thus determine if the Delete button can be enabled.
		
``` csharp
private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
    deleteBarButtonItem.Enabled = security.IsGranted(
        new PermissionRequest(
            securedObjectSpace, typeof(Employee), SecurityOperations.Delete, e.Row
        )
    );
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
    EmployeeDetailForm detailForm = new EmployeeDetailForm(employee);
    detailForm.MdiParent = MdiParent;
    detailForm.WindowState = FormWindowState.Maximized;
    detailForm.Show();
    detailForm.FormClosing += DetailForm_FormClosing;
}
```

We need to create `EmployeeDetailForm` in three cases: 
    
   - When a user clicks the New button
			
``` csharp
private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
    CreateDetailForm();
}
```

   - When a user clicks the Edit button (passes the current row handle as a parameter to the `CreateDetailForm` method)
   
``` csharp
private void EditEmployee() {
	Employee employee = employeeGridView.GetRow(employeeGridView.FocusedRowHandle) as Employee;
	CreateDetailForm(employee);
}

private void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
	EditEmployee();
}
```
   - When a user double-clicks a row 
		
``` csharp
private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
	if(e.Clicks == 2) {
		EditEmployee();
	}
}
```

## Step 4: Implement the Detail Form

- [EmployeeDetailForm](CS/EmployeeDetailForm.cs) contains detailed information on the Employee object. Perform the following operation in the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event handler: 
		
	- Create a `SecuredObjectSpace` instance to get the current or create new Employee object.
	- Use the [IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) request to check Delete operation availability and thus determine if the Delete button can be enabled. The Delete button is always disabled if you create new object.
	- Set [XPBindingSource.DataSource](https://docs.devexpress.com/XPO/DevExpress.Xpo.XPBindingSource.DataSource) to the Employee object.
		
``` csharp
private void EmployeeDetailForm_Load(object sender, EventArgs e) {
    security = ((MainForm)MdiParent).Security;
    objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
    securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
    if(employee == null) {
        employee = securedObjectSpace.CreateObject<Employee>();
    }
    else {
        employee = securedObjectSpace.GetObject(employee);
        deleteButtonItem.Enabled = security.IsGranted(
            new PermissionRequest(
                securedObjectSpace, typeof(Employee), SecurityOperations.Delete, employee
            )
        );
    }
    employeeBindingSource.DataSource = employee;
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
- The `AddControl` method creates a control for a specific member. Use the IsGranted request to check Read operation availability. If not available, create and disable the `ProtectedContentEdit` control which displays the "Protected Content" placeholder. Otherwise: 
		
	- Call the `GetControl` method to create an appropriate control depending of the member type. We use the [ComboBoxEdit](https://docs.devexpress.com/WindowsForms/614/controls-and-libraries/editors-and-simple-controls/simple-editors/concepts/dropdown-editors/combo-box-editors#comboboxedit-control) control for the Department associated property.
	- Add a binding to the [Control.DataBindings](https://docs.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control.databindings?view=netframework-4.8) collection.
	- Use the IsGranted request to check Write operation availability and thus determine whether the control should be enabled.
		
``` csharp
private void AddControl(LayoutControlItem layout, object targetObject, string memberName, string caption) {
    layout.Text = caption;
    Type type = targetObject.GetType();
    BaseEdit control;
    if(security.IsGranted(
        new PermissionRequest(
            securedObjectSpace, type, SecurityOperations.Read, targetObject, memberName
        )
    )) {
        control = GetControl(type, memberName);
        if(control != null) {
            control.DataBindings.Add(
                new Binding(
                    "EditValue", employeeBindingSource, memberName, true,
                     DataSourceUpdateMode.OnPropertyChanged
                )
            );
            control.Enabled = security.IsGranted(
                new PermissionRequest(
                    securedObjectSpace, type, SecurityOperations.Write, targetObject, memberName
                 )
            );
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
            ((ComboBoxEdit)control).Properties.Items.AddRange(
                securedObjectSpace.GetObjects<Department>() as XPCollection<Department>
            );
        }
        else {
            control = new TextEdit();
        }
    }
    return control;
}
```
		
- Use `SecuredObjectSpace` to commit all changes to database in the [saveBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler and delete the current object in the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler.
		
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

## Step 5: Run and Test the App
 - Log in under 'User' with an empty password.
   
   ![](/images/WinForms_LoginForm.png)

 - Notice that secured data is displayed as 'Protected Content'.
   ![](/images/WinForms_MainForm.png)

 - Press the Logout button and log in under 'Admin' to see all records.
