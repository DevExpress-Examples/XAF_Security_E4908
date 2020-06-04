using DevExpress.Persistent.BaseImpl;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if NETCOREAPP3_1
using Microsoft.Extensions.Hosting;
#else
using Microsoft.AspNetCore.Mvc; 
#endif
using Microsoft.OData.Edm;
using System.Linq;
using XafSolution.Module.BusinessObjects;

namespace ASPNETCoreODataService {
	public class Startup {
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services) {
			services.AddOData();
#if NETCOREAPP3_1
			services.AddControllers(mvcOptions => {
				mvcOptions.EnableEndpointRouting = false;
			});
#else
			services.AddMvc(options => {
				options.EnableEndpointRouting = false;
			}).SetCompatibilityVersion(CompatibilityVersion.Latest);
#endif
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie();
			services.AddSingleton<XpoDataStoreProviderService>();
			services.AddSingleton(Configuration);
			services.AddHttpContextAccessor();
			services.AddScoped<SecurityProvider>();
		}

#if NETCOREAPP3_1
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
#else
		public void Configure(IApplicationBuilder app, IHostingEnvironment env) { 
#endif
			if(env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
				app.UseHsts();
			}
			app.UseAuthentication();
			app.UseMiddleware<UnauthorizedRedirectMiddleware>();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseCookiePolicy();
			app.UseMvc(routeBuilder => {
				routeBuilder.EnableDependencyInjection();
				routeBuilder.Count().Expand().Select().OrderBy().Filter().MaxTop(null);
				routeBuilder.MapODataServiceRoute("ODataRoutes", null, GetEdmModel());
			});
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
