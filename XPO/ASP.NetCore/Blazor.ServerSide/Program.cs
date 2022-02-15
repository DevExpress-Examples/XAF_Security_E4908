using Blazor.ServerSide.Helpers;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.DC.Xpo;
using Blazor.ServerSide.Services;
using DevExpress.ExpressApp.Blazor.AmbientContext;
using DevExpress.ExpressApp.Blazor.Services;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
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
builder.Services.AddSingleton<IXpoDataStoreProvider>((serviceProvider) => {
    string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
    return dataStoreProvider;
});
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
    ITypesInfo typesInfo = serviceProvider.GetRequiredService<ITypesInfo>();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
    security.RegisterXPOAdapterProviders();
    return security;
});
builder.Services.AddSingleton<ITypesInfo>((serviceProvider) => {
    TypesInfo typesInfo = new TypesInfo();
    RegisterEntities(typesInfo);
    return typesInfo;
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
app.UseDemoData();

app.Run();

static void RegisterEntities(TypesInfo typesInfo) {
    typesInfo.GetOrAddEntityStore(ti => new XpoTypeInfoSource(ti));
    typesInfo.RegisterEntity(typeof(Employee));
    typesInfo.RegisterEntity(typeof(PermissionPolicyUser));
    typesInfo.RegisterEntity(typeof(PermissionPolicyRole));
}