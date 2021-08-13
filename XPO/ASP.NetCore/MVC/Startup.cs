using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace MvcApplication {
    public class Startup {
        private string loginPath = "/Authentication";
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services) {
            JsonResolver resolver = new JsonResolver();
            Action<MvcNewtonsoftJsonOptions> JsonOptions =
                options => {
                    options.SerializerSettings.ContractResolver = resolver;
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                };
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
            }).SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddControllers()
                .AddNewtonsoftJson(JsonOptions);
            services.AddSingleton(Configuration);
            services.AddHttpContextAccessor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                 .AddCookie(options => {
                     options.LoginPath = loginPath;
                 });
            services.AddSingleton<XpoDataStoreProviderService>();
            services.AddScoped<SecurityProvider>();
            services.AddScoped((serviceProvider) => {
                AuthenticationMixed authentication = new AuthenticationMixed();
                authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
                authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
                authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
                SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
                security.RegisterXPOAdapterProviders();
                return security;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseDefaultFiles();
            app.UseHttpsRedirection();
            app.UseStaticFiles(new StaticFileOptions() {
                OnPrepareResponse = context => {
                    if(context.Context.User.Identity.IsAuthenticated) {
                        return;
                    } else {
                        string referer = context.Context.Request.Headers["Referer"].ToString();
                        string authenticationPagePath = loginPath;
                        string vendorString = "vendor.css";
                        if(context.Context.Request.Path.HasValue && context.Context.Request.Path.StartsWithSegments(authenticationPagePath)
                            || referer != null && (referer.Contains(authenticationPagePath) || referer.Contains(vendorString))) {
                            return;
                        }
                        context.Context.Response.Redirect(loginPath);
                    }
                }
            });
            app.UseCookiePolicy();
            app.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseDemoData(Configuration.GetConnectionString("ConnectionString"));
        }
    }
}
