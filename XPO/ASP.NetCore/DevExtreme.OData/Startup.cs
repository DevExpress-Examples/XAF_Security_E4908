using BusinessObjectsLibrary;
using DevExpress.Persistent.BaseImpl;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace ASPNETCoreODataService {
	public class Startup {
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services) {
			
			services.AddControllers(mvcOptions => {
				mvcOptions.EnableEndpointRouting = false;
			}).AddOData(opt => opt.Count().Filter().Expand().Select().OrderBy().SetMaxTop(null).AddRouteComponents( GetEdmModel()));

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie();
			services.AddSingleton<XpoDataStoreProviderService>();
			services.AddSingleton(Configuration);
			services.AddHttpContextAccessor();
			services.AddScoped<SecurityProvider>();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if(env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			}
			else {
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
