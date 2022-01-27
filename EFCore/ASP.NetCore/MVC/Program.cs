using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MvcApplication;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.DC;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

string loginPath = "/Authentication";
Action<MvcNewtonsoftJsonOptions> JsonOptions =
    options => {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    };
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(JsonOptions);
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
     .AddCookie(options => {
         options.LoginPath = loginPath;
     });
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
app.UseDemoData<ApplicationDbContext>(app.Configuration.GetConnectionString("ConnectionString"),
    (builder, connectionString) =>
    builder.UseSqlServer(connectionString));
app.Run();