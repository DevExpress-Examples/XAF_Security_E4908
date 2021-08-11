using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using Blazor.ServerSide.Helpers;

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
            services.AddSingleton<XpoDataStoreProviderService>();
            services.AddSingleton(Configuration);
            services.AddScoped<SecurityProvider>();
        }

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
                endpoints.MapFallbackToPage("/_Host");
                endpoints.MapBlazorHub();
            });
            app.UseDemoData(Configuration.GetConnectionString("ConnectionString"));
        }
    }
}
