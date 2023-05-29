using Blazor.WebAssembly;
using Blazor.WebAssembly.Services;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddScoped<CookieHandler>();
builder.Services.AddHttpClient("API", options => options.BaseAddress = new Uri("https://localhost:5001/api/"))
    .AddHttpMessageHandler<CookieHandler>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<WebAPIAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider => provider.GetRequiredService<WebAPIAuthenticationStateProvider>());
builder.Services.AddScoped<IWebAPI, WebAPI>();
builder.Services.AddScoped<SimpleODataClientDataSource>();

builder.Services.AddDevExpressBlazor(options => {
    options.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5;
    options.SizeMode = DevExpress.Blazor.SizeMode.Medium;
});
await builder.Build().RunAsync();

