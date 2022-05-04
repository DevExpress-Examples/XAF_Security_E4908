using Blazor.ServerSide.Services;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; });
builder.Services.AddDevExpressBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.Configure<DevExpress.Blazor.Configuration.GlobalOptions>(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
});

builder.Services
    .AddSingleton<ITypesInfo>(s => {
        var typesInfo = XafTypesInfo.Instance;
        XpoTypesInfoHelper.ForceInitialize((TypesInfo)typesInfo);
        typesInfo.RegisterEntity(typeof(Employee));
        typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
        typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
        return typesInfo;
    })
    .AddScoped<IObjectSpaceProviderFactory, ObjectSpaceProviderFactory>()
    .AddSingleton<IXpoDataStoreProvider>((serviceProvider) => {
        var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString");
        return XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
    });

builder.Services.AddXafAspNetCoreSecurity(builder.Configuration, options => {
    options.RoleType = typeof(PermissionPolicyRole);
    options.UserType = typeof(PermissionPolicyUser);
    options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
    options.SupportNavigationPermissionsForTypes = false;
}).AddAuthenticationStandard();

var app = builder.Build();

if(app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
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

app.UseEndpoints(endpoints => {
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub();
});
app.UseDemoData();

app.Run();

