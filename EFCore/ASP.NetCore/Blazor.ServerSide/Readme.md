<!-- default file list -->

This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a Blazor Server App with Entity Framework Core.
You will also see how to execute Create, Write and Delete data operations and take security permissions into account.

## Prerequisites

- Make sure that your environment meets [Microsoft Blazor Prerequisites](https://docs.devexpress.com/Blazor/401055/getting-started/prerequisites).
- [Download and run our Unified Component Installer](https://www.devexpress.com/Products/Try/)
  - *We recommend that you select all products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*

## Create and Configure a New Project

- Follow the [Create a New Blazor Application](https://docs.devexpress.com/Blazor/401057/getting-started/create-a-new-application) article to create a Blazor project from scratch.
- Add the [EFCore/BusinessObjectsLibrary](../BusinessObjectsLibrary) project reference. *BusinessObjectsLibrary* adds important NuGet dependencies:
    ```xml
	<PackageReference Include="Microsoft.EntityFrameworkCore" Version="5.0.0" />
	<PackageReference Include="DevExpress.ExpressApp.EFCore" Version="21.1.3" />
    <PackageReference Include="DevExpress.Persistent.Base" Version="21.1.3" />
    <PackageReference Include="DevExpress.Persistent.BaseImpl.EFCore" Version="21.1.3" />
    ```
    The `DevExpress.Persistent.BaseImpl.EFCore` NuGet package contains the PermissionPolicyUser, PermissionPolicyRole and other XAF's Security System API.
	Update package versions if you are using a higher DevExpress.Blazor version in the Blazor Application project.
- Add NuGet packages for Entity Framework Core with SQL Server:
    ```xml
    <PackageReference Include="Microsoft.EntityFrameworkCore.Proxies" Version="5.0.0" />
    <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="5.0.0" />
    ```
- Create the Helpers folder and add a new code file to it. Set the file name to SecurityProvider.cs and declare the SecurityProvider class. The SecurityProvider class creates and initializes required XAF components ([SecurityStrategyComplex](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategyComplex), [SecuredEFCoreObjectSpaceProvider](https://docs.devexpress.com/eXpressAppFramework/DevExpress.EntityFrameworkCore.Security.SecuredEFCoreObjectSpaceProvider?p=netstandard)) and calls the SecurityStrategyBase.Logon method.
---

## Step 1. Configure the Blazor Application

For detailed information about the ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/get-started?view=aspnetcore-3.1&tabs=visual-studio).

Configure the Blazor Application in the `ConfigureServices` and `Configure` methods of [Startup.cs](Startup.cs):

```csharp
public void ConfigureServices(IServiceCollection services) {
	services.AddSession();
	services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		 .AddCookie();
	services.AddRazorPages();
	services.AddServerSideBlazor();
	services.AddHttpContextAccessor();
	services.AddXafSecurity().AddSecuredTypes(typeof(Employee), typeof(PermissionPolicyUser), typeof(PermissionPolicyRole));
}

public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
	if(env.IsDevelopment()) {
		app.UseDeveloperExceptionPage();
	}
	else {
		app.UseExceptionHandler("/Error");
		app.UseHsts();
	}
	app.UseSession();
	app.UseStaticFiles();
	app.UseAuthentication();
	app.UseDefaultFiles();
	app.UseHttpsRedirection();
	app.UseRouting();
	app.UseEndpoints(endpoints => {
		endpoints.MapFallbackToPage("/_Host");
		endpoints.MapBlazorHub();
	});
}
```

- The `AddXafSecurity` method registers the `XpoDataStoreProviderService` and SecurityProvider services.

```csharp
public static IServiceCollection AddXafSecurity(this IServiceCollection services) {
	return services.AddSingleton<XpoDataStoreProviderService>()
		.AddScoped<SecurityProvider>();
}
```

- The `AddSecuredTypes` method registers types that will be processed by the Security System.

```csharp
public static IServiceCollection AddSecuredTypes(this IServiceCollection service, params Type[] types) {
	return service.Configure<SecurityOptions>(options => {
		options.SecuredTypes = types;
	});
}
```

## Step 2. Initialize Data Store and XAF Security System. Authentication and Permission Configuration

- The [XpoDataStoreProviderService](Helpers/XpoDataStoreProviderService.cs) class provides access to the Data Store Provider object.

```csharp
public class XpoDataStoreProviderService {
	private IXpoDataStoreProvider dataStoreProvider;
	private string connectionString;
	public XpoDataStoreProviderService(IConfiguration config) {
		connectionString = config.GetConnectionString("XafApplication");
	}
	public IXpoDataStoreProvider GetDataStoreProvider() {
		if(dataStoreProvider == null) {
			dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
		}
		return dataStoreProvider;
	}
}
```

- In appsettings.json, add the connection string and replace "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.

```json
"ConnectionStrings": {
  "XafApplication": "Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"
}
```

The [SecurityProvider](Helpers/SecurityProvider.cs) class provides access to the XAF Security System functionality.

```csharp
public SecurityProvider(XpoDataStoreProviderService xpoDataStoreProviderService, IHttpContextAccessor contextAccessor, IOptions<AppSettings> appSettingsAccessor) {
	this.xpoDataStoreProviderService = xpoDataStoreProviderService;
	this.contextAccessor = contextAccessor;
	appSettings = appSettingsAccessor.Value;
	if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
		Initialize();
	}
}
```

The `Initialize` method initializes the `Security` and `ObjectSpaceProvider` properties and [logs in](<https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.Logon(System.Object)>) to the Security System.

```csharp
public void Initialize() {
	Security = GetSecurity(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
	ObjectSpaceProvider = GetObjectSpaceProvider(Security);
	Login(Security, ObjectSpaceProvider);
}
private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
	IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
	security.Logon(objectSpace);
}
```

The `GetSecurity` method initializes the Security System instance and registers authentication providers.

The `AuthenticationMixed` class allows you to register several authentication providers to use both the [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and ASP.NET Core Identity authentication.

```csharp
private SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
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

The `GetObjectSpaceProvider` method initializes the Object Space Provider.

```csharp
private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
	SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProviderService.GetDataStoreProvider(), true);
	RegisterEntities(objectSpaceProvider);
	return objectSpaceProvider;
}
// Registers all business object types you use in the application.
private void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
	if(securedTypes != null) {
		foreach(Type type in securedTypes) {
			objectSpaceProvider.TypesInfo.RegisterEntity(type);
		}
	}
}
```

The `InitConnection` method authenticates a user both in the Security System and [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2).
A user is identified by user name and password parameters.

```csharp
public bool InitConnection(string userName, string password) {
	AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
	SecurityStrategyComplex security = GetSecurity(typeof(AuthenticationStandardProvider).Name, parameters);
	IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(security);
	try {
		Login(security, objectSpaceProvider);
		SignIn(contextAccessor.HttpContext, userName);
		return true;
	}
	catch {
		return false;
	}
}
// Signs into Security System.
private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
	IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
	security.Logon(objectSpace);
}
// Signs into HttpContext.
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
public IActionResult OnGet() {
    httpContext.SignOutAsync();
    return Redirect("/Login");
}
```

[Index.razor](Pages/Index.razor) is the main page. It configures the [Blazor Data Grid](https://docs.devexpress.com/Blazor/DevExpress.Blazor.DxDataGrid-1) and allows a use to log out.

The `OnInitialized` method creates an ObjectSpace instance and gets Employee and Department objects.

```csharp
protected override void OnInitialized() {
	ObjectSpace = SecurityProvider.ObjectSpaceProvider.CreateObjectSpace();
	employees = ObjectSpace.GetObjectsQuery<Employee>();
	departments = ObjectSpace.GetObjectsQuery<Department>();
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
		@if (SecurityProvider.Security.CanCreate<Employee>())
		{
			<button class="btn btn-link" @onclick="@(() => StartRowEdit(null))">New</button>
		}
	</HeaderCellTemplate>
	<CellTemplate>
		@if (SecurityProvider.Security.CanWrite(context))
		{
			<a @onclick="@(() => StartRowEdit(context))" href="javascript:;">Edit</a>
		}
		@if (SecurityProvider.Security.CanDelete(context))
		{
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
<DxDataGridColumn Field=@nameof(Employee.FirstName)>
    <DisplayTemplate>
        <SecuredContainer Context="readOnly" CurrentObject=@context PropertyName=@nameof(Employee.FirstName)>
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

- Log in under 'User' with an empty password.
  ![](/images/Blazor_LoginPage.png)

- Note that secured data is displayed as '*******'.
  ![](/images/Blazor_ListView.png)

- Press the Logout button and log in under 'Admin' to see all records.
