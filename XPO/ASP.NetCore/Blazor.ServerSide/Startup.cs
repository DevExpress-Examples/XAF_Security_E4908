using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;

namespace BlazorApplication.NetCore
{
    public class Startup {
		public IConfiguration Configuration { get; }
		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}
		public void ConfigureServices(IServiceCollection services) {
			services.AddSession();
			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				 .AddCookie();
			services.AddRazorPages();
			services.AddServerSideBlazor();
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
				app.UseExceptionHandler("/Error");
				app.UseHsts();
			}
			app.UseSession();
			app.UseStaticFiles();
			app.UseAuthentication();
			app.UseDefaultFiles();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseEndpoints(endpoints => {
				endpoints.MapFallbackToPage("/_Host");
				endpoints.MapBlazorHub();
			});
			app.UseDemoData(Configuration.GetConnectionString("ConnectionString"));
		}
	}
}
