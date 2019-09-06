This example demonstrates how to protect your data with the [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the following client-server web app:
 * Server: an OData v4 service built with [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2).
 * Client: an HTML/JavaScript app with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/).
 
## Prerequisites
* [Visual Studio 2017 or 2019](https://visualstudio.microsoft.com/vs/) with the following workloads:
  * **ASP.NET and web development**
  * **.NET Core cross-platform development**
* [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/all)
* [DevExpress Unified Installer for .NET and HTML5 Developers](https://www.devexpress.com/Products/Try/)
  * We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.
- Build the solution and run the *XafSolution.Win* project to log in under 'User' or 'Admin' with an empty password. The application will generate a database with business objects from the *XafSolution.Module* project. 
- Add the *XafSolution.Module* assembly reference to your application.

***

## Step 1: Configure the ASP.NET Core Server App
For detailed information about ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-2.2&tabs=windows).
- Configure the OData and MVC pipelines in the `ConfigureServices` and `Configure` methods of [Startup.cs](Startup.cs):

``` csharp
public void ConfigureServices(IServiceCollection services) {
    services.AddOData();
    services.AddMvc(options => {
        options.EnableEndpointRouting = false;
    }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
}
public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
    if(env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
    }
    else {
        app.UseHsts();
    }
    app.UseMvc(routeBuilder => { // Add MVC to the request pipeline.
        routeBuilder.EnableDependencyInjection();
        routeBuilder.Count().Expand().Select().OrderBy().Filter().MaxTop(null);
        routeBuilder.MapODataServiceRoute("ODataRoutes", null, GetEdmModel()); // Add the ODATA routing to MVC.
    });
}
```
		
- Define the EDM model that contains data description for all used entities. The [PermissionContainer](/Helpers/PermissionContainer.cs) class is used as container to transfer permissions to the client side.

``` csharp
private IEdmModel GetEdmModel() {
    ODataModelBuilder builder = new ODataConventionModelBuilder();
    EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
    EntitySetConfiguration<Party> parties = builder.EntitySet<Party>("Parties");
    EntitySetConfiguration<PermissionContainer> permissions = builder.EntitySet<PermissionContainer>("Permissions");
    EntitySetConfiguration<Department> departments = builder.EntitySet<Department>("Departments");

    permissions.EntityType.HasKey(t => t.Key);
    employees.EntityType.HasKey(t => t.Oid);
    parties.EntityType.HasKey(t => t.Oid);
    departments.EntityType.HasKey(t => t.Oid);
    return builder.GetEdmModel();
}
//...
public class PermissionContainer {
    // Сontains member permissions: member names are keys and permissions are boolean values.
    public IDictionary<string, object> Data { get; set; }
    // Сontains an object key that identifies this permission container's related business object.
    public string Key { get; set; }
    public PermissionContainer() {
        Data = new Dictionary<string, object>();
    }
}
```

- Enable the authentication service and configure the request pipeline with the authentication middleware in the `ConfigureServices` and `Configure` methods. [UnauthorizedRedirectMiddleware](/UnauthorizedRedirectMiddleware.cs) сhecks if the ASP.NET Core Identity is authenticated. If not, it redirects a user to the authentication page.

``` csharp
public void ConfigureServices(IServiceCollection services) {
    // ...
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
      .AddCookie(); // !!!
}
public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
    // ...
    app.UseAuthentication(); // !!!
    app.UseMiddleware<UnauthorizedRedirectMiddleware>(); // !!!
    app.UseDefaultFiles();
    app.UseStaticFiles();
    app.UseHttpsRedirection();
    app.UseCookiePolicy();
    app.UseMvc(routeBuilder => {
        routeBuilder.EnableDependencyInjection();
        routeBuilder.Count().Expand().Select().OrderBy().Filter().MaxTop(null);
        routeBuilder.MapODataServiceRoute("ODataRoutes", null, GetEdmModel());
    });
}
//...
public class UnauthorizedRedirectMiddleware {
    public async Task InvokeAsync(HttpContext context) {
        if(context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated
          || IsAllowAnonymous(context)) {
            await _next(context);
                    else {
                context.Response.Redirect(authenticationPagePath);
            }
        }
        // ...
    }
}
```

## Step 2: Initialize Data Store and XAF's Security System

The [SecurityProvider](/Helpers/SecurityProvider.cs) class contains helper functions that provide access to XAF Security System functionality.

- Register `SecurityProvider` in the `ConfigureServices` method

``` csharp
public void ConfigureServices(IServiceCollection services) {
    // ...
    services.AddScoped<SecurityProvider>();
}
```

- The `GetSecurity` method provides access to the Security System instance and registers authentication providers. The `AuthenticationMixed` class allows you to register several authentication providers, so you can use both [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and ASP.NET Core Identity authentication.

``` csharp
public SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
    AuthenticationMixed authentication = new AuthenticationMixed();
    authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
    authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
    authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
    authentication.SetupAuthenticationProvider(authenticationName, parameter);
    SecurityStrategyComplex security = new SecurityStrategyComplex(
        typeof(PermissionPolicyUser),
        typeof(PermissionPolicyRole),
        authentication
    );
    security.RegisterXPOAdapterProviders();
    return security;
}
```

- The `GetObjectSpaceProvider` method provides access to the Object Space Provider. The [XpoDataStoreProviderService](/Helpers/XpoDataStoreProviderService.cs) class provides access to the Data Store Provider object.

``` csharp
public IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security, XpoDataStoreProviderService xpoDataStoreProviderService, string connectionString) {
    SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProviderService.GetDataStoreProvider(connectionString, null, true), true);
    RegisterEntities(objectSpaceProvider);
    return objectSpaceProvider;
}
//...
public class XpoDataStoreProviderService {
    private IXpoDataStoreProvider dataStoreProvider;
    public IXpoDataStoreProvider GetDataStoreProvider(string connectionString, IDbConnection connection, bool enablePoolingInConnectionString) {
        if(dataStoreProvider == null) {
            dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, enablePoolingInConnectionString);
        }
        return dataStoreProvider;
    }
}
// Registers all business object types you use in the application.
private void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
    objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
    objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
    objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
}
```
	
- The `InitConnection` method authenticates a user both in the Security System and in [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2). A user is identified by the user name and password parameters.

``` csharp
public bool InitConnection(string userName, string password, HttpContext httpContext, XpoDataStoreProviderService xpoDataStoreProviderService, string connectionString) {
    AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(
      userName, password
    );
    SecurityStrategyComplex security = GetSecurity(typeof(AuthenticationStandardProvider).Name, parameters);
    IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(
      security, xpoDataStoreProviderService, connectionString
    );
    try {
        Login(security, objectSpaceProvider);
        SignIn(httpContext, userName);
        return true;
    }
    catch {
        return false;
    }
}
//...
// Logs into the Security System.
public void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
    IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
    security.Logon(objectSpace);
}
// Signs into HttpContext and creates a cookie.
private void SignIn(HttpContext httpContext, string userName) {
    List<Claim> claims = new List<Claim>{
            new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
          };
    ClaimsIdentity id = new ClaimsIdentity(
      claims, "ApplicationCookie",
      ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType
    );
    ClaimsPrincipal principal = new ClaimsPrincipal(id);
    httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
}
```

## Step 3: Implement OData Controllers for CRUD, Login, Logoff, etc.

- [BaseController](/Controllers/BaseController.cs) contains logic common for all controllers. [AccountController](/Controllers/AccountController.cs) and [SecuredController](/Controllers/SecuredController.cs) are derived from BaseController.

``` csharp
[Route("api/[controller]")]
public class BaseController : ODataController {
	protected XpoDataStoreProviderService XpoDataStoreProviderService { get; set; }
	protected SecurityProvider SecurityProvider { get; set; }
	protected IConfiguration Config { get; set; }
	public BaseController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper) {
		XpoDataStoreProviderService = xpoDataStoreProviderService;
		Config = config;
		SecurityProvider = securityHelper;
	}
}
```

- Add the `Configuration` property to [Startup.cs](/Startup.cs) and register it as a singleton to have access to connectionString from controllers. The `IConfiguration` object is used to access the application configuration [appsettings.json](/appsettings.json) file.

``` csharp		
//...
public IConfiguration Configuration { get; }
public Startup(IConfiguration configuration) {
    Configuration = configuration;
}
public void ConfigureServices(IServiceCollection services) {
    //...
    services.AddSingleton(Configuration);
}
```	
- In appsettings.json, replace "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.

``` json
"ConnectionStrings": {
  "XafApplication": "Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"
}
```

- Register HttpContextAccessor in the `ConfigureServices` method to access [HttpContext](https://docs.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8) in controller constructors.

``` csharp
public void ConfigureServices(IServiceCollection services) {
    // ...
    services.AddHttpContextAccessor();
}
```

- [SecuredController](/Controllers/SecuredController.cs) has logic to initialize the Security System with the appropriate authentication provider and the Object Space Provider. The identity authentication provider name and [Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=visual-studio) are used as `GetSecurity` method's parameters.  [ActionsController](/Controllers/ActionsController.cs), [DepartmentsController](/Controllers/DepartmentsController.cs) and [EmployeesController](/Controllers/EmployeesController.cs) are derived from SecuredController.

``` csharp
public class SecuredController : BaseController {
	protected SecurityStrategyComplex Security { get; set; }
	protected IObjectSpace ObjectSpace { get; set; }
	public SecuredController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper, IHttpContextAccessor contextAccessor)
	: base(xpoDataStoreProviderService, config, securityHelper) {
		Security = SecurityProvider.GetSecurity(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
		string connectionString = Config.GetConnectionString("XafApplication");
		IObjectSpaceProvider objectSpaceProvider = SecurityProvider.GetObjectSpaceProvider(Security, XpoDataStoreProviderService, connectionString);
		SecurityProvider.Login(Security, objectSpaceProvider);
		ObjectSpace = objectSpaceProvider.CreateObjectSpace();
	}
}
```

- [EmployeesController](/Controllers/EmployeesController.cs) implements the `Get` method to get access to Employee objects. [DepartmentsController](/Controllers/DepartmentsController.cs) contains similar implementation that obtains Department objects.

``` csharp
public class EmployeesController : SecuredController {
    public EmployeesController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, SecurityProvider securityHelper, IHttpContextAccessor contextAccessor)
			: base(xpoDataStoreProviderService, config, securityHelper, contextAccessor) { }
    [HttpGet]
    [EnableQuery]
    public ActionResult Get() {
        IQueryable<Employee> employees = ObjectSpace.GetObjects<Employee>().AsQueryable();
        return Ok(employees);
    }
}
```

- [AccountController](/Controllers/AccountController.cs) handles the Login and Logoff operations.
The `Login` method is called when a user clicks the `Login` button on the login page. The `Logoff` method is called when a user clicks the `Logoff` button on the main page.

``` csharp
// Attempts to log users with accepted credentials into the Security System.
[HttpGet]
[ODataRoute("Login(userName={userName}, password={password})")]
[AllowAnonymous]
public ActionResult Login(string userName, string password) {
    ActionResult result;
    string connectionString = Config.GetConnectionString("XafApplication");
    if(SecurityProvider.InitConnection(userName, password, HttpContext, XpoDataStoreProviderService, connectionString)) {
        result = Ok();
    }
    else {
        // If anything goes wrong during the authentication process, this method returns the Unauthorized response.
        result = Unauthorized();
    }
    return result;
}
// Signs the user out of the HttpContext.
[HttpGet]
[ODataRoute("Logoff()")]
public ActionResult Logoff() {
    HttpContext.SignOutAsync();
    return Ok();
}
```
		
- [ActionsController](/Controllers/ActionsController.cs) contains additional methods that process permissions.

``` csharp
// Gathers permissions for all objects on the DevExtreme Data Grid current page and sends them to the client side as part of the response.
[HttpPost]
[ODataRoute("GetPermissions")]
public ActionResult GetPermissions(ODataActionParameters parameters) {
    ActionResult result = NoContent();
    if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
        List<string> keys = new List<string>(parameters["keys"] as IEnumerable<string>);
        string typeName = parameters["typeName"].ToString();
        List<PermissionContainer> permissionContainerList = new List<PermissionContainer>();
        ITypeInfo typeInfo = ObjectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
        if(typeInfo != null) {
            Type type = typeInfo.Type;
            IList entityList = ObjectSpace.GetObjects(type, new InOperator(typeInfo.KeyMember.Name, keys));
            foreach(var entity in entityList) {
                PermissionContainer permissionContainer = new PermissionContainer();
                permissionContainer.Key = typeInfo.KeyMember.GetValue(entity).ToString();
                IEnumerable<IMemberInfo> memberList = GetPersistentMembers(typeInfo);
                foreach(IMemberInfo member in memberList) {
                    bool permission = Security.IsGranted(new PermissionRequest(ObjectSpace, type, SecurityOperations.Read, entity, member.Name));
                    permissionContainer.Data.Add(member.Name, permission);
                }
                permissionContainerList.Add(permissionContainer);
            }
            result = Ok(permissionContainerList.AsQueryable());
        }
    }
    return result;
}
// Returns only visible persistent members which are displayed in the grid.
private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
    return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
}
```
	
## Step 4: Implement the Client-Side App
- The authentication page ([Authentication.html](/wwwroot/Authentication.html)) and the main page([Index.html](/wwwroot/Index.html)) represent the client side UI.
- [authentication_code.js](/wwwroot/js/authentication_code.js) gathers data from the login page and attempts to log the user in.

``` javascript
$("#validateAndSubmit").dxButton({
  text: "Login",
  type: "success",
  tabIndex: 1,
  onClick: function () {
    var userName = $("#userName").dxTextBox("instance").option("value");
    var password = $("#password").dxTextBox("instance").option("value");
    var url = 'Login(userName = \'' + userName + '\', password = \'' + password + '\')'; 
    $.ajax({
      method: 'GET',
      url: url,
      complete: function (e) {
  if (e.status == 200) {
    document.cookie = "userName=" + userName;
    document.location.href = "/";
  }
  if (e.status == 401) {
    alert("User name or password is incorrect");
  }
      }
    });
  }
});
```	
		
- [index_code.js](/wwwroot/js/index_code.js) configures the DevExtreme Data Grid and logs the user off. The [onLoaded](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/ODataStore/Configuration/#onLoaded) function sends a request to the server to obtain permissions for the current data grid page.

``` javascript
function onLoaded(data) {
  var oids = $.map(data, function (val) {
    return val.Oid._value;
  });
  var data = {
    keys: oids,
    typeName: 'Employee'
  };
  var options = {
    dataType: "json",
    contentType: "application/json",
    type: "POST",
    async: false,
    data: JSON.stringify(data)
  };
  $.ajax("GetPermissions", options)
    .done(function (e) {
      permissions = e.value;
  });
}
```	

- The `onCellPrepared` function handles the data grid's [cellPrepared](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#cellPrepared) event and checks Read operation permissions. If permission is denied, it displays the "Protected Content" text in grid cells:

``` javascript
function onCellPrepared(e) {
  if (e.rowType === "data") {
    var key = e.key._value;
    var objectPermission = getPermission(key);
    if (e.column.command != 'edit') {	
      var dataField = e.column.dataField.split('.')[0];
      if (!objectPermission[dataField]) {
        e.cellElement.text("Protected Content");
      }
    }
  }
}
```	
Note that SecuredObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. Use the [SecurityStrategy.IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) method to determine when to mask default values with the "Protected Content" placeholder in the UI.

- The `getPermission` function returns the permission object for a business object. The business object is identified by the key passed in function parameters:

``` javascript
function getPermission(key) {
  var permission = permissions.filter(function (entry) {
     return entry.Key === key;
  });
  return permission[0];
}
```

## Step 5: Run and Test the App
 - Log in under 'User' with an empty password.

![](/images/ODataLoginPage.png)

 - Notice that secured data is displayed as 'Protected Content'.
![](/images/ODataListView.png)

 - Press the Logoff button and log in under 'Admin' to see all records.
