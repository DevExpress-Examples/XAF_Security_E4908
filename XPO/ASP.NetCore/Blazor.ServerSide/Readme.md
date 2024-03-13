This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Blazor application.
You will also see how to execute Create, Write, and Delete data operations and take security permissions into account.

## Prerequisites

- [Visual Studio 2022 v17.0+](https://visualstudio.microsoft.com/vs/) with the following workloads:
  - **ASP.NET and web development**
  - **.NET Core cross-platform development**
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet-core)
- Download and run the [Unified Component Installer](https://www.devexpress.com/Products/Try/) or add [NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url) to Visual Studio NuGet feeds.
  
  *We recommend that you select all products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*


> **NOTE** 
>
> If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

> If you wish to create a Blazor project with our Blazor Components from scratch, follow the [Create a New Blazor Application](https://docs.devexpress.com/Blazor/401057/getting-started/create-a-new-application) article.

---

## Step 1. Configure the Blazor Application

For detailed information about the ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio).

- Configure the Blazor application in the [Program.cs](Program.cs):

    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    builder.Services.AddRazorPages();
    builder.Services.AddServerSideBlazor();
    builder.Services.AddDevExpressBlazor();
    builder.Services.AddSession();

    var app = builder.Build();
    if (app.Environment.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
    }
    else {
        app.UseExceptionHandler("/Error");
        app.UseHsts();
    }
    app.UseSession();
    app.UseHttpsRedirection();
    app.UseRouting();
    app.UseEndpoints(endpoints => {
        endpoints.MapFallbackToPage("/_Host");
        endpoints.MapBlazorHub();
    });
    app.Run();
    ```
## Step 2. Initialize Data Store and XAF Security System. Authentication and Permission Configuration

- Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.
    ```csharp
    builder.Services.AddSingleton<ITypesInfo>((serviceProvider) => {
        TypesInfo typesInfo = new TypesInfo();
        typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
        typesInfo.RegisterEntity(typeof(Employee));
        typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
        typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
        return typesInfo;
    })
    ```

- Register ObjectSpaceProviders that will be used in your application. To do this, [implement](./Services/ObjectSpaceProviderFactory.cs) the `IObjectSpaceProviderFactory` interface.
    ```csharp
    builder.Services.AddScoped<IObjectSpaceProviderFactory, ObjectSpaceProviderFactory>()
    
    // ...
    
    public class ObjectSpaceProviderFactory : IObjectSpaceProviderFactory {
        readonly ISecurityStrategyBase security;
        readonly IXpoDataStoreProvider xpoDataStoreProvider;
        readonly ITypesInfo typesInfo;

        public ObjectSpaceProviderFactory(ISecurityStrategyBase security, IXpoDataStoreProvider xpoDataStoreProvider, ITypesInfo typesInfo) {
            this.security = security;
            this.typesInfo = typesInfo;
            this.xpoDataStoreProvider = xpoDataStoreProvider;

        }

        IEnumerable<IObjectSpaceProvider> IObjectSpaceProviderFactory.CreateObjectSpaceProviders() {
            yield return new SecuredObjectSpaceProvider((ISelectDataSecurityProvider)security, xpoDataStoreProvider, typesInfo, null, true);
        }
    }
    ```

- Set up database connection settings in your Data Store Provider object. In XPO, it is `IXpoDataStoreProvider`.
    ```csharp
    builder.Services.AddSingleton<IXpoDataStoreProvider>((serviceProvider) => {
        var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString");
        return XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
    });
    ```
        
    The `IConfiguration` object is used to access the application configuration [appsettings.json](appsettings.json) file. In _appsettings.json_, add the connection string.
    ```json
    "ConnectionStrings": {
        "ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    }
    ```

- Register security system and authentication in the [Program.cs](Program.cs). [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication), and ASP.NET Core Identity authentication is registered automatically in [AspNetCore Security setup]().

    ```csharp
    builder.Services.AddXafAspNetCoreSecurity(builder.Configuration, options => {
        options.RoleType = typeof(PermissionPolicyRole);
        options.UserType = typeof(PermissionPolicyUser);
        options.Events.OnSecurityStrategyCreated = strategy => ((SecurityStrategy)strategy).RegisterXPOAdapterProviders();
    }).AddAuthenticationStandard();
    ```

- Call the `UseDemoData` method at the end of the [Program.cs](Program.cs) to update the database:
    
    ```csharp
    public static WebApplication UseDemoData(this WebApplication app) {
        using var scope = app.Services.CreateScope();
        var updatingObjectSpaceFactory = scope.ServiceProvider.GetRequiredService<IUpdatingObjectSpaceFactory>();
        using var objectSpace = updatingObjectSpaceFactory
            .CreateUpdatingObjectSpace(typeof(BusinessObjectsLibrary.BusinessObjects.Employee), true));
        new Updater(objectSpace).UpdateDatabase();
        return app;
    }
    ```
    For more details about how to create demo data from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.

## Step 3. Create an edit model

 [EditableEmployee](Models/EditableEmployee.cs) is an edit model class for the Employee business object.
 ```csharp
 public class EditableEmployee {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Department Department { get; set; }
}
 ```

Add additional extension methods to easily convert the Employee object to the EditableEmployee edit model and vice versa.

  ```csharp
public static class EmployeeExtensions {
    public static EditableEmployee ToModel(this Employee employee) {
        return new EditableEmployee {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Department = employee.Department
        };
    }
    public static void FromModel(this EditableEmployee editableEmployee, Employee employee) {
        employee.FirstName = editableEmployee.FirstName;
        employee.LastName = editableEmployee.LastName;
        employee.Email = editableEmployee.Email;
        employee.Department = editableEmployee.Department;
    }
}
 ```

## Step 4. Pages

[Login.cshtml](Pages/Login.cshtml) is a login page that allows you to log into the application.

[Login.cshtml.cs](Pages/Login.cshtml.cs) class uses `IStandardAuthenticationService` from XAF Security System to implement the Login logic. It authenticates user with the AuthenticationStandard authentication and return a ClaimsPrincipal object with all the necessary XAF Security data. That principal is then authenticated to the ASP.NET Core Identity authentication.

```csharp
readonly IStandardAuthenticationService authenticationStandard;

// ...

public IActionResult OnPost() {
    Response.Cookies.Append("userName", Input.UserName ?? string.Empty);
    if(ModelState.IsValid) {
        ClaimsPrincipal principal = authenticationStandard.Authenticate(new AuthenticationStandardLogonParameters(Input.UserName, Input.Password));
        if(principal != null) {
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return Redirect("/");
        }
        ModelState.AddModelError("Error", "User name or password is incorrect");
    }
    return Page();
}
```

[Logout.cshtml.cs](Pages/Logout.cshtml.cs) class implements the Logout logic.

```csharp
public IActionResult OnGet() {
    this.HttpContext.SignOutAsync();
    return Redirect("/Login");
}
```

[Index.razor](Pages/Index.razor) is the main page. It configures the [Blazor Grid](https://docs.devexpress.com/Blazor/403143/components/grid) and allows a user to log out.

The `OnInitialized` method creates `security` and `objectSpace` instances and gets `Employee` and `Department` objects.

```csharp
protected override void OnInitialized() {
    security = (SecurityStrategy)securityProvider.GetSecurity();
    objectSpace = objectSpaceFactory.CreateObjectSpace<Employee>();
    employees = objectSpace.GetObjectsQuery<Employee>();
    departments = objectSpace.GetObjectsQuery<Department>();
    base.OnInitialized();
}
```

The `Grid_CustomizeEditModel` method creates the EditableEmployee edit model.

```csharp
void Grid_CustomizeEditModel(GridCustomizeEditModelEventArgs e) {
    e.EditModel = e.IsNew ? new EditableEmployee() : ((Employee)e.DataItem).ToModel();
    editableEmployee = (Employee)e.DataItem;
}
```

The `Grid_EditModelSaving` method transfers the edit model changes to the business object.

```csharp
void Grid_EditModelSaving(GridEditModelSavingEventArgs e) {
    Employee employee = e.IsNew ? objectSpace.CreateObject<Employee>() : (Employee)e.DataItem;
    ((EditableEmployee)e.EditModel).FromModel(employee);
    UpdateData();
}
```

The `Grid_DataItemDeleting` method removes an object.

```csharp
void Grid_DataItemDeleting(GridDataItemDeletingEventArgs e) {
    objectSpace.Delete(e.DataItem);
    UpdateData();
}
```

The `UpdateData` method commits changes and refreshes the grid data.

```csharp
void UpdateData() {
    objectSpace.CommitChanges();
    employees = objectSpace.GetObjectsQuery<Employee>();
    editableEmployee = null;
}
```

To show/hide the `New`, `Edit`, and `Delete` actions, use the appropriate `CanCreate`, `CanEdit`, and `CanDelete` methods of the Security System.

```razor
<DxGridCommandColumn Width="160px" NewButtonVisible=@(security.CanCreate<Employee>())>
    <CellDisplayTemplate>
        @if(security.CanWrite(context.DataItem)) {
            <DxButton Text="Edit" Click="@(() => context.Grid.StartEditRowAsync(context.VisibleIndex))" RenderStyle="ButtonRenderStyle.Link" />
        }
        @if(security.CanDelete(context.DataItem)) {
            <DxButton Text="Delete" Click="@(() => context.Grid.ShowRowDeleteConfirmation(context.VisibleIndex))" RenderStyle="ButtonRenderStyle.Link" />
        }
    </CellDisplayTemplate>
</DxGridCommandColumn>
```

The page is decorated with the Authorize attribute to prohibit unauthorized access.

```razor
@attribute [Authorize]
```

To show the `*******` text instead of a default value in grid cells and editors, use [SecuredDisplayCellTemplate](Components/SecuredDisplayCellTemplate.razor) and [SecuredEditCellTemplate](Components/SecuredEditCellTemplate.razor).

```razor
<DxGridDataColumn FieldName="@nameof(Employee.FirstName)">
    <CellDisplayTemplate>
        <SecuredDisplayCellTemplate CurrentObject="@context.DataItem" PropertyName="@nameof(Employee.FirstName)">
            @(((Employee)context.DataItem).FirstName)
        </SecuredDisplayCellTemplate>
    </CellDisplayTemplate>
    <CellEditTemplate>
        <SecuredEditCellTemplate Context=readOnly CurrentObject=editableEmployee PropertyName=@nameof(Employee.FirstName)>
            <DxTextBox @bind-Text=((EditableEmployee)context.EditModel).FirstName ReadOnly=@readOnly />
        </SecuredEditCellTemplate>
    </CellEditTemplate>
</DxGridDataColumn>
```

To show the `*******` text instead of the default text, check the Read permission by using the `CanRead` method of the Security System.
Use the `CanWrite` method of the Security System to check if a user is allowed to edit a property and an editor should be created for this property.

[CellEditTemplateBase](Components/CellEditTemplateBase.cs):
```csharp
protected bool CanWrite => CurrentObject is null ? Security.CanWrite(typeof(T), PropertyName) : Security.CanWrite(CurrentObject, PropertyName);
protected bool CanRead => CurrentObject is null ? Security.CanRead(typeof(T), PropertyName) : Security.CanRead(CurrentObject, PropertyName);
```

## Step 4: Run and Test the App

- Log in a 'User' with an empty password.
  ![](/images/Blazor_LoginPage.png)

- Note that secured data is displayed as '*******'.
  ![](/images/Blazor_ListView.png)

- Press the **Logout** button and log in as 'Admin' to see all the records.
