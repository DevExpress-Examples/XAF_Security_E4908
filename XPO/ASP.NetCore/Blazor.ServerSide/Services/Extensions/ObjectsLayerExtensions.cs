using Blazor.ServerSide.Services;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Services;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ObjectsLayerExtensions {

        public static IServiceCollection AddXafSecurityObjectsLayer(this IServiceCollection services) {
            services.AddScoped<IObjectSpaceFactory, ObjectSpaceFactory>();

            services.AddSingleton<ITypesInfo>(x => {
                XpoTypesInfoHelper.ForceInitialize();
                XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
                XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
                XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
                return XafTypesInfo.Instance;
            });

            services.AddSingleton<IXpoDataStoreProvider>((serviceProvider) => {
                var connectionString = serviceProvider.GetRequiredService<IConfiguration>().GetConnectionString("ConnectionString");
                return XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
            });

            services.AddScoped<SecurityStandartAuthenticationService>();
            services.AddScoped<ISecurityProvider, SecurityProvider>();
            services.AddScoped<PrincipalAuthenticationService>();
            services.AddScoped<IPrincipalProvider, PrincipalProvider>();
            services.AddScoped(serviceProvider => (IPrincipalProviderInitializer)serviceProvider.GetService<IPrincipalProvider>());

            return services;
        }
    }
}
