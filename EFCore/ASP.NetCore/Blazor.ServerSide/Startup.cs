using BusinessObjectsLibrary.BusinessObjects;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Blazor.ServerSide.Helpers;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.ExpressApp;

namespace Blazor.ServerSide {
    public class Startup {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services) {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddDevExpressBlazor();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            services.AddHttpContextAccessor();
            services.AddSession();
            services.AddSingleton(Configuration);

            services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
                string connectionString = Configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(connectionString);
                options.UseLazyLoadingProxies();
                options.UseSecurity(serviceProvider.GetRequiredService<SecurityStrategyComplex>(), XafTypesInfo.Instance);
            }, ServiceLifetime.Scoped);
            services.AddScoped<SecurityProvider>();
            services.AddScoped((serviceProvider) => {
                AuthenticationMixed authentication = new AuthenticationMixed();
                authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
                authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
                authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
                SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
                return security;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
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
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
            app.UseDemoData<ApplicationDbContext>((builder, _) =>
                builder.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
        }
    }
}
