> # Under construction for 22.1 release
This example demonstrates how to expose your data with [XAF Web API]() and protect it with [XAF Security System](https://docs.devexpress.com/eXpressAppFramework/113366/Concepts/Security-System/Security-System-Overview) in the following client-server web app:

- Server: an OData v7 service built with [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-5.0).
- Client: an HTML/JavaScript app with the [DevExtreme Data Grid](https://js.devexpress.com/Overview/DataGrid/).

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

## Step 1: Configure the ASP.NET Core Server App
1. Add EFCore DevExpress NuGet packages to your project:

    ```xml
    <PackageReference Include="DevExpress.ExpressApp.EFCore" Version="21.2.4" />
    <PackageReference Include="DevExpress.Persistent.BaseImpl.EFCore" Version="21.2.4" />
    ```
2. Install Entity Framework Core, as described in the [Installing Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/get-started/overview/install) article.

3. [Configure](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/?view=aspnetcore-6.0&tabs=windows) the OData and MVC pipelines in the [Program.cs](Program.cs):

    ```csharp
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
        .AddControllers(mvcOptions => {
            mvcOptions.EnableEndpointRouting = false;
        })
        .AddOData((opt, services) => opt
            .Count()
            .Filter()
            .Expand()
            .Select()
            .OrderBy()
            .SetMaxTop(null)
            .AddRouteComponents(GetEdmModel())
            .AddRouteComponents("api/odata", new EdmModelBuilder(services).GetEdmModel())
        );
        
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
    app.Run();
    ```

- Define the EDM model that contains data description for all used entities. We also need to define actions to log in/out a user and get the user permissions.

    ```csharp
    IEdmModel GetEdmModel() {
        ODataModelBuilder builder = new ODataConventionModelBuilder();
        EntitySetConfiguration<ObjectPermission> objectPermissions = builder.EntitySet<ObjectPermission>("ObjectPermissions");
        EntitySetConfiguration<MemberPermission> memberPermissions = builder.EntitySet<MemberPermission>("MemberPermissions");
        EntitySetConfiguration<TypePermission> typePermissions = builder.EntitySet<TypePermission>("TypePermissions");

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
        readonly ITypesInfo typesInfo;
        readonly IDbContextFactory<ApplicationDbContext> dbFactory;

        public ObjectSpaceProviderFactory(ISecurityStrategyBase security, ITypesInfo typesInfo, IDbContextFactory<ApplicationDbContext> dbFactory) {
            this.security = security;
            this.typesInfo = typesInfo;
            this.dbFactory = dbFactory;
        }

        IEnumerable<IObjectSpaceProvider> IObjectSpaceProviderFactory.CreateObjectSpaceProviders() {
            yield return new SecuredEFCoreObjectSpaceProvider((ISelectDataSecurityProvider)security, dbFactory, typesInfo);
        }
    }
    ```

- Set up database connection settings in your Data Store Provider object. Add security extension to `DbContextFactory` to allow your application to filter data based on user permissions.
    ```csharp
    builder.Services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
        string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        options.UseSqlServer(connectionString);
        options.UseLazyLoadingProxies();
        options.UseSecurity(serviceProvider);
    }, ServiceLifetime.Scoped);
    ```
        
    The `IConfiguration` object is used to access the application configuration [appsettings.json](appsettings.json) file. In _appsettings.json_, add the connection string.
    ```json
    "ConnectionStrings": {
        "ConnectionString": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EFCoreTestDB;Integrated Security=True;MultipleActiveResultSets=True"
    }
    ```

    > **NOTE** The Security System requires [Multiple Active Result Sets](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/enabling-multiple-active-result-sets) in EF Core-based applications connected to the MS SQL database. We do not recommend that you remove “MultipleActiveResultSets=True;“ from the connection string or set the MultipleActiveResultSets parameter to false.

- Register security system and authentication in the [Program.cs](Program.cs). [AuthenticationStandard authentication](https://docs.devexpress.com/eXpressAppFramework/119064/Concepts/Security-System/Authentication#standard-authentication), and ASP.NET Core Identity authentication is registered automatically in [AspNetCore Security setup]().

    ```csharp
    builder.Services.AddXafAspNetCoreSecurity(builder.Configuration, options => {
        options.RoleType = typeof(PermissionPolicyRole);
        options.UserType = typeof(PermissionPolicyUser);
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
    For more details about how to create demo data from code, see the [Updater.cs](/EFCore/DatabaseUpdater/Updater.cs) class.

## Step 3. XAF Web API and OData Controllers for CRUD, Login, Logoff, etc.

- Register your business objects in [XAF Web Api]() to automatically implement CRUD logic & controllers for them.
    ```csharp
    builder.Services.AddXafWebApi(builder.Configuration, options => {
        options.BusinessObject<Employee>();
        options.BusinessObject<Department>();
    });
    ```

- [AccountController](Controllers/AccountController.cs) handles the Login and Logout operations.
The `Login` method is called when a user clicks the `Login` button on the login page. The `Logoff` method is called when a user clicks the `Logoff` button on the main page. A user is identified by the standard logon parameters, which are user name and password.

    ```csharp
    public class AccountController : ODataController {
        readonly IStandardAuthenticationService authenticationStandard;

        public AccountController(IStandardAuthenticationService authenticationStandard) {
            this.authenticationStandard = authenticationStandard;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public ActionResult Login(string userName, string password) {
            Response.Cookies.Append("userName", userName ?? string.Empty);
            ClaimsPrincipal principal = authenticationStandard.Authenticate(
                new AuthenticationStandardLogonParameters(userName, password));
            if(principal != null) {
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet("Logout")]
        public ActionResult Logout() {
            HttpContext.SignOutAsync();
            return Ok();
        }
    }
    ```

- [ActionsController](Controllers/ActionsController.cs) contains additional methods that process permissions. The `GetPermissions` method gathers permissions for all objects on the DevExtreme Data Grid current page and sends them to the client side as part of the response. The `GetTypePermissions` method gathers permissions for the type on the DevExtreme Data Grid's current page and sends them to the client side as part of the response.
    ```csharp
    public class ActionsController : ODataController {
        readonly IObjectSpaceFactory objectSpaceFactory;
        readonly SecurityStrategy security;
        readonly ITypesInfo typesInfo;
        public ActionsController(ISecurityProvider securityProvider, IObjectSpaceFactory objectSpaceFactory, ITypesInfo typesInfo) {
            this.typesInfo = typesInfo;
            this.objectSpaceFactory = objectSpaceFactory;
            this.security = (SecurityStrategy)securityProvider.GetSecurity();
        }

        [HttpPost("/GetPermissions")]
        public ActionResult GetPermissions(ODataActionParameters parameters) {
            if(parameters.ContainsKey("keys") && parameters.ContainsKey("typeName")) {
                string typeName = parameters["typeName"].ToString();

                ITypeInfo typeInfo = typesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
                if(typeInfo != null) {
                    Type type = typeInfo.Type;
                    using IObjectSpace objectSpace = objectSpaceFactory.CreateObjectSpace(type);
                    IEnumerable<int> keys = ((IEnumerable<string>)parameters["keys"]).Select(k => int.Parse(k));
                    IEnumerable<ObjectPermission> objectPermissions = objectSpace
                        .GetObjects(type, new InOperator(typeInfo.KeyMember.Name, keys))
                        .Cast<object>()
                        .Select(entity => CreateObjectPermission(entity, typeInfo));

                    return Ok(objectPermissions);
                }
            }
            return NoContent();
        }

        [HttpGet("/GetTypePermissions")]
        public ActionResult GetTypePermissions(string typeName) {
            ITypeInfo typeInfo = typesInfo.PersistentTypes.FirstOrDefault(t => t.Name == typeName);
            if(typeInfo != null) {
                Type type = typeInfo.Type;
                using IObjectSpace objectSpace = objectSpaceFactory.CreateObjectSpace(type);

                var result = new TypePermission {
                    Key = type.Name,
                    Create = security.CanCreate(type)
                };
                foreach(IMemberInfo member in GetPersistentMembers(typeInfo)) {
                    result.Data.Add(member.Name, security.CanWrite(type, member.Name));
                }
                return Ok(result);
            }
            return NoContent();
        }

        private ObjectPermission CreateObjectPermission(object entity, ITypeInfo typeInfo) {
            var objectPermission = new ObjectPermission {
                Key = typeInfo.KeyMember.GetValue(entity).ToString(),
                Write = security.CanWrite(entity),
                Delete = security.CanDelete(entity)
            };
            foreach(IMemberInfo member in GetPersistentMembers(typeInfo)) {
                objectPermission.Data.Add(member.Name, new MemberPermission {
                    Read = security.CanRead(entity, member.Name),
                    Write = security.CanWrite(entity, member.Name)
                });
            }
            return objectPermission;
        }

        private static IEnumerable<IMemberInfo> GetPersistentMembers(ITypeInfo typeInfo) {
            return typeInfo.Members.Where(p => p.IsVisible && p.IsProperty && (p.IsPersistent || p.IsList));
        }
    }
    ```
    
## Step 4: Implement the Client-Side App
1. The authentication page ([Authentication.html](wwwroot/Authentication.html)) and the main page([Index.html](wwwroot/Index.html)) represent the client side UI.
2. [authentication_code.js](wwwroot/js/authentication_code.js) gathers data from the login page and attempts to log the user in.

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
        
3. [index_code.js](wwwroot/js/index_code.js) configures the DevExtreme Data Grid and logs the user out. 
The [onLoaded](https://js.devexpress.com/Documentation/ApiReference/Data_Layer/ODataStore/Configuration/#onLoaded) function sends a request to the server to obtain permissions for the current data grid page.

    ```javascript
    function onLoaded(data) {
        var oids = $.map(data, function (val) {
            return val.ID.toString();
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

5. The `onInitialized` function handles the data grid's [initialized](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#initialized) event 
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

6. The `onEditorPreparing` function handles the data grid's [editorPreparing](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#editorPreparing) event and checks Read and Write operation permissions. 
If the Read operation permission is denied, it displays the "*******" placeholder and disables the editor. If the Write operation permission is denied, the editor is disabled.

    ```javascript
    function onEditorPreparing(e) {
        if (e.parentType === "dataRow") {
            var dataField = e.dataField.split('.')[0];
            if (!e.row.isNewRow) {
                var key = e.row.key;
                var objectPermission = getPermission(key.toString());
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

7. The `onCellPrepared` function handles the data grid's [cellPrepared](https://js.devexpress.com/Documentation/ApiReference/UI_Widgets/dxDataGrid/Events/#cellPrepared) event and checks Read, Write, and Delete operation permissions. 
If the Read permission is denied, it displays the "*******" placeholder in data grid cells. Write and Delete operation permission checks define whether the Write and Delete actions should be displayed or not.

    ```javascript
    function onCellPrepared(e) {
        if (e.rowType === "data") {
            var key = e.key;
            var objectPermission = getPermission(key.toString());
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
    Use the [SecurityStrategy.IsGranted](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategy.IsGranted(DevExpress.ExpressApp.Security.IPermissionRequest)) method to determine when to mask default values with the "*******" placeholder in the UI.

8. The `getPermission` function returns the permission object for a business object. The business object is identified by the key passed in function parameters:

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
