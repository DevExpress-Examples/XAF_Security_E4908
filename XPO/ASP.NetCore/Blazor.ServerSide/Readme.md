> # Under construction for 22.1 release

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

- Enable the authentication service and configure the request pipeline with the authentication middleware in the [Program.cs](Program.cs). 
[UnauthorizedRedirectMiddleware](UnauthorizedRedirectMiddleware.cs) сhecks if the ASP.NET Core Identity is authenticated. If not, it redirects a user to the authentication page.

    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    //...
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie();
    builder.Services.AddAuthorization();

    var app = builder.Build();
    //...
    app.UseAuthentication();
    app.UseAuthorization();
    app.UseMiddleware<UnauthorizedRedirectMiddleware>();
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseCookiePolicy();

    //...
    public class UnauthorizedRedirectMiddleware {
        private const string authenticationPagePath = "/Authentication.html";
        private readonly RequestDelegate _next;
        public UnauthorizedRedirectMiddleware(RequestDelegate next) {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context) {
            if(context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated
                || IsAllowAnonymous(context)) {
                await _next(context);
            } else {
                context.Response.Redirect(authenticationPagePath);
            }
        }
        private static bool IsAllowAnonymous(HttpContext context) {
            string referer = context.Request.Headers["Referer"];
            return context.Request.Path.HasValue && context.Request.Path.StartsWithSegments(authenticationPagePath)
                || referer != null && referer.Contains(authenticationPagePath);
        }
    }
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

## Step 3. Pages

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

[Logout.cshtml.cs](Pages/Logout.cshtml.cs) class implements the Logout logic

```csharp
public IActionResult OnGet() {
    this.HttpContext.SignOutAsync();
    return Redirect("/Login");
}
```

[Index.razor](Pages/Index.razor) is the main page. It configures the [Blazor Data Grid](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxDataGrid-1) and allows a user to log out.

The `OnInitialized` method creates an ObjectSpace instance and gets `Employee` and `Department` objects.

```csharp
protected override void OnInitialized() {
    objectSpace = objectSpaceFactory.CreateObjectSpace<Employee>();
    employees = objectSpace.GetObjectsQuery<Employee>();
    departments = objectSpace.GetObjectsQuery<Department>();
    base.OnInitialized();
}
```

The `HandleValidSubmit` method saves changes if data is valid.

```csharp
async Task HandleValidSubmit() {
    objectSpace.CommitChanges();
    await grid.Refresh();
    employee = null;
    await grid.CancelRowEdit();
}
```

The `OnRowRemoving` method removes an object.

```csharp
Task OnRowRemoving(object item) {
    objectSpace.Delete(item);
    objectSpace.CommitChanges();
    return grid.Refresh();
}
```

To show/hide the `New`, `Edit`, `Delete` actions, use the appropriate `CanCreate`, `CanEdit` and `CanDelete` methods of the Security System.

```razor
<DxDataGridCommandColumn Width="100px">
    <HeaderCellTemplate>
        @if(Security.CanCreate<Employee>()) {
            <button class="btn btn-link" @onclick="@(() => StartRowEdit(null))">New</button>
        }
    </HeaderCellTemplate>
    <CellTemplate>
        @if(Security.CanWrite(context)) {
            <a @onclick="@(() => StartRowEdit(context))" href="javascript:;">Edit </a>
        }
        @if(Security.CanDelete(context)) {
            <a @onclick="@(() => OnRowRemoving(context))" href="javascript:;">Delete</a>
        }
    </CellTemplate>
</DxDataGridCommandColumn>
```

The page is decorated with the Authorize attribute to prohibit unauthorized access.

```razor
@attribute [Authorize]
```

To show the `*******` text instead of a default value in data grid cells and editors, use [SecuredContainer](Components/SecuredContainer.razor)

```razor
<DxDataGridColumn Field="@nameof(Employee.FirstName)">
    <DisplayTemplate>
        <SecuredContainer Context="readOnly" CurrentObject="@context" PropertyName="@nameof(Employee.FirstName)">
            @(((Employee)context).FirstName)
        </SecuredContainer>
    </DisplayTemplate>
</DxDataGridColumn>
//...
<DxFormLayoutItem Caption="First Name">
    <Template>
        <SecuredContainer Context="readOnly" CurrentObject=@employee PropertyName=@nameof(Employee.FirstName) IsEditor=true>
            <DxTextBox @bind-Text=employee.FirstName ReadOnly=@readOnly />
        </SecuredContainer>
    </Template>
</DxFormLayoutItem>
```

To show the `*******` text instead of the default text, check the Read permission by using the `CanRead` method of the Security System.
Use the `CanWrite` method of the Security System to check if a user is allowed to edit a property and an editor should be created for this property.

```razor
private bool HasAccess => objectSpace.IsNewObject(CurrentObject) ?
    SecurityProvider.Security.CanWrite(CurrentObject.GetType(), PropertyName) :
    SecurityProvider.Security.CanRead(CurrentObject, PropertyName);
```

## Step 4: Run and Test the App

- Log in a 'User' with an empty password.
  ![](/images/Blazor_LoginPage.png)

- Note that secured data is displayed as '*******'.
  ![](/images/Blazor_ListView.png)

- Press the **Logout** button and log in as 'Admin' to see all the records.
