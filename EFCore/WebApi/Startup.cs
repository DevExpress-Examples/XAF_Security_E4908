﻿using System.Runtime.InteropServices;
using System.Text;
using DevExpress.ExpressApp.AspNetCore.WebApi;
using DevExpress.ExpressApp.Core;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.Authentication.ClientServer;
using DevExpress.ExpressApp.WebApi.Services;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using DevExpress.Persistent.BaseImpl.EFCore.AuditTrail;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPI.API.Security;
using WebAPI.BusinessObjects;
using WebAPI.Services;

namespace WebAPI;

public class Startup {
    public Startup(IConfiguration configuration) {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services) {
        
        services
            .AddScoped<IObjectSpaceProviderFactory, ObjectSpaceProviderFactory>()
            .AddSingleton<IWebApiApplicationSetup, WebApiApplicationSetup>();

        services.AddXafAspNetCoreSecurity(Configuration, options => {
		        options.RoleType = typeof(PermissionPolicyRole);
		        // ApplicationUser descends from PermissionPolicyUser and supports the OAuth authentication. For more information, refer to the following topic: https://docs.devexpress.com/eXpressAppFramework/402197
		        // If your application uses PermissionPolicyUser or a custom user type, set the UserType property as follows:
		        options.UserType = typeof(ApplicationUser);
		        // ApplicationUserLoginInfo is only necessary for applications that use the ApplicationUser user type.
		        // If you use PermissionPolicyUser or a custom user type, comment out the following line:
		        options.UserLoginInfoType = typeof(ApplicationUserLoginInfo);

		        options.SupportNavigationPermissionsForTypes = false;
	        })
	        .AddAuthenticationStandard(options => {
		        options.IsSupportChangePassword = true;
	        });
        const string customBearerSchemeName = "CustomBearer";
        services
            .AddAuthentication(customBearerSchemeName)
            
            .AddCookie()
        //Configure OAuth2 Identity Providers based on your requirements. For more information, see
        //https://docs.devexpress.com/eXpressAppFramework/402197/task-based-help/security/how-to-use-active-directory-and-oauth2-authentication-providers-in-blazor-applications
        //https://developers.google.com/identity/protocols/oauth2
        //https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-auth-code-flow
        //https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow
            .AddMicrosoftIdentityWebApi(Configuration, configSectionName: "Authentication:AzureAd", jwtBearerScheme: "AzureAd");

        services.AddAuthorization(options => {
            options.DefaultPolicy = new AuthorizationPolicyBuilder(
                CookieAuthenticationDefaults.AuthenticationScheme,
                "AzureAd")
                    .RequireAuthenticatedUser()
                    .RequireXafAuthentication()
                    .Build();
        });
        services.AddDbContextFactory<WebAPIEFCoreDbContext>((serviceProvider, options) => {
            // Uncomment this code to use an in-memory database. This database is recreated each time the server starts. With the in-memory database, you don't need to make a migration when the data model is changed.
            // Do not use this code in production environment to avoid data loss.
            // We recommend that you refer to the following help topic before you use an in-memory database: https://docs.microsoft.com/en-us/ef/core/testing/in-memory
            //options.UseInMemoryDatabase("InMemory");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                string connectionString = Configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(connectionString);
            }
            else {
                string sqliteDBPath = Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WebAPIDemo.db");
                options.UseSqlite($"Data Source={sqliteDBPath}");
            }
            options.UseChangeTrackingProxies();
            options.UseObjectSpaceLinkProxies();
            options.UseLazyLoadingProxies();
            options.UseSecurity(serviceProvider);
        }, ServiceLifetime.Scoped);
        services.AddXafReportingCore(options => {
            options.ReportDataType = typeof(ReportDataV2);
        });
        services.AddDbContextFactory<WebAPIAuditingDbContext>((_, options) => {
            string connectionString = Configuration.GetConnectionString("ConnectionString");
            options.UseSqlServer(connectionString);
            options.UseChangeTrackingProxies();
            options.UseObjectSpaceLinkProxies();
            options.UseLazyLoadingProxies();
        }, ServiceLifetime.Scoped);
        services.AddAuditTrail().AddAuditedDbContextFactory<WebAPIEFCoreDbContext, WebAPIAuditingDbContext>();

        services
	        .AddXafWebApi(Configuration, options => {
		        // Use options.BusinessObject<YourBusinessObject>() to make the Business Object available in the Web API and generate the GET, POST, PUT, and DELETE HTTP methods for it.
		        options.BusinessObject<Post>();
		        options.BusinessObject<ApplicationUser>();
		        options.BusinessObject<MediaDataObject>();
		        options.BusinessObject<MediaResourceObject>();
	        });
        services
            .AddControllers()
            .AddOData((options, serviceProvider) => options
                .AddRouteComponents("api/odata", new EdmModelBuilder(serviceProvider).GetEdmModel())
                .EnableQueryFeatures(100));

        services.AddCors(options => options.AddPolicy(
            "Open", builder => builder.SetIsOriginAllowed(_ => true).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

        services.AddSwaggerGen(c => {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new OpenApiInfo {
                Title = "WebAPI API",
                Version = "v1",
                Description = @"Use AddXafWebApi(options) in the WebAPI.WebApi\Startup.cs file to make Business Objects available in the Web API."
            });
            var azureAdAuthorityUrl = $"{Configuration["Authentication:AzureAd:Instance"]}{Configuration["Authentication:AzureAd:TenantId"]}";
            c.AddSecurityDefinition("AzureAd", new OpenApiSecurityScheme {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows() {
                    AuthorizationCode = new OpenApiOAuthFlow() {
                        AuthorizationUrl = new Uri($"{azureAdAuthorityUrl}/oauth2/v2.0/authorize"),
                        TokenUrl = new Uri($"{azureAdAuthorityUrl}/oauth2/v2.0/token"),
                        Scopes = new Dictionary<string, string> {
                            // Configure scopes corresponding to https://docs.microsoft.com/en-us/azure/active-directory/develop/quickstart-configure-app-expose-web-apis
                            { @"[Enter the scope name in the WebAPI.WebApi\Startup.cs file]", @"[Enter the scope description in the WebAPI.WebApi\Startup.cs file]" }
                        }
                    }
                }
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                {
                    new OpenApiSecurityScheme {
                        Reference = new OpenApiReference {
                            Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                            Id = "AzureAd"
                        },
                        In = ParameterLocation.Header
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
        if(env.IsDevelopment()) {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPI WebApi v1");
                c.OAuthClientId(Configuration["Authentication:AzureAd:ClientId"]);
                c.OAuthUsePkce();
            });
        }
        else {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. To change this for production scenarios, see: https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        app.UseHttpsRedirection();
        app.UseRequestLocalization();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseCors("Open");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }
}
