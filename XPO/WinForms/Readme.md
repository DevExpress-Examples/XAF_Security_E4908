This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Windows Forms App (.NET Core) using XPO ORM for data access. You will also learn how to execute Create, Write, and Delete data operations taking into account security permissions.

>If you are using EF Core for data access, [follow this tutorial](https://github.com/DevExpress-Examples/XAF_Security_E4908/tree/master/EFCore/WinForms).

### Prerequisites

- [Visual Studio 2022 v17.0+](https://visualstudio.microsoft.com/vs/)
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Download and run our Unified Component Installer](https://www.devexpress.com/Products/Try/) or add [NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url) to Visual Studio NuGet feeds.
  
  *We recommend that you select all products when you run the DevExpress installer. The installer registers local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*


> **NOTE** 
>
> If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

## Step 1: Initialize Data Store and XAF's Security System

1. Create a new .NET 6 WinForms application or use an existing application.
2. Add DevExpress NuGet packages to your project:

    ```xml
    <PackageReference Include="DevExpress.Win.Grid" Version="21.2.4" />
    <PackageReference Include="DevExpress.Persistent.BaseImpl.Xpo" Version="21.2.4" />
    <PackageReference Include="DevExpress.ExpressApp.Security.Xpo" Version="21.2.4" />
    ```
3. Open the application configuration file (_App.config_). Add the following line to this file.
    
    ```xml
    <add name="ConnectionString" connectionString="Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True"/>
    ```
4. Create an instance of `TypesInfo` required for the correct operation of the Security System.
    ```csharp
    TypesInfo typesInfo = new TypesInfo();
    ```
5. Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.

    ```csharp
    typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
    typesInfo.RegisterEntity(typeof(Employee));
    typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
    typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
    ```
6. Call the `CreateDemoData` method at the beginning of the `Main` method of _Program.cs_:
    ```csharp
    private static void CreateDemoData(TypesInfo typesInfo, IXpoDataStoreProvider dataStoreProvider) {
        using (var objectSpaceProvider = new XPObjectSpaceProvider(dataStoreProvider, typesInfo, null)) {
            using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                new Updater(objectSpace).UpdateDatabase();
            }
        }
    }
    ```
    For more details about how to create demo data from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.
        
7. Initialize the Security System.

    ```csharp
    namespace WindowsFormsApplication {
        static class Program {
            [STAThread]
            static void Main() {
                // ...
                AuthenticationStandard authentication = new AuthenticationStandard();
                SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
                security.RegisterXPOAdapterProviders();
                // ...
            }
        }
    }
    ```

8. Create a **SecuredObjectSpaceProvider** object. It allows you to create a **SecuredObjectSpace** to ensure a secured data access.
    
    ```csharp
    SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, dataStoreProvider, typesInfo, null);
    ```

## Step 2: Implement the Main and Login Forms

1. In Program.cs, create [MainForm](MainForm.cs) using a custom constructor. `MainForm` is the MDI parent form for [EmployeeListForm](EmployeeListForm.cs) and [EmployeeDetailForm](EmployeeDetailForm.cs).

    ```csharp
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    MainForm mainForm = new MainForm(security, objectSpaceProvider);
    Application.Run(mainForm);
    ```

2. Display the [LoginForm](LoginForm.cs) on the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event. If the dialog returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult), `EmployeeListForm` is created and shown.

    ```csharp
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
3. Handle the RibbonControl's Logout item.

    ```csharp
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
    
4. [LoginForm](LoginForm.cs) contains two TextBox controls for username and password, and the Login button that attempts to log the user into the security system and returns [DialogResult.OK](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.dialogresult?view=netframework-4.8) if logon was successful.

    ```csharp
    private void Login_Click(object sender, EventArgs e) {
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
            XtraMessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
    ```

## Step 3: Implement the List Form

1. [EmployeeListForm](EmployeeListForm.cs) contains a [DevExpress Grid View](https://docs.devexpress.com/WindowsForms/3464/Controls-and-Libraries/Data-Grid/Views/Grid-View) that displays a list of all Employees. Handle the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event and: 
    - Create a `EFCoreObjectSpace` instance to access protected data and use its data manipulation APIs (for instance, *IObjectSpace.GetObjects*) OR if you prefer, the familiar `UnitOfWork` object accessible through the *SecuredObjectSpace.Session* property.
    - Set [XPBindingSource.DataSource](https://docs.devexpress.com/XPO/DevExpress.Xpo.XPBindingSource.DataSource) to the Employees collection obtained from the secured object space.
    - Call the CanCreate method to check Create operation availability and thus determine whether the New button can be enabled.

    ```csharp
    private void EmployeeListForm_Load(object sender, EventArgs e) {
        security = ((MainForm)MdiParent).Security;
        objectSpaceProvider = ((MainForm)MdiParent).ObjectSpaceProvider;
        securedObjectSpace = objectSpaceProvider.CreateObjectSpace();
        // The XPO way:
        // var session = ((SecuredObjectSpace)securedObjectSpace).Session;
        // 
        // The XAF way:
        employeeBindingSource.DataSource = securedObjectSpace.GetObjects<Employee>();
        newBarButtonItem.Enabled = security.CanCreate<Employee>();
        protectedContentTextEdit = new RepositoryItemProtectedContentTextEdit();
    }
    ```

2. Handle the [GridView.CustomRowCellEdit](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Grid.GridView.CustomRowCellEdit) event and check Read operation availability.
        
    ```csharp
    private void GridView_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e) {
        string fieldName = e.Column.FieldName;
        object targetObject = employeeGridView.GetRow(e.RowHandle);
        if (!security.CanRead(targetObject, fieldName)) {
            e.RepositoryItem = protectedContentTextEdit;
        }
    }
    ```

    Note that SecuredObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the SecurityStrategy.CanRead method to determine when to mask default values with the "*******" placeholder in the UI.
        
3. Handle the [FocusedRowObjectChanged](https://docs.devexpress.com/WindowsForms/DevExpress.XtraGrid.Views.Base.ColumnView.FocusedRowObjectChanged) event and use the SecurityStrategy.CanDelete method to check Delete operation availability and thus determine if the Delete button can be enabled.
        
    ```csharp
    private void EmployeeGridView_FocusedRowObjectChanged(object sender, FocusedRowObjectChangedEventArgs e) {
        deleteBarButtonItem.Enabled = security.CanDelete(e.Row);
    }
    ```
4. Delete the current object in the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler.
    
    ```csharp
    private void DeleteBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
        object cellObject = employeeGridView.GetRow(employeeGridView.FocusedRowHandle);
        securedObjectSpace.Delete(cellObject);
        securedObjectSpace.CommitChanges();
    }
    ```
        
5. Create and show `EmployeeDetailForm`.
        
    ```csharp
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
                
        ```csharp
        private void NewBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            CreateDetailForm();
        }
        ```

    - When a user clicks the Edit button (passes the current row handle as a parameter to the `CreateDetailForm` method)
    
        ```csharp
        private void EditEmployee() {
            Employee employee = employeeGridView.GetRow(employeeGridView.FocusedRowHandle) as Employee;
            CreateDetailForm(employee);
        }

        private void EditBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) {
            EditEmployee();
        }
        ```
    - When a user double-clicks a row 
                
        ```csharp
        private void EmployeeGridView_RowClick(object sender, RowClickEventArgs e) {
            if(e.Clicks == 2) {
                EditEmployee();
            }
        }
        ```

## Step 4: Implement the Detail Form

1. [EmployeeDetailForm](EmployeeDetailForm.cs) contains detailed information on the Employee object. Perform the following operation in the [Form.Load](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load) event handler: 

    - Create a `SecuredObjectSpace` instance to get the current or create new Employee object.
    - Use the SecurityStrategy.CanDelete method to check Delete operation availability and thus determine if the Delete button can be enabled. The Delete button is always disabled if you create new object.
    - Set [XPBindingSource.DataSource](https://docs.devexpress.com/XPO/DevExpress.Xpo.XPBindingSource.DataSource) to the Employee object.
        
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
            deleteBarButtonItem.Enabled = security.CanDelete(employee);
        }
        employeeBindingSource.DataSource = employee;
        AddControls();
    }
    ```    
2. The `AddControls` method creates controls for all members declared in the `visibleMembers` dictionary.
        
    ```csharp
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
3. The `AddControl` method creates a control for a specific member. Use the SecurityStrategy.CanRead method to check Read operation availability. If not available, create and disable the `ProtectedContentEdit` control which displays the "*******" placeholder. Otherwise: 
        
    - Call the `GetControl` method to create an appropriate control depending of the member type. We use the [ComboBoxEdit](https://docs.devexpress.com/WindowsForms/DevExpress.XtraEditors.ComboBoxEdit) control for the Department associated property.
    - Add a binding to the [Control.DataBindings](https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.databindings?view=netframework-4.8) collection.
    - Use the SecurityStrategy.CanWrite method to check Write operation availability and thus determine whether the control should be enabled.
        
    ```csharp
    private void AddControl(LayoutControlItem layout, object targetObject, string memberName, string caption) {
        layout.Text = caption;
        Type type = targetObject.GetType();
        BaseEdit control;
        if(security.CanRead(targetObject, memberName)) {
            control = GetControl(type, memberName);
            if(control != null) {
                control.DataBindings.Add(
                    new Binding(
                        "EditValue", employeeBindingSource, memberName, true,
                        DataSourceUpdateMode.OnPropertyChanged
                    )
                );
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
        
4. Use `SecuredObjectSpace` to commit all changes to database in the [saveBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler and delete the current object in the [deleteBarButtonItem.ItemClick](https://docs.devexpress.com/WindowsForms/DevExpress.XtraBars.BarItem.ItemClick) event handler.
        
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

## Step 5: Run and Test the App

1. Log in a **User** with an empty password.
   
   ![](/images/WinForms_LoginForm.png)

2. Notice that secured data is displayed as "*******".
   ![](/images/WinForms_MainForm.png)

3. Press the **Log Out** button and log in as **Admin** to see all the records.