using Blazor.ServerSide.Helpers;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.DC;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDevExpressBlazor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
    string connectionString = builder.Configuration.GetConnectionString("ConnectionString");
    options.UseSqlServer(connectionString);
    options.UseLazyLoadingProxies();
    ITypesInfo typesInfo = serviceProvider.GetRequiredService<ITypesInfo>();
    options.UseSecurity(serviceProvider.GetRequiredService<SecurityStrategyComplex>(), typesInfo);
}, ServiceLifetime.Scoped);
builder.Services.AddScoped<SecurityProvider>();
builder.Services.AddScoped((serviceProvider) => {
    AuthenticationMixed authentication = new AuthenticationMixed();
    authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
    authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
    authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
    ITypesInfo typesInfo = serviceProvider.GetRequiredService<ITypesInfo>();
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication, typesInfo);
    return security;
});
builder.Services.AddSingleton<ITypesInfo, TypesInfo>();

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
app.UseDefaultFiles();
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});
app.UseDemoData<ApplicationDbContext>(app.Configuration.GetConnectionString("ConnectionString"),
    (builder, connectionString) =>
    builder.UseSqlServer(connectionString));
app.Run();