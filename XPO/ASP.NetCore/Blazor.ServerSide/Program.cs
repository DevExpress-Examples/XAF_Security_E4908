using Blazor.ServerSide.Helpers;
using Blazor.ServerSide.Services;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.WebApi.Services;
using DevExpress.ExpressApp.WebApi.Swashbuckle;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.OData;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddXafSecurityObjectsLayer();

builder.Services.AddXafSecurity(options => {
    options.RoleType = typeof(DevExpress.Persistent.BaseImpl.PermissionPolicy.PermissionPolicyRole);
    options.UserType = typeof(PermissionPolicyUser);
    options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
    options.SupportNavigationPermissionsForTypes = false;
}).AddExternalAuthentication<PrincipalProvider>()
            .AddAuthenticationStandard(options => {
                options.IsSupportChangePassword = true;
            });
builder.Services.AddXafWebApi(options => {
    options.BusinessObject<Department>();
    options.BusinessObject<Employee>();
});
builder.Services.AddControllers().AddOData(options => {
    options
        .EnableQueryFeatures(100)
        .AddRouteComponents("api/odata", new XafApplicationEdmModelBuilder(builder.Services).GetEdmModel());
});
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
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                    {
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

    c.SchemaFilter<XpoSchemaFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();

    app.UseSwagger();
    app.UseSwaggerUI(c => {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "MainDemo WebApi v1");
    });
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseDefaultFiles();
app.UseRouting();
app.UseAuthorization();
app.UseMiddleware<LogOutMiddleware>();
app.UseMiddleware<PrincipalProviderInitializerMiddleware>();
app.UseEndpoints(endpoints => {
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub();
    endpoints.MapControllers();
});
app.UseDemoData(app.Configuration.GetConnectionString("ConnectionString"));

app.Run();

