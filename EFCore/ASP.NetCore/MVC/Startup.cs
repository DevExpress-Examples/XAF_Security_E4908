using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;


namespace MvcApplication {
    public class Startup {
        private string loginPath = "/Authentication";
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc(options => {
                options.EnableEndpointRouting = false;
            })

                .SetCompatibilityVersion(CompatibilityVersion.Latest);

            services.AddControllers().
            AddNewtonsoftJson(options => {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                     .AddCookie(options => {
                         options.LoginPath = loginPath;
                     });


            services.AddScoped((serviceProvider) =>
            serviceProvider.GetRequiredService<SecurityStrategyComplexProvider>().GetSecurity());
            services.AddScoped<ISelectDataSecurityProvider>((serviceProvider) =>
            serviceProvider.GetRequiredService<SecurityStrategyComplexProvider>().GetSecurity());
            services.AddDbContextFactory<ApplicationDbContext>((serviceProvider, options) => {
                // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
                // Do not use this code in production environment to avoid data loss.
                // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
                //options.UseInMemoryDatabase("InMemory");
                string connectionString = Configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(connectionString);
                options.UseLazyLoadingProxies();
                options.UseSecurity(serviceProvider);
            }, ServiceLifetime.Scoped);
            services.AddSingleton(Configuration);
            services.AddHttpContextAccessor();
            services.AddScoped<SecurityProvider>();
            services.AddScoped<SecurityStrategyComplexProvider>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.

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
            app.UseDemoData<ApplicationDbContext>((builder, _) =>
                builder.UseSqlServer(Configuration.GetConnectionString("ConnectionString")));
        }
    }
}
