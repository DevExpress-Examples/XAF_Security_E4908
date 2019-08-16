This example demonstrates how to protect your data with the [XAF's Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the following client-server web app:
 * Server: an OData v4 service built with the [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2).
 * Client: an HTML/JavaScript app with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/).
 
## Prerequisites
* [Visual Studio 2017 or 2019](https://visualstudio.microsoft.com/vs/) with the following workloads:
  * **ASP.NET and web development**
  * **.NET Core cross-platform development**
* [.NET Core SDK 2.2 or later](https://www.microsoft.com/net/download/all)
* [Unified Installer for .NET and HTML5 Developers](https://www.devexpress.com/Products/Try/)
  * We recommend that you select all the DevExpress products when you run the installation. It will register local NuGet package sources, item and project templates required for these tutorials. You can uninstall unnecessary components later.
- Build the solution and run the *XafSolution.Win* project to log in under 'User' or 'Admin' with an empty password. It will generate a database with business objects from the *XafSolution.Module* project.

## How It Works

1. Log in under 'User' with an empty password in the basic logon form.
![](/images/ODataLoginPage.png)
2. Notice that secured data is displayed as 'Protected Content'.
![](/images/ODataListView.png)
3. Press the Logoff button and log in under 'Admin' to see all records.

## Main implementation steps

1. Configure the application

You can find more information about configuring the ASP.Net Core application in the [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-2.2&tabs=windows).

	1.1. ODATA and MVC
	Add ODATA and MVC to the application with following code in the ConfigureServices method in the [Startup.cs](Startup.cs):
	```csharp
	public void ConfigureServices(IServiceCollection services) {
		services.AddOData();
		services.AddMvc(options => {
			options.EnableEndpointRouting = false;
		}).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
	}
	```
	Add MVC to the request pipeline in the Configure method in the [Startup.cs](Startup.cs):
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
	The routeBuilder.MapODataServiceRoute adds the ODATA routing to MVC.
	
	1.2. EDM model
	
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
	
	The Key property contains the object key to identificate which business object this permission container is related with.
	
	The Data property is the dictionary which contains the member permissions and where the member name is a key and the permission is the bool value.
	
	1.3. Configure the authentication
	
	Add the authentication service in the ConfigureServices method:
	```csharp
	public void ConfigureServices(IServiceCollection services) {
			// ...
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie();
		}
	```
	Configure the request pipeline with the authentication middleware in the Configure method:
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
	
	1.4. The [UnauthorizedRedirectMiddleware](/UnauthorizedRedirectMiddleware.cs) redirects unauthorized requests to the authentication page
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
	In the InvokeAsync method checks if the ASP.Net Core Identity is authenticated and, if not, redirect the user to the authentication page.
	
	Add the UnauthorizedRedirectMiddleware to the request pipeline after the UseAuthentication method call:
	```csharp
	public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
		// ...
		app.UseAuthentication();
		app.UseMiddleware<UnauthorizedRedirectMiddleware>();
		// ...
	}
	```

2. XAF Security System integration

The [ConnectionHelper](/Helpers/ConnectionHelper.cs) class contains some helper functions to provide access to XAF Security System functionality.

	2.1. The GetSecurity method provides access to the Security System instance and registers authentication providers
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
	The AuthenticationMixed class allows to register several authentication providers, so you can use both the [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and the ASP.Net Core Identity authentication.
	
	2.2. The GetObjectSpaceProvider method provides access to the Object Space Provider
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
	
	The RegisterEntities method registers all business object types we use in the application:
	```csharp
	private static void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
		objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
		objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
		objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
	}
	```
	
	2.3. The InitConnection method authenticates the user both in the Security System and in the [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2) by the user name and password parameters
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
	
	The Login method performs logging in to the Security System:
	```csharp
	public static void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
		IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
		security.Logon(objectSpace);
	}
	```
	
	The SignIn method performs signing in to the HttpContext and creates the cookie:
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
	
3. Controllers provide the access to business data and handle the actions such as log in/log off and get permissions

	3.1. The [BaseController](/Controllers/BaseController.cs) contains common for all controllers logic which initializes the Security System and the Object Space Provider.
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
	
	- Initialize the Security System with the appropriate authentication provider passing the identity authentication provider name and the [Identity](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity?view=aspnetcore-2.2&tabs=visual-studio) to the GetSecurity method as parameters.
	
	- The IConfiguration object is used to get access to the application configuration [appsettings.json](/appsettings.json) file. This file contains the connection string to a database:
	```csharp
	"ConnectionStrings": {
		"XafApplication": "Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"
	}
	```
		Replace 'DBSERVER' with your data source name to set connection to your database.
	
		Add the Configuration property to the [Startup.cs](/Startup.cs) file and register it as a singleton to have an access to the connectionString in the controllers:
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
	
	- Get the object space provider, log in to the Security System and then create the ObjectSpace instance to get access to data
	
	3.2. The [EmployeesController](/Controllers/EmployeesController.cs) class is a descendant from the BaseController class and implements the Get method to get access to Employee objects
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
	
	The DepartmentsController class contains the similar implementation to get access to Department objects.
	
	3.3. The [AccountController](/Controllers/AccountController.cs) class handles the log in and log off actions
	
	The Login method is called when the user perform the login action on the login page and tries to log in the user to the Security System with passed credentials:
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
	
	If something goes wrong during the authentication process, this method will return the Unauthorized response.
	
	The Logoff method is called when the user perform a log off action on the main page and performs signing out from the HttpContext:
	```csharp
	[HttpGet]
		[ODataRoute("Logoff()")]
		public ActionResult Logoff() {
			HttpContext.SignOutAsync();
			return Ok();
		}
	```
	
	3.4. The [ActionsController](/Controllers/ActionsController.cs) contains an additional methods to process permissions
	
	The GetPermissions method gathers permissions for all objects on the DxDataGrid current page and send them to the client as part of the response:
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
	
	The additional GetPersistentMembers method returns only visible persistent members which are displayed in the grid:
	```csharp
	private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
		return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
	}
	```
	
4. Implement the client side

	4.1. The authentication page([Authentication.html](/wwwroot/Authentication.html)) and the main page([Index.html](/wwwroot/Index.html)) represent the client side UI
	
	4.2. The [authentication_code.js](/wwwroot/js/authentication_code.js) javascript file gathers the data from the login page and performs the login action
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
	
	4.3. The [index_code.js](/wwwroot/js/index_code.js) javascript file configures the DevExtreme Data Grid and performs the log off action
	
	The [onLoaded](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/ODataStore/Configuration/#onLoaded) function makes a request to the server to get permissions for the current data grid page:
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
	
	The onCellPrepared function handles the data grid [cellPrepared](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#cellPrepared) event and checks read operation permissions to display the "Protected Content" text in the cell or not:
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
	
	The getPermission function returns the permission object related to the business object with the key passed in function parameters:
	```javascript
	function getPermission(key) {
		var permission = permissions.filter(function (entry) {
			return entry.Key === key;
		});
		return permission[0];
	}
	```