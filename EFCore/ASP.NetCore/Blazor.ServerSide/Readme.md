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

1. Add DevExpress NuGet packages to your project:

    ```xml
    <PackageReference Include="DevExpress.Blazor" Version="21.2.4" />
    <PackageReference Include="DevExpress.ExpressApp.EFCore" Version="21.2.4" />
    <PackageReference Include="DevExpress.Persistent.BaseImpl.EFCore" Version="21.2.4" />
    ```
2. Install Entity Framework Core, as described in the [Installing Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/get-started/overview/install) article.

3. For detailed information about the ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio).

[Configure](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-6.0&tabs=windows) the Blazor Application in the [Program.cs](Program.cs):

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
    string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    options.UseSqlServer(connectionString);
    options.UseLazyLoadingProxies();
}, ServiceLifetime.Scoped);

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
app.UseStaticFiles();
app.UseAuthentication();
app.UseDefaultFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});
app.UseDemoData<ApplicationDbContext>(app.Configuration.GetConnectionString("ConnectionString"),
    (builder, connectionString) =>
    builder.UseSqlServer(connectionString));
app.Run();
```

- Register [DbContextFactory](https://docs.microsoft.com/ru-ru/dotnet/api/microsoft.extensions.dependencyinjection.entityframeworkservicecollectionextensions.adddbcontextfactory?view=efcore-5.0) in the [Program.cs](Program.cs)  to access [DbContext](https://docs.microsoft.com/ru-ru/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-5.0) from code.

- The `IConfiguration` object is used to access the application configuration [appsettings.json](appsettings.json) file. In _appsettings.json_, add the connection string.
    ``` json
    "ConnectionStrings": {
        "ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFCoreTestDB;Integrated Security=True;MultipleActiveResultSets=True"
    }
    ```

    > **!NOTE:** The Security System requires [Multiple Active Result Sets](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/enabling-multiple-active-result-sets) in EF Core-based applications connected to the MS SQL database. We do not recommend that you remove “MultipleActiveResultSets=True;“ from the connection string or set the MultipleActiveResultSets parameter to false.
    
- Register HttpContextAccessor in the [Program.cs](Program.cs) to access [HttpContext](https://docs.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8) in controller constructors.

	```csharp
	builder.Services.AddHttpContextAccessor();
	```

- In the [Program.cs](Program.cs) file, register the `TypesInfo` service required for the correct operation of the Security System.

	```csharp
	builder.Services.AddSingleton<ITypesInfo, TypesInfo>();
	``` 

- Call the UseDemoData method at the end of the [Program.cs](Program.cs):
    
    
    ```csharp
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData<TContext>(this WebApplication app, string connectionString, EFCoreDatabaseProviderHandler databaseProviderHandler) where TContext : DbContext {
            ITypesInfo typesInfo = app.Services.GetRequiredService<ITypesInfo>();
            using (var objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(TContext), typesInfo, connectionString, databaseProviderHandler))
            using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                new Updater(objectSpace).UpdateDatabase();
            }
            return app;
        }
    }
    ```
    For more details about how to create demo data from code, see the [Updater.cs](/EFCore/DatabaseUpdater/Updater.cs) class.

## Step 2. Initialize Data Store and XAF Security System. Authentication and Permission Configuration

Register security system and authentication in [Program.cs](Program.cs). We register it as a scoped to have access to SecurityStrategyComplex from SecurityProvider. The `AuthenticationMixed` class allows you to register several authentication providers, 
so you can use both [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and ASP.NET Core Identity authentication.

```csharp
builder.Services.AddScoped((serviceProvider) => {
    AuthenticationMixed authentication = new AuthenticationMixed();
    authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
    authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
    authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
    ITypesInfo typesInfo = serviceProvider.GetRequiredService<ITypesInfo>();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
    return security;
});  
```


Add security extension to `DbContextFactory`to allow your application to filter data based on user permissions. The `DbContextFactory` registers in [Program.cs](Program.cs):

```csharp
builder.Services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
    //...
    ITypesInfo typesInfo = serviceProvider.GetRequiredService<ITypesInfo>();
    options.UseSecurity(serviceProvider.GetRequiredService<SecurityStrategyComplex>(), typesInfo);
```

The [SecurityProvider](Helpers/SecurityProvider.cs) class contains helper functions that provide access to XAF Security System functionality.

```csharp
public class SecurityProvider : IDisposable {
    public SecurityStrategyComplex Security { get; private set; }
    public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
    IHttpContextAccessor contextAccessor;
    IDbContextFactory<ApplicationDbContext> xafDbContextFactory;
    public SecurityProvider(SecurityStrategyComplex security, IDbContextFactory<ApplicationDbContext> xafDbContextFactory, IHttpContextAccessor contextAccessor) {
        this.xafDbContextFactory = xafDbContextFactory;
        Security = security;
        this.contextAccessor = contextAccessor;
        if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
            Initialize();
        }
    }
    public void Initialize() {
        ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
        ObjectSpaceProvider = GetObjectSpaceProvider(Security);
        Login(Security, ObjectSpaceProvider);
    }
    //...
}
```

- Register `SecurityProvider`, in the [Program.cs](Program.cs).

    ```csharp
    builder.Services.AddScoped<SecurityProvider>();
    ```


- The `GetObjectSpaceProvider` method provides access to the Object Space Provider.

    ```csharp
    private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
        SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, xafDbContextFactory);
        return objectSpaceProvider;
    }
    ```
    
- The `InitConnection` method authenticates a user both in the Security System and in [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2). 
A user is identified by the user name and password parameters.

    ```csharp
    public bool InitConnection(string userName, string password) {
        AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
        Security.Logoff();
        ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(AuthenticationStandardProvider).Name, parameters);
        IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(Security);
        try {
            Login(Security, objectSpaceProvider);
            SignIn(contextAccessor.HttpContext, userName);
            return true;
        } catch {
            return false;
        }
    }
    //...
    // Logs into the Security System.
    private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
        IObjectSpace objectSpace = ((INonsecuredObjectSpaceProvider)objectSpaceProvider).CreateNonsecuredObjectSpace();
        security.Logon(objectSpace);
    }
    // Signs into HttpContext and creates a cookie.
    private void SignIn(HttpContext httpContext, string userName) {
        List<Claim> claims = new List<Claim>{
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
        ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
        ClaimsPrincipal principal = new ClaimsPrincipal(id);
        httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
    }
    ```

## Step 3. Pages

[Login.cshtml](Pages/Login.cshtml) is a login page that allows you to log into the application.

[Login.cshtml.cs](Pages/Login.cshtml.cs) class uses SecurityProveder and implements the Login logic.

```csharp
public IActionResult OnPost() {
    Response.Cookies.Append("userName", Input.UserName ?? string.Empty);
    if(ModelState.IsValid) {
        if(securityProvider.InitConnection(Input.UserName, Input.Password)) {
            return Redirect("/");
        }
        ModelState.AddModelError("Error", "User name or password is incorrect");
    }
    return Page();
}
```

[Logout.cshtml.cs](Pages/Logout.cshtml.cs) class implements the Logout logic

```csharp
public void OnGet() {
    Input = new InputModel();
    string userName = Request.Cookies["userName"]?.ToString();
    Input.UserName = userName ?? "User";
}
public class InputModel {
    [Required(ErrorMessage = "User name is required")]
    public string UserName { get; set; }
    public string Password { get; set; }
}
```

[Index.razor](Pages/Index.razor) is the main page. It configures the [Blazor Data Grid](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxDataGrid-1) and allows a use to log out.

The `OnInitialized` method creates an ObjectSpace instance and gets Employee and Department objects.

```csharp
protected override void OnInitialized() {
    objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
    employees = objectSpace.GetObjectsQuery<Employee>();
    departments = objectSpace.GetObjectsQuery<Department>();
}
```

The `HandleValidSubmit` method saves changes if data is valid.

```csharp
async Task HandleValidSubmit() {
    ObjectSpace.CommitChanges();
    await grid.Refresh();
    employee = null;
    await grid.CancelRowEdit();
}
```

The `OnRowRemoving` method removes an object.

```csharp
Task OnRowRemoving(object item) {
    ObjectSpace.Delete(item);
    ObjectSpace.CommitChanges();
    return grid.Refresh();
}
```

To show/hide the `New`, `Edit`, `Delete` actions, use the appropriate `CanXXX` methods of the Security System.

```razor
<DxDataGridCommandColumn Width="100px">
    <HeaderCellTemplate>
        @if(securityProvider.Security.CanCreate<Employee>()) {
            <button class="btn btn-link" @onclick="@(() => StartRowEdit(null))">New</button>
        }
    </HeaderCellTemplate>
    <CellTemplate>
        @if(securityProvider.Security.CanWrite(context)) {
            <a @onclick="@(() => StartRowEdit(context))" href="javascript:;">Edit </a>
        }
        @if(securityProvider.Security.CanDelete(context)) {
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
private bool HasAccess => ObjectSpace.IsNewObject(CurrentObject) ?
    SecurityProvider.Security.CanWrite(CurrentObject.GetType(), PropertyName) :
    SecurityProvider.Security.CanRead(CurrentObject, PropertyName);
```

## Step 4: Run and Test the App

- Log in a 'User' with an empty password.
  ![](/images/Blazor_LoginPage.png)

- Note that secured data is displayed as '*******'.
  ![](/images/Blazor_ListView.png)

- Press the **Logout** button and log in as 'Admin' to see all the records.
