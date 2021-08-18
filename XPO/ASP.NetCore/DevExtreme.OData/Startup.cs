using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using DevExpress.Persistent.BaseImpl;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace DevExtreme.OData {
    public class Startup {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers(mvcOptions => {
                mvcOptions.EnableEndpointRouting = false;
            }).AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(null).AddRouteComponents(GetEdmModel()));
            services.AddHttpContextAccessor();
            services.AddSingleton(Configuration);
            services.AddScoped<SecurityProvider>();
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();
            services.AddSingleton<XpoDataStoreProviderService>();
            services.AddScoped((serviceProvider) => {
                AuthenticationMixed authentication = new AuthenticationMixed();
                authentication.LogonParametersType = typeof(AuthenticationStandardLogonParameters);
                authentication.AddAuthenticationStandardProvider(typeof(PermissionPolicyUser));
                authentication.AddIdentityAuthenticationProvider(typeof(PermissionPolicyUser));
                SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
                return security;
            });
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseHsts();
            }
            app.UseODataQueryRequest();
            app.UseODataBatching();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseMiddleware<UnauthorizedRedirectMiddleware>();
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
            app.UseDemoData(Configuration.GetConnectionString("ConnectionString"));
        }
        private IEdmModel GetEdmModel() {
            ODataModelBuilder builder = new ODataConventionModelBuilder();
            EntitySetConfiguration<Employee> employees = builder.EntitySet<Employee>("Employees");
            EntitySetConfiguration<Department> departments = builder.EntitySet<Department>("Departments");
            EntitySetConfiguration<Party> parties = builder.EntitySet<Party>("Parties");
            EntitySetConfiguration<ObjectPermission> objectPermissions = builder.EntitySet<ObjectPermission>("ObjectPermissions");
            EntitySetConfiguration<MemberPermission> memberPermissions = builder.EntitySet<MemberPermission>("MemberPermissions");
            EntitySetConfiguration<TypePermission> typePermissions = builder.EntitySet<TypePermission>("TypePermissions");

            employees.EntityType.HasKey(t => t.Oid);
            departments.EntityType.HasKey(t => t.Oid);
            parties.EntityType.HasKey(t => t.Oid);

            ActionConfiguration login = builder.Action("Login");
            login.Parameter<string>("userName");
            login.Parameter<string>("password");

            builder.Action("Logout");

            ActionConfiguration getPermissions = builder.Action("GetPermissions");
            getPermissions.Parameter<string>("typeName");
            getPermissions.CollectionParameter<string>("keys");

            ActionConfiguration getTypePermissions = builder.Action("GetTypePermissions");
            getTypePermissions.Parameter<string>("typeName");
            getTypePermissions.ReturnsFromEntitySet<TypePermission>("TypePermissions");
            return builder.GetEdmModel();
        }
    }
}
