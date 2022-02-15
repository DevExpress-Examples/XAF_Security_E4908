This example demonstrates how to protect your data with the [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the following client-server web app:

- Server: an OData v7 service built with [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0).
- Client: an HTML/JavaScript app with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/).

## Prerequisites

- [Visual Studio 2022 v17.0+](https://visualstudio.microsoft.com/vs/) with the following workloads:
  - **ASP.NET and web development**
  - **.NET Core cross-platform development**
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet-core)
- [Download and run our Unified Component Installer](https://www.devexpress.com/Products/Try/) or add [NuGet feed URL](https://docs.devexpress.com/GeneralInformation/116042/installation/install-devexpress-controls-using-nuget-packages/obtain-your-nuget-feed-url) to Visual Studio NuGet feeds.
  
  *We recommend that you select all products when you run the DevExpress installer. It will register local NuGet package sources and item / project templates required for these tutorials. You can uninstall unnecessary components later.*


> **NOTE** 
>
> If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

## Step 1: Configure the ASP.NET Core Server App
For detailed information about ASP.NET Core application configuration, see [official Microsoft documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-6.0&tabs=windows).
- Configure the OData and MVC pipelines in the [Program.cs](Program.cs):

	```csharp
	var builder = WebApplication.CreateBuilder(args);
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddSingleton<IXpoDataStoreProvider>((serviceProvider) => {
		string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
		IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
		return dataStoreProvider;
	});

	var app = builder.Build();
	if (app.Environment.IsDevelopment()) {
		app.UseDeveloperExceptionPage();
	}
	else {
		app.UseHsts();
	}
	app.UseODataQueryRequest();
	app.UseODataBatching();
	app.UseRouting();
	app.UseCors();
	app.UseEndpoints(endpoints => {
		endpoints.MapControllers();
	});
	app.UseDemoData();
	app.Run();
	```
- The `IXpoDataStoreProvider` provides access to the Data Store Provider object.
		
- The `IConfiguration` object is used to access the application configuration [appsettings.json](appsettings.json) file. 
	In _appsettings.json_, add the connection string.
	``` json
	"ConnectionStrings": {
		"ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
	}
	```
		
- Register HttpContextAccessor in the [Program.cs](Program.cs) to access [HttpContext](https://docs.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8) in controller constructors.

	```csharp
	builder.Services.AddHttpContextAccessor();
	```

- Define the EDM model that contains data description for all used entities. We also need to define actions to log in/out a user and get the user permissions.

	```csharp
    IEdmModel GetEdmModel() {
        ODataModelBuilder builder = new ODataConventionModelBuilder();
        EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
        EntitySetConfiguration<Department> departments = builder.EntitySet<Department>("Departments");
        EntitySetConfiguration<Party> parties = builder.EntitySet<Party>("Parties");
        EntitySetConfiguration<ObjectPermission> objectPermissions = builder.EntitySet<ObjectPermission>("ObjectPermissions");
        EntitySetConfiguration<MemberPermission> memberPermissions = builder.EntitySet<MemberPermission>("MemberPermissions");
        EntitySetConfiguration<TypePermission> typePermissions = builder.EntitySet<TypePermission>("TypePermissions");

        employees.EntityType.HasKey(t => t.Oid);
        departments.EntityType.HasKey(t => t.Oid);
        parties.EntityType.HasKey(t => t.Oid);

        ActionConfiguration login = builder.Action("Login");
        login.Parameter<string>("userName");
        login.Parameter<string>("password");

        builder.Action("Logout");

        ActionConfiguration getPermissions = builder.Action("GetPermissions");
        getPermissions.Parameter<string>("typeName");
        getPermissions.CollectionParameter<string>("keys");

        ActionConfiguration getTypePermissions = builder.Action("GetTypePermissions");
        getTypePermissions.Parameter<string>("typeName");
        getTypePermissions.ReturnsFromEntitySet<TypePermission>("TypePermissions");
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

- Enable the authentication service and configure the request pipeline with the authentication middleware in the [Program.cs](Program.cs). 
[UnauthorizedRedirectMiddleware](UnauthorizedRedirectMiddleware.cs) сhecks if the ASP.NET Core Identity is authenticated. If not, it redirects a user to the authentication page.

	```csharp
	var builder = WebApplication.CreateBuilder(args);
	//...
	builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		.AddCookie();

	var app = builder.Build();
	//...
	app.UseAuthentication();
	app.UseMiddleware<UnauthorizedRedirectMiddleware>();
	app.UseDefaultFiles();
	app.UseStaticFiles();
	app.UseHttpsRedirection();
	app.UseCookiePolicy();

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

- In the [Program.cs](Program.cs) file, register the `TypesInfo` service required for the correct operation of the Security System.

	```csharp
	builder.Services.AddSingleton<ITypesInfo, TypesInfo>();
	```

- Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.

    ```csharp
    typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
    typesInfo.RegisterEntity(typeof(Employee));
    typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
    typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
    ```

- Call the UseDemoData method at the end of the [Program.cs](Program.cs).
	
	
	```csharp
	public static WebApplication UseDemoData(this WebApplication app) {
		IXpoDataStoreProvider xpoDataStoreProvider = app.Services.GetRequiredService<IXpoDataStoreProvider>();
		ITypesInfo typesInfo = app.Services.GetRequiredService<ITypesInfo>();
		using (var objectSpaceProvider = new XPObjectSpaceProvider(xpoDataStoreProvider, typesInfo, null)) {
			using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
				new Updater(objectSpace).UpdateDatabase();
			}
		}
		return app;
	}
    ```
    For more details about how to create demo data from code, see the [Updater.cs](/XPO/DatabaseUpdater/Updater.cs) class.

## Step 2: Initialize Data Store and XAF's Security System

Register security system and authentication in the [Program.cs](Program.cs). We register it as a scoped to have access to SecurityStrategyComplex from SecurityProvider. The `AuthenticationMixed` class allows you to register several authentication providers, 
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

The [SecurityProvider](Helpers/SecurityProvider.cs) class contains helper functions that provide access to XAF Security System functionality.

```csharp
public class SecurityProvider : IDisposable {
    public SecurityStrategyComplex Security { get; private set; }
    public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
    IXpoDataStoreProvider xpoDataStoreProvider;
    IHttpContextAccessor contextAccessor;
    public SecurityProvider(SecurityStrategyComplex security, IXpoDataStoreProvider xpoDataStoreProvider, IHttpContextAccessor contextAccessor) {
        Security = security;
        this.xpoDataStoreProvider = xpoDataStoreProvider;
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
		SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProvider, security.TypesInfo, null);
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

## Step 3: Implement OData Controllers for CRUD, Login, Logoff, etc.

- [EmployeesController](Controllers/EmployeesController.cs) has methods to implement CRUD logic with Employee objects. The `Get` method allows access to Employee objects.

	```csharp
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
			IQueryable<Employee> employees = ((XPQuery<Employee>)objectSpace.GetObjectsQuery<Employee>());
			return Ok(employees);
		}
	```

	The `Delete` method allows deleting Employee objects.

	```csharp
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

	```csharp
    [HttpPatch]
    public ActionResult Patch(Guid key, [FromBody] JsonElement jElement) {
        Employee employee = objectSpace.FirstOrDefault<Employee>(e => e.Oid == key);
        if(employee != null) {
            JsonParser.ParseJObject<Employee>(jElement, employee, objectSpace);
            return Ok(employee);
        }
        return NotFound();
    }
	```

	The `Post` method allows creating Employee objects.

	```csharp
    [HttpPost]
    public ActionResult Post([FromBody] JsonElement jElement) {
        Employee employee = objectSpace.CreateObject<Employee>();
        JsonParser.ParseJObject<Employee>(jElement, employee, objectSpace);
        return Ok(employee);
    }
	```
	
	[JsonParser](Helpers/JsonParser.cs) is a helper class to obtain business object properties values from the `JObject` object.

- [DepartmentsController](Controllers/DepartmentsController.cs) has methods to get access to Department objects.
 
	```csharp
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
			IQueryable<Department> departments = ((XPQuery<Department>)objectSpace.GetObjectsQuery<Department>());
			return Ok(departments);
		}
		[HttpGet]
		[EnableQuery]
		public ActionResult Get(Guid key) {
			Department department = objectSpace.GetObjectByKey<Department>(key);
			return department != null ? Ok(department) : (ActionResult)NoContent();
		}
		public void Dispose() {
			objectSpace?.Dispose();
			securityProvider?.Dispose();
		}
	}
	```

- [AccountController](Controllers/AccountController.cs) handles the Login and Logout operations.
The `Login` method is called when a user clicks the `Login` button on the login page. The `Logoff` method is called when a user clicks the `Logoff` button on the main page.

	```csharp
	// Attempts to log users with accepted credentials into the Security System.
    [HttpPost("Login")]
    [AllowAnonymous]
    public ActionResult Login(string userName, string password) {
        ActionResult result;
        if(securityProvider.InitConnection(userName, password)) {
            result = Ok();
        } else {
            result = Unauthorized();
        }
        return result;
    }
	// Signs the user out of the HttpContext.
    [HttpGet("Logout")]
    public ActionResult Logout() {
        HttpContext.SignOutAsync();
        return Ok();
    }
	```
		
- [ActionsController](Controllers/ActionsController.cs) contains additional methods that process permissions.

	The `GetPermissions` method gathers permissions for all objects on the DevExtreme Data Grid current page and sends them to the client side as part of the response.
	```csharp
    [HttpPost("/GetPermissions")]
    public ActionResult GetPermissions(ODataActionParameters parameters) {
        ActionResult result = NoContent();
        if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
            IEnumerable<Guid> keys = ((IEnumerable<string>)parameters["keys"]).Select(k => Guid.Parse(k));
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

	```javascript
	$("#userName").dxTextBox({
		name: "userName",
		placeholder: "User name",
		tabIndex: 2,
		onInitialized: function (e) {
			var texBoxInstance = e.component;
			var userName = getCookie("userName");
			if (userName === undefined) {
				userName = "User";
			}
			texBoxInstance.option("value", userName);
		},
		onEnterKey: pressEnter
	}).dxValidator({
		validationRules: [{
			type: "required",
			message: "The user name must not be empty"
		}]
	});

	$("#password").dxTextBox({
		name: "Password",
		placeholder: "Password",
		mode: "password",
		tabIndex: 3,
		onEnterKey: pressEnter
	});

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
					window.location = "Index.html";					
				}
				if (e.status === 401) {
					alert("User name or password is incorrect");
				}
			}
		});

		e.preventDefault();
	});

	function pressEnter(data) {
		$('#validateAndSubmit').click();
	}

	function getCookie(name) {
		let matches = document.cookie.match(new RegExp(
			"(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
		));
		return matches ? decodeURIComponent(matches[1]) : undefined;
	}
	```	
		
- [index_code.js](wwwroot/js/index_code.js) configures the DevExtreme Data Grid and logs the user out. 
The [onLoaded](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/ODataStore/Configuration/#onLoaded) function sends a request to the server to obtain permissions for the current data grid page.

	```javascript
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

	```javascript
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
		grid.option("editing.allowAdding", typePermissions.Create);
	}
	```	

- The `onEditorPreparing` function handles the data grid's [editorPreparing](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#editorPreparing) event and checks Read and Write operation permissions. 
If the Read operation permission is denied, it displays the "*******" placeholder and disables the editor. If the Write operation permission is denied, the editor is disabled.

	```javascript
	function onEditorPreparing(e) {
		if (e.parentType === "dataRow") {
			var dataField = e.dataField.split('.')[0];
			var key = e.row.key._value;
			if (key != undefined) {
				var objectPermission = getPermission(key);
				if (!objectPermission[dataField].Read) {
					e.editorOptions.disabled = true;
                    e.editorOptions.value = "*******";
				}
				if (!objectPermission[dataField].Write) {
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
If the Read permission is denied, it displays the "*******" placeholder in data grid cells. Write and Delete operation permission checks define whether the Write and Delete actions should be displayed or not.

	```javascript
	function onCellPrepared(e) {
		if (e.rowType === "data") {
			var key = e.key._value;
			var objectPermission = getPermission(key);
			if (!e.column.command) {
				var dataField = e.column.dataField.split('.')[0];
				if (!objectPermission[dataField].Read) {
					e.cellElement.text("*******");
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
when to mask default values with the "*******" placeholder in the UI.

- The `getPermission` function returns the permission object for a business object. The business object is identified by the key passed in function parameters:

	```javascript
	function getPermission(key) {
		var permission = permissions.filter(function (entry) {
			return entry.Key === key;
		});
		return permission[0];
	}
	```

## Step 5: Run and Test the App
 - Log in a 'User' with an empty password.

   ![](/images/ODataLoginPage.png)

 - Notice that secured data is displayed as '*******'.
   ![](/images/ODataListView.png)

 - Press the **Logout** button and log in as 'Admin' to see all the records.
