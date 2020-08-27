This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Web Forms application. 
You will also learn how to execute Create, Write and Delete data operations taking into account security permissions.

>For simplicity, the instructions include only C# code snippets. For the complete C# and VB code, see the [CS](CS) and [VB](VB) sub-directories.


### Prerequisites
- Build the solution and run the *XafSolution.Win* project to log in under 'User' or 'Admin' with an empty password. The application will generate a database with business objects from the *XafSolution.Module* project. 
- Add the *XafSolution.Module* assembly reference to your application.

> **!NOTE:** If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

***

## Step 1. Database Connection and Security System Initialization
- Implement [XpoDataStoreProviderService](CS/XpoDataStoreProviderService.cs) to create Data Store Provider and access its value in singleton manner.
  
  ```csharp
  public static class XpoDataStoreProviderService {
      private static IXpoDataStoreProvider dataStoreProvider;
      public static IXpoDataStoreProvider GetDataStoreProvider(IDbConnection connection, bool enablePoolingInConnectionString) {
          if(dataStoreProvider == null) {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, enablePoolingInConnectionString);
          }
          return dataStoreProvider;
      }
  }
  ```
  
  In the [Web.config](CS/Web.config) file, add the connection string and replace "DBSERVER" with the Database Server name or its IP address. Use "localhost" or "(local)" if you use a local Database Server.
  
  ```xml
  <add name="ConnectionString" connectionString="Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"/>
  ```
  
- Implement the [ConnectionHelper](CS/ConnectionHelper.cs) class.
  
  Implement the `GetObjectSpaceProvider` method to create a new `SecuredObjectSpaceProvider` instance.

  ```csharp
  public static SecuredObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
      SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, XpoDataStoreProviderService.GetDataStoreProvider(null, true), true);
      RegisterEntities(objectSpaceProvider);
      return objectSpaceProvider;
  }
  ```
  
  The `RegisterEntities` method needs to initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) 
  system and register the business objects, which you will access from your code.
  ```csharp
  private static void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
      objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
      objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
      objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
  }
  ```
  
  Implement the `GetSecurity` method to create and initialize the Security System.
  ```csharp
  public static SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
      AuthenticationMixed authentication = new AuthenticationMixed();
      authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
      authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
      authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
      authentication.SetupAuthenticationProvider(authenticationName, parameter);
      SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
      security.RegisterXPOAdapterProviders();
      return security;
  }
  ```

## Step 2. Login Page Implementation
Create the [Login.aspx](CS/Login.aspx) page, add [ASPxTextBox](https://docs.devexpress.com/AspNet/11586/aspnet-webforms-controls/data-editors/aspxtextbox) 
to enter the login/password. Then, add 'Log In' [ASPxButton](https://documentation.devexpress.com/AspNet/11620/ASP-NET-WebForms-Controls/Data-Editors/ASPxButton) to log in.
  
```csharp
protected void LoginButton_Click(object sender, EventArgs e) {
    string userName = UserNameBox.Text;
    string password = PasswordBox.Text;
    AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
    SecurityStrategyComplex security = ConnectionHelper.GetSecurity(typeof(AuthenticationStandardProvider).Name, parameters);
    SecuredObjectSpaceProvider objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(security);
    IObjectSpace logonObjectSpace = objectSpaceProvider.CreateObjectSpace();
    try {
        security.Logon(logonObjectSpace);
    }
    catch {	}
    if(security.IsAuthenticated) {
        SetCookie(userName);
        FormsAuthentication.RedirectFromLoginPage(userName, true);
    }
    else {
        ClientScript.RegisterStartupScript(GetType(), null, "errorMessage();", true);
    }
    security.Dispose();
    objectSpaceProvider.Dispose();
}
```
  
The `LoginButton_Click` method initializes the Security System and tries to log in the user by the specified user name and password. 
If the user was successfully logged in, the Security System creates a cookie with the specified user name.
  
If the Security System can not find the user by specified credentials, it throws an exception. 
In this example, we handle this exception and display an error message with the client script.

```javascript
function errorMessage() {
    alert("User name or password is incorrect");
}
```
  
## Step 3. Default Page Implementation
  
Create the [Default.aspx](CS/Default.aspx) page and add Log Off [ASPxButton](https://documentation.devexpress.com/AspNet/11620/ASP-NET-WebForms-Controls/Data-Editors/ASPxButton) 
to log out and return to the login page.

```csharp
protected void LogoutButton_Click(object sender, EventArgs e) {
    FormsAuthentication.SignOut();
    FormsAuthentication.RedirectToLoginPage();
}
```

Add [ASPxGridView](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView) to display data in the grid format.

- Initialize the Security System in the `Page_Init` event handler. Now you can create `SecuredObjectSpace` and use [its data manipulation APIs](https://docs.devexpress.com/eXpressAppFramework/113711/concepts/data-manipulation-and-business-logic/create-read-update-and-delete-data) (for instance, *IObjectSpace.GetObjects*) OR if you prefer, the familiar `UnitOfWork` object accessible through the *SecuredObjectSpace.Session* property.
  
  ```csharp
  protected void Page_Init(object sender, EventArgs e) {
      security = ConnectionHelper.GetSecurity(typeof(IdentityAuthenticationProvider).Name, HttpContext.Current.User.Identity);
      objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(security);
      IObjectSpace logonObjectSpace = objectSpaceProvider.CreateObjectSpace();
      security.Logon(logonObjectSpace);
      objectSpace = objectSpaceProvider.CreateObjectSpace();
      // The XAF way:
      // var employees = securedObjectSpace.GetObjects<Employee>();
      //
      // The XPO way:
      EmployeeDataSource.Session = ((XPObjectSpace)objectSpace).Session;
      DepartmentDataSource.Session = ((XPObjectSpace)objectSpace).Session;
      // ...
  }
  ```  
  Also, you need to assign the Session to ASPxGridView Data Sources.
  
  > !NOTE: We temporarily do not use `EmployeeDataSource` in [Server Mode](https://docs.devexpress.com/AspNet/14781/aspnet-webforms-controls/grid-view/concepts/bind-to-data/binding-to-large-data-database-server-mode) because of unexpected behavior when grouping ASPxGridView by associated properties, like Department. It is a known issue described in the [Security \- SecuredSessionObjectLayer allows accessing the key and the ServiceField properties of associated objects](https://supportcenter.devexpress.com/internal/ticket/details/t818790) bug report.

- Check the read operation for appropriate members before each ASPxGridView cell is displayed

  The Data Source contains only the data that is filtered based on security permissions, but we still have to handle protected data in UI. 
  In the [ASPxGridView\.HtmlDataCellPrepared](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.HtmlDataCellPrepared) 
  event handler, display the '*******' placeholder instead of a default value returned by secured Data Source.  
  ```csharp
  protected void EmployeeGrid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e) {
      Employee employee = objectSpace.GetObjectByKey<Employee>(e.KeyValue);
      string memberName = GetMemberName(e.DataColumn);
      if(!security.CanRead(employee, memberName)) {
          e.Cell.Text = "*******";
      }
  }
  private string GetMemberName(GridViewDataColumn column) {
      return column?.FieldName.Split('!')[0];
  }
  ```
  
- Check Create and Delete operations to create/ not create buttons and delete them

  Hide appropriate buttons in the [ASPxGridView\.CommandButtonInitialize](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.CommandButtonInitialize) event handler.
  ```csharp
  protected void EmployeeGrid_CommandButtonInitialize(object sender, ASPxGridViewCommandButtonEventArgs e) {
      if(e.ButtonType == ColumnCommandButtonType.New) {
          if(!security.CanCreate<Employee>()) {
              e.Text = string.Empty;
          }
      }
      if(e.ButtonType == ColumnCommandButtonType.Delete) {
          Employee employee = ((ASPxGridView)sender).GetRow(e.VisibleIndex) as Employee;
          e.Visible = security.CanDelete(employee);
      }
  }
  ```
  
- Check Read and Write operations for editors in [Popup Edit Form](https://demos.devexpress.com/aspxgridviewdemos/gridediting/PopupEditForm.aspx)

  Handle the [ASPxGridView\.CellEditorInitialize](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.CellEditorInitialize) 
  event to display the '*******' placeholder and disable editors based on security permissions.
  
  ```csharp
  protected void EmployeeGrid_CellEditorInitialize(object sender, ASPxGridViewEditorEventArgs e) {
      Employee employee = objectSpace.GetObjectByKey<Employee>(e.KeyValue);
      string memberName = GetMemberName(e.Column);
      if(!security.CanRead(employee, memberName)) {
          e.Editor.Value = "*******";
          e.Editor.Enabled = false;
      }
      else if(!security.CanWrite(employee, memberName)) {
          e.Editor.Enabled = false;
      }
  }
  ```

- Commit changes to database on inserting, updating, or deleting a row

  Handle [ASPxGridView\.RowInserted](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.RowInserted), 
  [ASPxGridView\.RowUpdated](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.RowUpdated), and 
  [ASPxGridView\.RowDeleted](https://docs.devexpress.com/AspNet/DevExpress.Web.ASPxGridView.RowDeleted) events.
  ```csharp
  protected void EmployeeGrid_RowUpdated(object sender, DevExpress.Web.Data.ASPxDataUpdatedEventArgs e) {
      objectSpace.CommitChanges();
  }
  protected void EmployeeGrid_RowDeleted(object sender, DevExpress.Web.Data.ASPxDataDeletedEventArgs e) {
      objectSpace.CommitChanges();
  }
  protected void EmployeeGrid_RowInserted(object sender, DevExpress.Web.Data.ASPxDataInsertedEventArgs e) {
      objectSpace.CommitChanges();
  }
  ```

- Dispose of the objects, which were created, when the page is unloaded

  ```csharp
  protected void Page_Unload(object sender, EventArgs e) {
      objectSpace.Dispose();
      security.Dispose();
      objectSpaceProvider.Dispose();
  }
  ```
  
  ## Step 4: Run and Test the App
 - Log in under 'User' with an empty password.
   
   ![](/images/WebForms_LoginPage.png)

 - Notice that secured data is displayed as '*******'.
   ![](/images/WebForms_ListView.png)

 - Press the Logout button and log in under 'Admin' to see all records.
