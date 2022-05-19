using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.WebApi.Services;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExtreme.OData.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

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

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();
builder.Services.AddAuthorization();

builder.Services
    .AddSingleton<ITypesInfo>(s => {
        var typesInfo = new TypesInfo();
        typesInfo.RegisterEntity(typeof(Employee));
        typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
        typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
        return typesInfo;
    })
    .AddScoped<IObjectSpaceProviderFactory, ObjectSpaceProviderFactory>()
    .AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
        string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
        options.UseSqlServer(connectionString);
        options.UseLazyLoadingProxies();
        options.UseSecurity(serviceProvider);
    }, ServiceLifetime.Scoped);

builder.Services.AddXafWebApi(builder.Configuration, options => {
    options.BusinessObject<Employee>();
    options.BusinessObject<Department>();
});

builder.Services.AddXafAspNetCoreSecurity(builder.Configuration, options => {
    options.RoleType = typeof(PermissionPolicyRole);
    options.UserType = typeof(PermissionPolicyUser);
}).AddAuthenticationStandard();

builder.Services.AddSwaggerGen(c => {
    c.EnableAnnotations();
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MainDemo", Version = "v1" });

    c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme() {
        Type = SecuritySchemeType.Http,
        Name = "Bearer",
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
        {
            new OpenApiSecurityScheme() {
                Reference = new OpenApiReference() {
                    Type = ReferenceType.SecurityScheme,
                    Id = "JWT"
                }
            },
            new string[0]
        },
    });
});

var app = builder.Build();

if(app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MainDemo WebApi v1");
    });
}
else {
    app.UseHsts();
}
app.UseODataQueryRequest();
app.UseODataBatching();

app.UseRouting();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<UnauthorizedRedirectMiddleware>();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseEndpoints(endpoints => {
    endpoints.MapControllers();
});
app.UseDemoData();

app.Run();

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
