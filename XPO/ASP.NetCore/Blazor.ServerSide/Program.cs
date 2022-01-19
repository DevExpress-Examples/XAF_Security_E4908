using Blazor.ServerSide.Helpers;
using Blazor.ServerSide.Services;
using DevExpress.ExpressApp.Blazor.AmbientContext;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server.Circuits;

var builder = WebApplication.CreateBuilder(args);

//Force DemoDataStoreProvider initialize
_ = DevExpress.ExpressApp.Blazor.DemoServices.DemoDataStoreProvider.ConnectionString;

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddSingleton<XpoDataStoreProviderService>();
builder.Services.AddScoped<SecurityProvider>();

builder.Services.AddSingleton<DevExpress.ExpressApp.Blazor.DemoServices.ISharedDataService, DevExpress.ExpressApp.Blazor.DemoServices.SharedDataService>();
builder.Services.AddScoped<DevExpress.ExpressApp.Blazor.DemoServices.IDemoDataStoreProvider, DevExpress.ExpressApp.Blazor.DemoServices.DemoDataStoreProvider>();
builder.Services.AddScoped<IValueManagerStorageContext, ValueManagerStorageContext>();
builder.Services.AddSingleton<IValueManagerStorageContainerInitializer, ValueManagerContextActivator>();
builder.Services.AddScoped<DemoDataInMemoryProvider>();
builder.Services.AddScoped<InitialApplicationStateProvider>();
builder.Services.AddScoped<InitialApplicationStateInitializer>();
builder.Services.AddScoped<CircuitHandler, CircuitHandlerProxy>();

builder.Services.AddScoped((serviceProvider) => {
    AuthenticationMixed authentication = new AuthenticationMixed();
    authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
    authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
    authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
    return security;
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
}
else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();

app.UseMiddleware<DevExpress.ExpressApp.Blazor.DemoServices.DemoDataMiddleware>();
app.UseMiddleware<ValueManagerContextMiddleware>();


app.UseDefaultFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapFallbackToPage("/_Host");
    endpoints.MapBlazorHub();
});
app.UseDemoData(app.Configuration.GetConnectionString("ConnectionString"));

app.Run();