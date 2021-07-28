using BusinessObjectsLibrary;
using DevExpress.Persistent.BaseImpl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if NET5_0
using Microsoft.Extensions.Hosting;
#else
using Microsoft.AspNetCore.Mvc; 
#endif
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using System.Linq;

namespace ASPNETCoreODataService {
	public class Startup {
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services) {
			
#if NET5_0
			services.AddControllers(mvcOptions => {
				mvcOptions.EnableEndpointRouting = false;
			}).AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(null).AddRouteComponents( GetEdmModel()));
#else
			services.AddOData();
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

#if NET5_0
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
			app.UseODataQueryRequest();
			app.UseODataBatching();

			app.UseRouting();
			//app.UseCors();
			app.UseAuthentication();
			app.UseMiddleware<UnauthorizedRedirectMiddleware>();
			app.UseDefaultFiles();
			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseCookiePolicy();
			app.UseEndpoints(endpoints => {
				endpoints.MapControllers();
			});
			//app.UseMvc(routeBuilder => {
			//	//routeBuilder.EnableDependencyInjection();
			//	//routeBuilder.Count().Expand().Select().OrderBy().Filter().MaxTop(null);
			//	routeBuilder.MapRoute("ODataRoutes",null, GetEdmModel());
			//});
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
