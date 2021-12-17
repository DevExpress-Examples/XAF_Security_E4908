using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

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
builder.Services.AddSingleton<XpoDataStoreProviderService>();
builder.Services.AddScoped<SecurityProvider>();
builder.Services.AddScoped((serviceProvider) => {
    AuthenticationMixed authentication = new AuthenticationMixed();
    authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
    authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
    authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
    SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
    security.RegisterXPOAdapterProviders();
    return security;
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
app.UseDemoData(app.Configuration.GetConnectionString("ConnectionString"));
app.Run();