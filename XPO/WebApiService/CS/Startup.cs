using DevExpress.Xpo;
using DevExpress.Xpo.DB;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WebApiService {
    public class Startup {
        public Startup(IConfiguration configuration, IWebHostEnvironment env) {
            Configuration = configuration;
            HostingEnvironment = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostingEnvironment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            services.AddControllers();

            string connectionString;
            AutoCreateOption autoCreateOption = AutoCreateOption.SchemaAlreadyExists;
            if(HostingEnvironment.IsDevelopment()) {
                connectionString = Configuration.GetConnectionString("Dev");
                autoCreateOption = AutoCreateOption.DatabaseAndSchema;
            } else {
                connectionString = Configuration.GetConnectionString("Prod");
                connectionString = XpoDefault.GetConnectionPoolString(connectionString);
            }
            IDataStore dataStore = XpoDefault.GetConnectionProvider(connectionString, autoCreateOption);
            services.AddSingleton(new WebApiDataStoreService(dataStore));

            // add XML Serializer formatters
            services.AddMvc()
                .AddXmlSerializerFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
