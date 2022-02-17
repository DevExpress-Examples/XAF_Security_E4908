using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using SecutirySharedLibrary.Services;
using SecutirySharedLibrary.Middleware;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using BusinessObjectsLibrary.BusinessObjects;
using Blazor.ServerSide.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddXafSecurityObjectsLayer<ObjectSpaceFactory>(options => {
    options.Events.CustomizeTypesInfo = (typesInfo, s) => {
        XpoTypesInfoHelper.ForceInitialize();
        ((TypesInfo)typesInfo).GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
        typesInfo.RegisterEntity(typeof(Employee));
        typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
        typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
    };
});

builder.Services.AddSingleton<IXpoDataStoreProvider>((serviceProvider) => {
    var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString");
    return XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
});

builder.Services.AddXafSecurity(options => {
    options.RoleType = typeof(PermissionPolicyRole);
    options.UserType = typeof(PermissionPolicyUser);
    options.Events.OnSecurityStrategyCreated = securityStrategy => ((SecurityStrategy)securityStrategy).RegisterXPOAdapterProviders();
    options.SupportNavigationPermissionsForTypes = false;
}).AddExternalAuthentication<PrincipalProvider>()
        .AddAuthenticationStandard(options => {
            options.IsSupportChangePassword = true;
        });

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
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
app.UseMiddleware<LogOut>();
app.UseMiddleware<PrincipalProviderInitializer>();
app.UseEndpoints(endpoints => {
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub();
});
app.UseDemoData();

app.Run();

