This example demonstrates how to protect your data with the [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the following client-server web app:
 * Server: an OData v4 service built with [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-2.2).
 * Client: an HTML/JavaScript app with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/).
 
>There is also an example which uses .Net Core as Target Framework. If you are interested in the .Net Core example, run the [ODataService.NetCore](ODataService.NetCore.csproj) project 
from the [NonXAFSecurityExamples.NetCore](/NonXAFSecurityExamples.NetCore.sln) solution.
 
## Prerequisites
- [Visual Studio 2017 or 2019](https://visualstudio.microsoft.com/vs/) with the following workloads:
  - **ASP.NET and web development**
  - **.NET Core cross-platform development**
- [.NET Core SDK 2.2](https://www.microsoft.com/net/download/all)
- [DevExpress Unified Installer for .NET and HTML5 Developers](https://www.devexpress.com/Products/Try/)
  - We recommend that you select all  products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.
- To use the .Net Core version of the example, [install DevExpress \.NET Core 3 Desktop Products](https://documentation.devexpress.com/GeneralInformation/401278/Installation/Install-DevExpress-NET-Core-3-Desktop-Products).
- Build the [NonXAFSecurityExamples](NonXAFSecurityExamples.sln)/[NonXAFSecurityExamples.NetCore](NonXAFSecurityExamples.NetCore.sln) solution and 
run the [XafSolution.Win](/XafSolution/XafSolution.Win/XafSolution.Win.csproj)/[XafSolution.Win.NetCore](/XafSolution/XafSolution.Win/XafSolution.Win.NetCore.csproj) project to log in under 'User' or 'Admin' with an empty password. 
The application will generate a database with business objects from the [XafSolution.Module](XafSolution/XafSolution.Module/XafSolution.Module.csproj)/[XafSolution.Module.NetCore](XafSolution/XafSolution.Module/XafSolution.Module.NetCore.csproj) project.
- Add the [XafSolution.Module](/XafSolution/XafSolution.Module/XafSolution.Module.csproj)/[XafSolution.Module.NetCore](/XafSolution/XafSolution.Module/XafSolution.Module.NetCore.csproj) assembly reference to your application.

***

## Step 1: Configure the ASP.NET Core Server App
For detailed information about ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-2.2&tabs=windows).
- Configure the OData and MVC pipelines in the `ConfigureServices` and `Configure` methods of [Startup.cs](Startup.cs):

	``` csharp
	public void ConfigureServices(IServiceCollection services) {
		services.AddOData();
		services.AddMvc(options => {
			options.EnableEndpointRouting = false;
		}).SetCompatibilityVersion(CompatibilityVersion.Latest);
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
		
- Define the EDM model that contains data description for all used entities.

	``` csharp
	private IEdmModel GetEdmModel() {
		ODataModelBuilder builder = new ODataConventionModelBuilder();
		EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
		EntitySetConfiguration<Department> departments = builder.EntitySet<Department>("Departments");
		EntitySetConfiguration<Party> parties = builder.EntitySet<Party>("Parties");
		EntitySetConfiguration<ObjectPermissions> objectPermissions = builder.EntitySet<ObjectPermissions>("ObjectPermissions");
		EntitySetConfiguration<MemberPermissions> memberPermissions = builder.EntitySet<MemberPermissions>("MemberPermissions");
		EntitySetConfiguration<TypePermission> typePermissions = builder.EntitySet<TypePermission>("TypePermissions");

		employees.EntityType.HasKey(t => t.Oid);
		departments.EntityType.HasKey(t => t.Oid);
		parties.EntityType.HasKey(t => t.Oid);
		return builder.GetEdmModel();
	}
	```

	The [MemberPermission](Helpers/MemberPermission.cs), [ObjectPermission](Helpers/ObjectPermission.cs) and [TypePermission](Helpers/TypePermission.cs) classes are used as containers to transfer permissions to the client side.

	```csharp
	public class MemberPermission {
		[Key]
		public Guid Key { get; set; }
		public bool Read { get; set; }
		public bool Write { get; set; }
		public MemberPermission() {
			Key = Guid.NewGuid();
		}
	}
	//...
	public class ObjectPermission {
		public IDictionary<string, object> Data { get; set; }
		[Key]
		public string Key { get; set; }
		public bool Write { get; set; }
		public bool Delete { get; set; }
		public ObjectPermission() {
			Data = new Dictionary<string, object>();
		}
	}
	//...
	public class TypePermission {
		public IDictionary<string, object> Data { get; set; }
		[Key]
		public string Key { get; set; }
		public bool Create { get; set; }
		public TypePermission() {
			Data = new Dictionary<string, object>();
		}
	}
	```

- Enable the authentication service and configure the request pipeline with the authentication middleware in the `ConfigureServices` and `Configure` methods. 
[UnauthorizedRedirectMiddleware](UnauthorizedRedirectMiddleware.cs) сhecks if the ASP.NET Core Identity is authenticated. If not, it redirects a user to the authentication page.

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
		// ...
		public async Task InvokeAsync(HttpContext context) {
			if(context.User != null && context.User.Identity != null && context.User.Identity.IsAuthenticated
				|| IsAllowAnonymous(context)) {
				await _next(context);
			}
			else {
				context.Response.Redirect(authenticationPagePath);
			}
		}
		// ...
	}
	```

## Step 2: Initialize Data Store and XAF's Security System

The [SecurityProvider](Helpers/SecurityProvider.cs) class contains helper functions that provide access to XAF Security System functionality.

``` csharp
public class SecurityProvider : IDisposable {
	public SecurityStrategyComplex Security { get; private set; }
	public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
	XpoDataStoreProviderService xpoDataStoreProviderService;
	IConfiguration config;
	IHttpContextAccessor contextAccessor;
	public SecurityProvider(XpoDataStoreProviderService xpoDataStoreProviderService, IConfiguration config, IHttpContextAccessor contextAccessor) {
		this.xpoDataStoreProviderService = xpoDataStoreProviderService;
		this.config = config;
		this.contextAccessor = contextAccessor;
		if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
			Initialize();
		}
	}
	public void Initialize() {
		Security = GetSecurity(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
		ObjectSpaceProvider = GetObjectSpaceProvider(Security);
		Login(Security, ObjectSpaceProvider);
	}
}
```

- Add the `Configuration` property to [Startup.cs](Startup.cs) and register it as a singleton to have access to connectionString from controllers. 
The `IConfiguration` object is used to access the application configuration [appsettings.json](appsettings.json) file.

	``` csharp		
	//...
	public IConfiguration Configuration { get; }
	public Startup(IConfiguration configuration) {
		Configuration = configuration;
	}
	```	
	In appsettings.json, replace "DBSERVER" with the Database Server name or its IP address. Use "**localhost**" or "**(local)**" if you use a local Database Server.

	``` json
	"ConnectionStrings": {
	  "XafApplication": "Data Source=DBSERVER;Initial Catalog=XafSolution;Integrated Security=True"
	}
	```

- Register `SecurityProvider`, `Configuration` and `HttpContextAccessor` in the `ConfigureServices` method. `HttpContextAccessor` allows to access [HttpContext](https://docs.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8) in controller constructors.

	``` csharp
	public void ConfigureServices(IServiceCollection services) {
		// ...
		services.AddSingleton(Configuration);
		services.AddHttpContextAccessor();
		services.AddScoped<SecurityProvider>();
	}
	```

- The `GetSecurity` method provides access to the Security System instance and registers authentication providers. The `AuthenticationMixed` class allows you to register several authentication providers, 
so you can use both [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and ASP.NET Core Identity authentication.

	``` csharp
	public SecurityStrategyComplex GetSecurity(string authenticationName, object parameter) {
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

- The `GetObjectSpaceProvider` method provides access to the Object Space Provider. The [XpoDataStoreProviderService](Helpers/XpoDataStoreProviderService.cs) class provides access to the Data Store Provider object.

	``` csharp
	private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
		string connectionString = config.GetConnectionString("XafApplication");
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
	
- The `InitConnection` method authenticates a user both in the Security System and in [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2). 
A user is identified by the user name and password parameters.

	``` csharp
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
> Make sure that the static [EnableRfc2898 and SupportLegacySha512 properties](https://docs.devexpress.com/eXpressAppFramework/112649/Concepts/Security-System/Passwords-in-the-Security-System) in your non-XAF application have same values as in the XAF application where passwords were set. Otherwise you won't be able to login.

## Step 3: Implement OData Controllers for CRUD, Login, Logoff, etc.

- [EmployeesController](Controllers/EmployeesController.cs) has methods to implement CRUD logic with Employee objects. The `Get` method allows access to Employee objects.

	``` csharp
	public class EmployeesController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		IObjectSpace objectSpace;
		public EmployeesController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
			objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
                   // The XPO way:
                   // var session = ((SecuredObjectSpace)ObjectSpace).Session;
                   // 
                   // The XAF way:
			IQueryable<Employee> employees = ((XPQuery<Employee>)objectSpace.GetObjectsQuery<Employee>()).AsWrappedQuery();
			return Ok(employees);
		}
	}
	```

	The `Delete` method allows deleting Employee objects.

	``` csharp
	[HttpDelete]
	public ActionResult Delete(Guid key) {
		Employee existing = objectSpace.GetObjectByKey<Employee>(key);
		if(existing != null) {
			objectSpace.Delete(existing);
			objectSpace.CommitChanges();
			return NoContent();
		}
		return NotFound(); 
	}
	```

	The `Patch` method allows updating Employee objects.

	``` csharp
	[HttpPatch]
	public ActionResult Patch(Guid key, [FromBody]JObject jObject) {
		Employee employee = ObjectSpace.FindObject<Employee>(new BinaryOperator(nameof(Employee.Oid), key));
		if(employee != null) {
			JsonParser.ParseJObject<Employee>(jObject, employee, ObjectSpace);
			return Ok(employee);
		}
		return NotFound();
	}
	```

	The `Post` method allows creating Employee objects.

	``` csharp
	[HttpPost]
	public ActionResult Post([FromBody]JObject jObject) {
		Employee employee = ObjectSpace.CreateObject<Employee>();
		JsonParser.ParseJObject<Employee>(jObject, employee, ObjectSpace);
		return Ok(employee);
	}
	```
	
	[JsonParser](Helpers/JsonParser.cs) is a helper class to obtain business object properties values from the `JObject` object.

- [DepartmentsController](Controllers/DepartmentsController.cs) has methods to get access to Department objects.
 
	``` csharp
	public class DepartmentsController : ODataController, IDisposable {
		SecurityProvider securityProvider;
		IObjectSpace objectSpace;
		public DepartmentsController(SecurityProvider securityProvider) {
			this.securityProvider = securityProvider;
			objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get() {
			IQueryable<Department> departments = ((XPQuery<Department>)objectSpace.GetObjectsQuery<Department>()).AsWrappedQuery();
			return Ok(departments);
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get(Guid key) {
			Department department = ObjectSpace.GetObjectByKey<Department>(key);
			return department != null ? Ok(department) : (ActionResult)NoContent();
		}
	}
	```

- [AccountController](Controllers/AccountController.cs) handles the Login and Logout operations.
The `Login` method is called when a user clicks the `Login` button on the login page. The `Logoff` method is called when a user clicks the `Logoff` button on the main page.

	``` csharp
	// Attempts to log users with accepted credentials into the Security System.
	[HttpPost]
	[ODataRoute("Login")]
	[AllowAnonymous]
	public ActionResult Login(string userName, string password) {
		ActionResult result;
		string connectionString = Config.GetConnectionString("XafApplication");
		if(securityProvider.InitConnection(userName, password)) {
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
		
- [ActionsController](Controllers/ActionsController.cs) contains additional methods that process permissions.

	The `GetPermissions` method gathers permissions for all objects on the DevExtreme Data Grid current page and sends them to the client side as part of the response.
	``` csharp
	[HttpPost]
	[ODataRoute("GetPermissions")]
	public ActionResult GetPermissions(ODataActionParameters parameters) {
		ActionResult result = NoContent();
		if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
			List<string> keys = new List<string>(parameters["keys"] as IEnumerable<string>);
			string typeName = parameters["typeName"].ToString();
			using(IObjectSpace objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace()) {
				ITypeInfo typeInfo = objectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
				if(typeInfo != null) {
					IList entityList = objectSpace.GetObjects(typeInfo.Type, new InOperator(typeInfo.KeyMember.Name, keys));
					List<ObjectPermission> objectPermissions = new List<ObjectPermission>();
					foreach(object entity in entityList) {
						ObjectPermission objectPermission = CreateObjectPermission(typeInfo, entity);
						objectPermissions.Add(objectPermission);
					}
					result = Ok(objectPermissions);
				}
			}
		}
		return result;
	}
	// Creates a new ObjectPermission object, which contains a business object key, write and delete operation permissions, and object-level member permissions for each persistent member.
	private ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
		Type type = typeInfo.Type;
		ObjectPermission objectPermission = new ObjectPermission();
		objectPermission.Key = typeInfo.KeyMember.GetValue(entity).ToString();
		objectPermission.Write = securityProvider.Security.CanWrite(entity);
		objectPermission.Delete = securityProvider.Security.CanDelete(entity);
		IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
		foreach(IMemberInfo member in members) {
			MemberPermission memberPermission = CreateMemberPermission(entity, type, member);
			objectPermission.Data.Add(member.Name, memberPermission);
		}
		return objectPermission;
	}
	// Creates a new MemberPermission object, which contains read and write operation permissions for a specific member.
	private MemberPermission CreateMemberPermission(object entity, Type type, IMemberInfo member) {
		return new MemberPermission {
			Read = securityProvider.Security.CanRead(entity, member.Name),
			Write = securityProvider.Security.CanWrite(entity, member.Name)
		};
	}
	// Returns only visible persistent members which are displayed in the grid.
	private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
		return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
	}
	```
	
	The `GetTypePermissions` method gathers permissions for the type on the DevExtreme Data Grid's current page and sends them to the client side as part of the response.
	
	```csharp
	[HttpGet]
	[ODataRoute("GetTypePermissions")]
	public ActionResult GetTypePermissions(string typeName) {
		ActionResult result = NoContent();
		using(IObjectSpace objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace()) {
			ITypeInfo typeInfo = objectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
			if(typeInfo != null) {
				TypePermission typePermission = CreateTypePermission(typeInfo);
				result = Ok(typePermission);
			}
		}
		return result;
	}
	// Creates a new TypePermission object which contains the type name, create operation permission and type-level member permissions for each persistent member.
	private TypePermission CreateTypePermission(ITypeInfo typeInfo) {
		Type type = typeInfo.Type;
		TypePermission typePermission = new TypePermission();
		typePermission.Key = type.Name;
		typePermission.Create = securityProvider.Security.CanCreate(type);
		IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
		foreach(IMemberInfo member in members) {
			bool writePermission = securityProvider.Security.CanWrite(type, member.Name);
			typePermission.Data.Add(member.Name, writePermission);
		}
		return typePermission;
	}
	```
	
## Step 4: Implement the Client-Side App
- The authentication page ([Authentication.html](wwwroot/Authentication.html)) and the main page([Index.html](wwwroot/Index.html)) represent the client side UI.
- [authentication_code.js](wwwroot/js/authentication_code.js) gathers data from the login page and attempts to log the user in.

	``` javascript
	$("#validateAndSubmit").dxButton({
		text: "Log In",
		tabIndex: 1,
		useSubmitBehavior: true
	});

	$("#form").on("submit", function (e) {
		var userName = $("#userName").dxTextBox("instance").option("value");
		var password = $("#password").dxTextBox("instance").option("value");
		$.ajax({
			method: 'POST',
			url: 'Login',
			data: {
				"userName": userName,
				"password": password
			},
			complete: function (e) {
				if (e.status === 200) {
					document.cookie = "userName=" + userName;
					document.location.href = "/";
				}
				if (e.status === 401) {
					alert("User name or password is incorrect");
				}
			}
		});

		e.preventDefault();
	});
	```	
		
- [index_code.js](wwwroot/js/index_code.js) configures the DevExtreme Data Grid and logs the user out. 
The [onLoaded](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/ODataStore/Configuration/#onLoaded) function sends a request to the server to obtain permissions for the current data grid page.

	``` javascript
	function onLoaded(data) {
		var oids = $.map(data, function (val) {
			return val.Oid._value;
		});
		var parameters = {
			keys: oids,
			typeName: 'Employee'
		};
		var options = {
			dataType: "json",
			contentType: "application/json",
			type: "POST",
			async: false,
			data: JSON.stringify(parameters)
		};
		$.ajax("GetPermissions", options)
			.done(function (e) {
				permissions = e.value;
			});
	}
	```	

- The `onInitialized` function handles the data grid's [initialized](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#initialized) event 
and checks create operation permission to define whether the Create action should be displayed or not.

	``` javascript
	function onInitialized(e) {
		$.ajax({
			method: 'GET',
			url: 'GetTypePermissions?typeName=Employee',
			async: false,
			complete: function (data) {
				typePermissions = data.responseJSON;
			}
		});
		var grid = e.component;
		grid.option("editing.allowAdding", typePermissions.create);
	}
	```	

- The `onEditorPreparing` function handles the data grid's [editorPreparing](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#editorPreparing) event and checks Read and Write operation permissions. 
If the Read operation permission is denied, it displays the "Protected Content" placeholder and disables the editor. If the Write operation permission is denied, the editor is disabled.

	``` javascript
	function onEditorPreparing(e) {
		if (e.parentType === "dataRow") {
			var dataField = e.dataField.split('.')[0];
			var key = e.row.key._value;
			if (key != undefined) {
				var objectPermission = getPermission(key);
				if (!objectPermission[dataField].read) {
					e.editorOptions.disabled = true;
					e.editorOptions.value = "Protected Content";
				}
				if (!objectPermission[dataField].write) {
					e.editorOptions.disabled = true;
				}
			}
			else {
				if (!typePermissions[dataField]) {
					e.editorOptions.disabled = true;
				}
			}
		}
	}
	```	

- The `onCellPrepared` function handles the data grid's [cellPrepared](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#cellPrepared) event and checks Read, Write, and Delete operation permissions. 
If the Read permission is denied, it displays the "Protected Content" placeholder in data grid cells. Write and Delete operation permission checks define whether the Write and Delete actions should be displayed or not.

	``` javascript
	function onCellPrepared(e) {
		if (e.rowType === "data") {
			var key = e.key._value;
			var objectPermission = getPermission(key);
			if (!e.column.command) {
				var dataField = e.column.dataField.split('.')[0];
				if (!objectPermission[dataField].read) {
					e.cellElement.text("Protected Content");
				}
			}
			else if (e.column.command == 'edit') {
				if (!objectPermission.Delete) {
					e.cellElement.find(".dx-link-delete").remove();
				}
				if (!objectPermission.Write) {
					e.cellElement.find(".dx-link-edit").remove();
				}
			}
		}
	}
	```	
	
Note that SecuredObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI. 
Use the [SecurityStrategy.IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) method to determine 
when to mask default values with the "Protected Content" placeholder in the UI.

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

 - Press the Logout button and log in under 'Admin' to see all records.
