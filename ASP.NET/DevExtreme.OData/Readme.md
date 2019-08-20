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

## How It Works

1. Log in under 'User' with an empty password.
![](/images/ODataLoginPage.png)
2. Notice that secured data is displayed as 'Protected Content'.
![](/images/ODataListView.png)
3. Press the Logoff button and log in under 'Admin' to see all records.

## Main implementation steps

1. Configure the application</br>
   For detailed information about ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-2.2&tabs=windows).
   	1. ODATA and MVC</br>
Add ODATA and MVC to the application. Insert the following code in the `ConfigureServices()` method of [Startup.cs](Startup.cs):</br>
		```csharp
		public void ConfigureServices(IServiceCollection services) {
		  services.AddOData();
		  services.AddMvc(options => {
		    options.EnableEndpointRouting = false;
		  }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
		}
		```
		Add MVC to the request pipeline in the `Configure()` method of [Startup.cs](Startup.cs):</br>
		```csharp
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
		  if(env.IsDevelopment()) {
		    app.UseDeveloperExceptionPage();
		  }
		  else {
		    app.UseHsts();
		  }
		  app.UseMvc(routeBuilder => {
		    routeBuilder.EnableDependencyInjection();
		    routeBuilder.Count().Expand().Select().OrderBy().Filter().MaxTop(null);
		    routeBuilder.MapODataServiceRoute("ODataRoutes", null, GetEdmModel());
		  });
		}
		```
		`routeBuilder.MapODataServiceRoute` adds the ODATA routing to MVC.</br></br>
	2. EDM model</br>	
	The EDM model contains data description for all used entities:
		```csharp
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
		```	
		The [PermissionContainer](/Helpers/PermissionContainer.cs) class is used as container to transfer permissions to the client side:
		```csharp
		public class PermissionContainer {
		  public IDictionary<string, object> Data { get; set; }
		  public string Key { get; set; }
		  public PermissionContainer() {
		    Data = new Dictionary<string, object>();
		  }
		}
		```	
		The `Key` property contains an object key that identifies this permission container's related business object.</br></br>
		The `Data` property is the dictionary which contains member permissions. Member names are keys and permissions are Boolean values.</br></br>
	3. Authentication configuration</br>	
	Add the authentication service to the `ConfigureServices()` method:
		```csharp
		public void ConfigureServices(IServiceCollection services) {
		  // ...
		  services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		    .AddCookie();
		}
		```
		Configure the request pipeline with the authentication middleware in the `Configure()` method:
		```csharp
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
		  // ...
		  app.UseAuthentication();
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
		```	
	4. [UnauthorizedRedirectMiddleware](/UnauthorizedRedirectMiddleware.cs) redirects unauthorized requests to the authentication page.
		```csharp
		public class UnauthorizedRedirectMiddleware {
		  // ...
		  public async Task InvokeAsync(HttpContext context) {
		    if(context.User!= null && context.User.Identity != null && context.User.Identity.IsAuthenticated 
		      || IsAllowAnonymous(context)) {
		      await _next(context);
                    else {
		      context.Response.Redirect(authenticationPagePath);
		    }
		  }
		  // ...
		}
		```
		The `InvokeAsync()` method checks if the ASP.NET Core Identity is authenticated. If not, it redirects a user to the authentication page.
	
		Add `UnauthorizedRedirectMiddleware` to the request pipeline after the `UseAuthentication` method call:
		```csharp
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
		  // ...
		  app.UseAuthentication();
		  app.UseMiddleware<UnauthorizedRedirectMiddleware>();
		  // ...
		}
		```

2. XAF Security System integration</br>
The [ConnectionHelper](/Helpers/ConnectionHelper.cs) class contains helper functions that provide access to XAF Security System functionality.</br>
	1. The `GetSecurity()` method provides access to the Security System instance and registers authentication providers.
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
		The `AuthenticationMixed` class allows you to register several authentication providers, so you can use both [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and ASP.NET Core Identity authentication.</br>
	2. The `GetObjectSpaceProvider()` method provides access to the Object Space Provider.
		```csharp
		public static IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security, XpoDataStoreProviderService xpoDataStoreProviderService, string connectionString) {
		  SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProviderService.GetDataStoreProvider(connectionString, null, true), true);
		  RegisterEntities(objectSpaceProvider);
		  return objectSpaceProvider;
		}
		```
		The [XpoDataStoreProviderService](/Helpers/XpoDataStoreProviderService.cs) class provides access to the Data Store Provider object:
		```csharp
		public class XpoDataStoreProviderService {
		  private IXpoDataStoreProvider dataStoreProvider;
		  public IXpoDataStoreProvider GetDataStoreProvider(string connectionString, IDbConnection connection, bool enablePoolingInConnectionString) {
		    if(dataStoreProvider == null) {
		      dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, enablePoolingInConnectionString);
		    }
		    return dataStoreProvider;
		  }
		}
		```	
		The `RegisterEntities()` method registers all business object types you use in the application:
		```csharp
		private static void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
		  objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
		  objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
		  objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
		}
		```
	
	3. The `InitConnection()` method authenticates a user both in the Security System and in [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2). A user is identified by the user name and password parameters.
		```csharp
		public static bool InitConnection(string userName, string password, HttpContext httpContext, 
		  XpoDataStoreProviderService xpoDataStoreProviderService, string connectionString) {
		  AuthenticationStandardLogonParameters parameters = new AuthenticationStandardLogonParameters(userName, password);
		  SecurityStrategyComplex security = GetSecurity(typeof(AuthenticationStandardProvider).Name, parameters);
		  IObjectSpaceProvider objectSpaceProvider = GetObjectSpaceProvider(security, xpoDataStoreProviderService, connectionString);
		  try {
		    Login(security, objectSpaceProvider);
		    SignIn(httpContext, userName);
		    return true;
		  }
		  catch {
		    return false;
		  }
		}
		```	
		The `Login()` method logs into the Security System:
		```csharp
		public static void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
		  IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
		  security.Logon(objectSpace);
		}
		```	
		The `SignIn()` method signs into HttpContext and creates a cookie:
		```csharp
		private static void SignIn(HttpContext httpContext, string userName) {
		  List<Claim> claims = new List<Claim>{
		    new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
		  };
		  ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
		  ClaimsPrincipal principal = new ClaimsPrincipal(id);
		  httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
		}
		```
	
3. Controllers provide access to business data, handle actions such as log in/log off, and obtain permissions.</br>
	1. The [BaseController](/Controllers/BaseController.cs) class contains logic common for all controllers. The code initializes the Security System and the Object Space Provider.
		```csharp
		[Route("api/[controller]")]
		public class BaseController : ODataController {
		  protected SecurityStrategyComplex Security { get; set; }
		  protected IObjectSpace ObjectSpace { get; set; }
		  protected XpoDataStoreProviderService XpoDataStoreProviderService { get; set; }
		  protected IConfiguration Config { get; set; }
		  public BaseController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config) {
		    XpoDataStoreProviderService = xpoDataStoreProviderService;
		    Config = config;
		  }
		  protected void Init() {
		    Security = ConnectionHelper.GetSecurity(typeof(IdentityAuthenticationProvider).Name, HttpContext?.User?.Identity);
		    string connectionString = Config.GetConnectionString("XafApplication");
		    IObjectSpaceProvider objectSpaceProvider = ConnectionHelper.GetObjectSpaceProvider(Security, XpoDataStoreProviderService, connectionString);
		    ConnectionHelper.Login(Security, objectSpaceProvider);
		    ObjectSpace = objectSpaceProvider.CreateObjectSpace();
		  }
		}
		```	
		Initialize the Security System with the appropriate authentication provider. Specify the identity authentication provider name and [Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=visual-studio) as `GetSecurity()` method's parameters.</br></br>
		The `IConfiguration` object is used to access the application configuration [appsettings.json](/appsettings.json) file. This file contains the database connection string:
		```csharp
		"ConnectionStrings": {
		  "XafApplication": "Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"
		}
		```
		Replace 'DBSERVER' with your data source name to set connection to your database.</br></br>
		Add the `Configuration` property to [Startup.cs](/Startup.cs) and register it as a singleton to have access to  connectionString from controllers:
		```csharp
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
		Use the object space provider to log in to the Security System and create the `ObjectSpace` instance to get access to data.</br></br>
	2. The [EmployeesController](/Controllers/EmployeesController.cs) class is a BaseController descendant. It implements the Get() method to get access to Employee objects.
		```csharp
		public class EmployeesController : BaseController {
		  public EmployeesController(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config): base(xpoDataStoreProviderService, config) {}
		  [HttpGet]
		  [EnableQuery]
		  public ActionResult Get() {
		    Init();
		    IQueryable<Employee> employees = ObjectSpace.GetObjects<Employee>().AsQueryable(); 
		    return Ok(employees);
		  }
		}
		```	
		The [DepartmentsController](/Controllers/DepartmentsController.cs) class contains similar implementation that obtains Department objects.</br></br>
	3. The [AccountController](/Controllers/AccountController.cs) class handles the log in / log off actions.</br>	
	The `Login()` method is called when a user performs the login action on the login page. The method attempts to log users with accepted credentials into the Security System:
		```csharp
		[HttpGet]
		[ODataRoute("Login(userName={userName}, password={password})")]
		[AllowAnonymous]
		public ActionResult Login(string userName, string password) {
		  ActionResult result;
		  string connectionString = Config.GetConnectionString("XafApplication");
		  if(ConnectionHelper.InitConnection(userName, password, HttpContext, XpoDataStoreProviderService, connectionString)) {
		    result = Ok();
		  }
		  else {
		    result = Unauthorized();
		  }
		  return result;
		}
		```	
		If anything goes wrong during the authentication process, this method returns the Unauthorized response.</br>	
		The `Logoff` method is called when a user initiates a log off action on the main page. The method signs the user out of the HttpContext:
		```csharp
		[HttpGet]
		[ODataRoute("Logoff()")]
		public ActionResult Logoff() {
		  HttpContext.SignOutAsync();
		  return Ok();
		}
		```
		
	4. [ActionsController](/Controllers/ActionsController.cs) contains additional methods that process permissions.</br>	
	The `GetPermissions()` method gathers permissions for all objects on the DevExtreme Data Grid current page and sends them to the client side as part of the response:
		```csharp
		[HttpPost]
		[ODataRoute("GetPermissions")]
		public ActionResult GetPermissions(ODataActionParameters parameters) {
		  Init();
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
		```	
		Alternatively, `GetPersistentMembers()` method returns only visible persistent members which are displayed in the grid:
		```csharp
		private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
		  return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
		}
		```
	
4. Implement the client-side code.
	1. The authentication page ([Authentication.html](/wwwroot/Authentication.html)) and the main page([Index.html](/wwwroot/Index.html)) represent the client side UI.
	2. [authentication_code.js](/wwwroot/js/authentication_code.js) gathers data from the login page and attempts to log the user in.
		```javascript
		$("#validateAndSubmit").dxButton({
		  text: "Login",
		  type: "success",
		  onClick: function () {
		    var userName = $("#userName").dxTextBox("instance").option("value");
		    var password = $("#password").dxTextBox("instance").option("value");
		    var url = 'Login(userName = \'' + userName + '\', password = \'' + password + '\')'; 
		    $.ajax({
		      method: 'GET',
		      url: url,
		      complete: function (e) {
			if (e.status == 200) {
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
		
	3. [index_code.js](/wwwroot/js/index_code.js) configures the DevExtreme Data Grid and logs the user off.</br>
	The [onLoaded](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/ODataStore/Configuration/#onLoaded) function sends a request to the server to obtain permissions for the current data grid page:
		```javascript
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
		The `onCellPrepared` function handles the data grid's [cellPrepared](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#cellPrepared) event and checks Read operation permissions. If permission is denied, it displays the "Protected Content" text in grid cells:
		```javascript
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
		The `getPermission()` function returns the permission object for a business object. The business object is identified by the key passed in function parameters:
		```javascript
		function getPermission(key) {
		  var permission = permissions.filter(function (entry) {
		     return entry.Key === key;
		  });
		  return permission[0];
		}
		```
