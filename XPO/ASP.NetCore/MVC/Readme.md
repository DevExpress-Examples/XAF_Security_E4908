This example demonstrates how to protect your data with the [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the ASP.NET Core MVC web application with the DevExtreme UI.
 
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

## Step 1: Configure the ASP.NET Core Server App Services

1. Create a new ASP.NET Core application or use an existing application.
2. Add DevExpress NuGet packages to your project:

    ```xml
    <PackageReference Include="DevExpress.Persistent.BaseImpl.Xpo" Version="21.2.4" />
    <PackageReference Include="DevExpress.ExpressApp.Security.Xpo" Version="21.2.4" />
    <PackageReference Include="DevExtreme.AspNet.Core" Version="21.2.4" />
    ```
3. [Configure](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-6.0&tabs=windows) the MVC pipelines in the [Program.cs](Program.cs):
    
    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    string loginPath = "/Authentication";
    JsonResolver resolver = new JsonResolver();
    Action<MvcNewtonsoftJsonOptions> JsonOptions =
    options => {
        options.SerializerSettings.ContractResolver = resolver;
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    };
    builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(JsonOptions);
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(options => {
         options.LoginPath = loginPath;
     });
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
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    app.UseAuthentication();
    app.UseDefaultFiles();
    app.UseHttpsRedirection();
    app.UseStaticFiles(new StaticFileOptions() {
        OnPrepareResponse = context => {
            if (context.Context.User.Identity.IsAuthenticated) {
                return;
            }
            else {
                string referer = context.Context.Request.Headers["Referer"].ToString();
                string authenticationPagePath = loginPath;
                string vendorString = "vendor.css";
                if (context.Context.Request.Path.HasValue && context.Context.Request.Path.StartsWithSegments(authenticationPagePath)
                    || referer != null && (referer.Contains(authenticationPagePath) || referer.Contains(vendorString))) {
                    return;
                }
                context.Context.Response.Redirect(loginPath);
            }
        }
    });
    app.UseCookiePolicy();
    app.UseRouting();
    app.UseAuthorization();
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    app.UseDemoData();
    app.Run();
    ```
    
4. The [JsonResolver](Helpers/JsonResolver.cs) is needed to serialize business objects correctly.
        
    ```csharp
    public class JsonResolver : Newtonsoft.Json.Serialization.DefaultContractResolver {
        public bool SerializeCollections { get; set; } = false;
        public bool SerializeReferences { get; set; } = true;
        public bool SerializeByteArrays { get; set; } = true;
        readonly XPDictionary dictionary;
        public JsonResolver() {
            dictionary = new ReflectionDictionary();
            dictionary.GetDataStoreSchema(typeof(Employee), typeof(Department));
        }
        protected override List<MemberInfo> GetSerializableMembers(Type objectType) {
            XPClassInfo classInfo = dictionary.QueryClassInfo(objectType);
            if(classInfo != null && classInfo.IsPersistent) {
                var allSerializableMembers = base.GetSerializableMembers(objectType);
                var serializableMembers = new List<MemberInfo>(allSerializableMembers.Count);
                foreach(MemberInfo member in allSerializableMembers) {
                    XPMemberInfo mi = classInfo.FindMember(member.Name);
                    if(!(mi.IsPersistent || mi.IsAliased || mi.IsCollection || mi.IsManyToManyAlias)
                        || ((mi.IsCollection || mi.IsManyToManyAlias) && !SerializeCollections)
                        || (mi.ReferenceType != null && !SerializeReferences)
                        || (mi.MemberType == typeof(byte[]) && !SerializeByteArrays)) {
                        continue;
                    }
                    serializableMembers.Add(member);
                }
                return serializableMembers;
            }
            return base.GetSerializableMembers(objectType);
        }
    }
    ```

5. The `IXpoDataStoreProvider` provides access to the Data Store Provider object.
    
6. The `IConfiguration` object is used to access the application configuration [appsettings.json](appsettings.json) file. In _appsettings.json_, add the connection string.
    
    ``` json
    "ConnectionStrings": {
        "ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=XPOTestDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
    }
    ```
        
7. Register HttpContextAccessor in the [Program.cs](Program.cs) to access [HttpContext](https://docs.microsoft.com/en-us/dotnet/api/system.web.httpcontext?view=netframework-4.8) in controller constructors.

	```csharp
	builder.Services.AddHttpContextAccessor();
	```

8. Set the [StaticFileOptions.OnPrepareResponse](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.builder.staticfileoptions.onprepareresponse?view=aspnetcore-3.0#Microsoft_AspNetCore_Builder_StaticFileOptions_OnPrepareResponse) property
with the logic which сhecks if the ASP.NET Core Identity is authenticated. And, if not, it redirects a user to the authentication page.

9. In the [Program.cs](Program.cs) file, register the `TypesInfo` service required for the correct operation of the Security System.

	```csharp
	builder.Services.AddSingleton<ITypesInfo, TypesInfo>();
	```

10. Register the business objects that you will access from your code in the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) system.

    ```csharp
    typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
    typesInfo.RegisterEntity(typeof(Employee));
    typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
    typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
    ```

11. Call the `UseDemoData` method at the end of the [Program.cs](Program.cs):
    
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

## Step 2: Initialize Data Store and XAF's Security System. Authentication and Permission Configuration

1. Register security system and authentication in [Program.cs](Program.cs). We register it as a scoped to have access to SecurityStrategyComplex from SecurityProvider. The `AuthenticationMixed` class allows you to register multiple authentication providers, so you can use both [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication) and ASP.NET Core Identity authentication.

    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    //...
    builder.Services.AddScoped((serviceProvider) => {
        AuthenticationMixed authentication = new AuthenticationMixed();
        authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
        authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
        authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
        ITypesInfo typesInfo = serviceProvider.GetRequiredService<ITypesInfo>();
        SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
        security.RegisterXPOAdapterProviders();
        return security;
    });
    ```

2. Create [MemberPermission](Helpers/MemberPermission.cs), [ObjectPermission](Helpers/ObjectPermission.cs) and [TypePermission](Helpers/TypePermission.cs) classes. These classes are used as containers to transfer permissions to the client side.

    ```csharp
    public class MemberPermission {
        public bool Read { get; set; }
        public bool Write { get; set; }
    }
    //...
    public class ObjectPermission {
        public IDictionary<string, object> Data { get; set; }
        public string Key { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
        public ObjectPermission() {
            Data = new Dictionary<string, object>();
        }
    }
    //...
    public class TypePermission {
        public IDictionary<string, bool> Data { get; set; }
        public bool Create { get; set; }
        public TypePermission() {
            Data = new Dictionary<string, bool>();
        }
    }
    ```

3. The [SecurityProvider](Helpers/SecurityProvider.cs) class provides access to the XAF Security System functionality.

    ```csharp
    public class SecurityProvider : IDisposable {
        public SecurityStrategyComplex Security { get; private set; }
        public IObjectSpaceProvider ObjectSpaceProvider { get; private set; }
        private IXpoDataStoreProvider xpoDataStoreProvider;
        private IHttpContextAccessor contextAccessor;
        public SecurityProvider(SecurityStrategyComplex security, IXpoDataStoreProvider xpoDataStoreProvider, IHttpContextAccessor contextAccessor) {
            Security = security;
            this.xpoDataStoreProvider = xpoDataStoreProvider;
            this.contextAccessor = contextAccessor;
            if(contextAccessor.HttpContext.User.Identity.IsAuthenticated) {
                Initialize();
            }
        }
        //...
    }
    ```
4. Register `SecurityProvider`, in the [Program.cs](Program.cs).

    ```csharp
    var builder = WebApplication.CreateBuilder(args);
    //...
    builder.Services.AddScoped<SecurityProvider>();
    ```

5. The `Initialize` method initializes `ObjectSpaceProvider` properties and performs [Login](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.Logon(System.Object)) to Security System.

    ```csharp
    public void Initialize() {
        ((AuthenticationMixed)Security.Authentication).SetupAuthenticationProvider(typeof(IdentityAuthenticationProvider).Name, contextAccessor.HttpContext.User.Identity);
        ObjectSpaceProvider = GetObjectSpaceProvider(Security);
        Login(Security, ObjectSpaceProvider);
    }    
    private void Login(SecurityStrategyComplex security, IObjectSpaceProvider objectSpaceProvider) {
        IObjectSpace objectSpace = objectSpaceProvider.CreateObjectSpace();
        security.Logon(objectSpace);
    }
    ```

6. The `GetObjectSpaceProvider` method initializes the Object Space Provider.

    ```csharp
    private IObjectSpaceProvider GetObjectSpaceProvider(SecurityStrategyComplex security) {
		SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, xpoDataStoreProvider, security.TypesInfo, null);
		return objectSpaceProvider;
	}
    ```
    
7. The `InitConnection` method authenticates a user both in the Security System and in [ASP.NET Core HttpContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.http.httpcontext?view=aspnetcore-2.2). 
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

## Step 3: Server-Side App Authentication and Authorization with MVC Controllers for Login, Logout and CRUD

1. All controllers get SecurityProvider as constructor parameters to have access to objectSpace and security.

```csharp
public class EmployeesController : Microsoft.AspNetCore.Mvc.Controller {
    SecurityProvider securityProvider;
    IObjectSpace objectSpace;
    public EmployeesController(SecurityProvider securityProvider) {
        this.securityProvider = securityProvider;
        objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
    }
    //...
}
```

2. [EmployeesController](Controllers/EmployeesController.cs) has methods to implement CRUD logic with Employee objects. 

    The `Get` method to get access to Employee objects. The [DataSourceLoader](https://devexpress.github.io/DevExtreme.AspNet.Data/net/api/DevExtreme.AspNet.Data.DataSourceLoader.html) class provides methods to load data from object collections.
    
    ```csharp
    [HttpGet]
    public object Get(DataSourceLoadOptions loadOptions) {
        IQueryable<Employee> employees = objectSpace.GetObjectsQuery<Employee>();
        return DataSourceLoader.Load(employees, loadOptions);
    }
    ```
     
    The `Delete` method deletes the Employee object by key.
    
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

    The `Update` method updates a specific Employee object with specified values.
    
    ```csharp
    [HttpPut]
    public ActionResult Update(Guid key, string values) {
        Employee employee = objectSpace.GetObjectByKey<Employee>(key);
        if(employee != null) {
            JsonParser.ParseJObject<Employee>(JObject.Parse(values), employee, objectSpace);
            return Ok(employee);
        }
        return NotFound();
    }
    ```
    
    The `Add` method creates new Employee object and saves it with specified values.
    
    ```csharp
    [HttpPost]
    public ActionResult Add(string values) {
        Employee employee = objectSpace.CreateObject<Employee>();
        JsonParser.ParseJObject<Employee>(JObject.Parse(values), employee, objectSpace);
        return Ok(employee);
    }
    ```
    
    [JsonParser](Helpers/JsonParser.cs) is a helper class to obtain business object properties values from the `JObject` object.
    
    > Note that secured XPObjectSpace returns default values (for instance, null) for protected object properties - it is secure even without any custom UI.

3. [DepartmentsController](Controllers/DepartmentsController.cs) has methods to get access to Department objects and contains code similar to the one in EmployeesController.
    
4. [AuthenticationController](Controllers/AuthenticationController.cs) handles the Log in and Log out operations.
    
    The `Authentication` methods return the [Authentication](Views/Authentication/Authentication.cshtml) view.
    The `Logout` method is called when a user clicks the `Log out` button on the [main page](Views/Home/Index.cshtml).
    
    ```csharp
    public class AuthenticationController : Controller {
        SecurityProvider securityProvider;
        public AuthenticationController(SecurityProvider securityProvider){
            this.securityProvider = securityProvider;
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password) {
            ActionResult result;
            if(securityProvider.InitConnection(userName, password)) {
                result = Ok();
            }
            else {
                result = Unauthorized();
            }
            return result;
        }
        [HttpGet]
        [Route("Logout")]
        public async Task<ActionResult> Logout() {
            await HttpContext.SignOutAsync();
            return Ok();
        }
        [Route("Authentication")]
        public IActionResult Authentication() {
            return View();
        }
        protected override void Dispose(bool disposing) {
            if(disposing) {
                securityProvider?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
    ```

5. [ActionsController](Controllers/ActionsController.cs) contains the `GetPermissions` method to process permissions.
    
    ```csharp
    [HttpPost]
    public ActionResult GetPermissions(List<Guid> keys, string typeName) {
        ActionResult result = NoContent();
        using(IObjectSpace objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace()) {
            PermissionHelper permissionHelper = new PermissionHelper(securityProvider.Security);
            ITypeInfo typeInfo = objectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
            if(typeInfo != null) {
                IList entityList = objectSpace.GetObjects(typeInfo.Type, new InOperator(typeInfo.KeyMember.Name, keys));
                List<ObjectPermission> objectPermissions = new List<ObjectPermission>();
                foreach(object entity in entityList) {
                    ObjectPermission objectPermission = permissionHelper.CreateObjectPermission(typeInfo, entity);
                    objectPermissions.Add(objectPermission);
                }
                result = Ok(objectPermissions);
            }
        }
        return result;
    }
    ```        
    
6. [HomeController](Controllers/HomeController.cs) sends to client-side the [Index](Views/Home/Index.cshtml) view and type permissions.
    
    ```csharp
    public IActionResult Index() {
        using(IObjectSpace objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace()) {
            ITypeInfo typeInfo = objectSpace.TypesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeof(Employee).Name);
            PermissionHelper permissionHelper = new PermissionHelper(securityProvider.Security);
            TypePermission typePermission = permissionHelper.CreateTypePermission(typeInfo);
            return View(typePermission);
        }
    }
    ```
    
7. [PermissionHelper](Helpers/PermissionHelper.cs) is a helper class which provides methods to create permissions.
        
    Use the SecurityStrategy.CanCreate and SecurityStrategy.CanWrite methods    
    to check if the user is allowed to perform a specific operation.
    
    The `CreateTypePermission` creates the `TypePermission` object, which contains create operation permissions for the specified type and write operation permissions for all members of this type obtained from the `GetPersistentMembers` method.
    
    ```csharp
    public TypePermission CreateTypePermission(ITypeInfo typeInfo) {
        Type type = typeInfo.Type;
        TypePermission typePermission = new TypePermission();
        typePermission.Create = Security.CanCreate(type);
        IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
        foreach(IMemberInfo member in members) {
            bool writePermission = Security.CanWrite(type, member.Name);
            typePermission.Data.Add(member.Name, writePermission);
        }
        return typePermission;
    }
    private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
        return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
    }
    ```
    
    The `CreateObjectPermission` method creates the `ObjectPermission` object, which contains write and delete operation permissions for the specified object and the `MemberPermission` objects 
    for all members of this type obtained from the `GetPersistentMembers` method.
    
    ```csharp
    public ObjectPermission CreateObjectPermission(ITypeInfo typeInfo, object entity) {
        ObjectPermission objectPermission = new ObjectPermission();
        objectPermission.Key = typeInfo.KeyMember.GetValue(entity).ToString();
        objectPermission.Write = Security.CanWrite(entity);
        objectPermission.Delete = Security.CanDelete(entity);
        IEnumerable<IMemberInfo> members = GetPersistentMembers(typeInfo);
        foreach(IMemberInfo member in members) {
            MemberPermission memberPermission = CreateMemberPermission(entity, member);
            objectPermission.Data.Add(member.Name, memberPermission);
        }
        return objectPermission;
    }
    ```
    
    The `CreateMemberPermission` method creates the `MemberPermission` object, which contains read and write operation permissions for the specified member.
    
    ```csharp
    public MemberPermission CreateMemberPermission(object entity, IMemberInfo member) {
        return new MemberPermission {
            Read = Security.CanRead(entity, member.Name),
            Write = Security.CanWrite(entity, member.Name)
        };
    }
    ```    
    
## Step 4: Client-Side App Authentication and Authorization to Customize UI Based on Access Rights

[Authentication.cshtml](Views/Authentication/Authentication.cshtml) is a login page that allows you to log into the application and [AuthenticationCode.js](wwwroot/js/AuthenticationCode.js) contains scripts executed on the login page.

[Index.cshtml](Views/Home/Index.cshtml) is the main page. It configures the DevExtreme Data Grid and allows logging the user out.

[IndexCode.js](wwwroot/js/IndexCode.js) contains scripts executed on the main page:

- The `onInitialized` function initializes the TypePermission object in the DataGrid [initialized](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#initialized) event.
    
    ```javascript
    function onInitialized(e) {
        typePermissions = e;
    }
    ```
    
- The `onLoaded` function handles the data grid's [load](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/DataSource/Methods/#load) event and sends a request to the server to obtain permissions for the current data grid page.
    
    ```javascript
    function onLoaded(data) {
        var oids = $.map(data, function (val) {
            return val.Oid;
        });
        var parameters = {
            keys: oids,
            typeName: 'Employee'
        };
        var options = {
            dataType: "json",
            contentType: "application/x-www-form-urlencoded; charset=UTF-8",
            type: "POST",
            async: false,
            data: parameters
        };
        $.ajax("api/Actions/GetPermissions", options)
            .done(function (e) {
                permissions = e;
            });
    }
    ```

- The `onEditorPreparing` function handles the data grid's [editorPreparing](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#editorPreparing) event and checks Read and Write operation permissions. 
If the Read operation permission is denied, it displays the "*******" placeholder and disables the editor. If the Write operation permission is denied, the editor is disabled.

    ```javascript
    function onEditorPreparing(e) {
        if (e.parentType === "dataRow") {
            var dataField = e.dataField.split('.')[0];
            var key = e.row.key;
            if (e.row.isNewRow) {
                if (!typePermissions[dataField]) {
                    e.editorOptions.disabled = true;
                }
            }
            else {
                var objectPermission = getPermission(key);
                if (!objectPermission.Data[dataField].Read) {
                    e.editorOptions.disabled = true;
                    e.editorOptions.value = "*******";
                }
                if (!objectPermission.Data[dataField].Write) {
                    e.editorOptions.disabled = true;
                }
            }
        }
    }
    ```    
    
- The `onCellPrepared` function handles the data grid's [cellPrepared](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#cellPrepared) event and checks Read operation permissions. 
If the Read operation is denied, it displays the "*******" placeholder in data grid cells.

    ```javascript
    function onCellPrepared(e) {
        if (e.rowType === "data") {
            var key = e.key;
            var objectPermission = getPermission(key);
            if (!e.column.command) {
                var dataField = e.column.dataField.split('.')[0];
                if (!objectPermission.Data[dataField].Read) {
                    e.cellElement.text("*******");
                }
            }
        }
    }
    ```

- The `allowUpdating` function handles the data grid's [allowUpdating](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Configuration/editing/#allowUpdating) event and checks Write operation permissions.
Write operation permission checks define whether the Edit actions should be displayed or not.

    ```javascript
    function allowUpdating(e) {
        if (e.row.rowType === "data") {
            var objectPermission = getPermission(e.row.key);
            if (objectPermission.Write) {
                return true;
            }
        }
        return false;
    }
    ```

- The `allowDeleting` function handles the data grid's [allowDeleting](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Configuration/editing/#allowDeleting) event and checks Delete operation permissions.
The delete operation permission  defines whether the Delete actions should be displayed or not.

    ```javascript
    function allowDeleting(e) {
        if (e.row.rowType === "data") {
            var objectPermission = getPermission(e.row.key);
            if (objectPermission.Delete) {
                return true;
            }
        }
        return false;
    }
    ```    

- The `getPermission` function returns the permission object for a specific business object. The business object is identified by the key passed in function parameters:

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
   ![](/images/MVC_ListView.png)

 - Press the **Logout** button and log in as 'Admin' to see all the records.
