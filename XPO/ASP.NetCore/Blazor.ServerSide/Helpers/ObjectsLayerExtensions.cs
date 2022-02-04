using Blazor.ServerSide.Services;
using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;

namespace Blazor.ServerSide.Helpers {
    public static class ObjectsLayerExtensions {

        public static IServiceCollection AddXafSecurityObjectsLayer(this IServiceCollection services) {
            InitializeTypesInfo();

            services.AddSingleton<XpoDataStoreProviderService>();

            services.AddScoped<IObjectSpaceProviderService, ObjectSpaceProviderService>();
            services.AddScoped(serviceProvider => (IObjectSpaceFactory)serviceProvider.GetService<IObjectSpaceProviderService>());

            services.AddSingleton<ITypesInfo>(x => {
                return XafTypesInfo.Instance;
            });

            services.AddScoped<XafSecurityAuthenticationService>();
            services.AddScoped<ISecurityProvider, SecurityProvider>();
            services.AddScoped<XafSecurityLoginService>();
            services.AddScoped<IPrincipalProvider, PrincipalProvider>();
            services.AddScoped(serviceProvider => (IPrincipalProviderInitializer)serviceProvider.GetService<IPrincipalProvider>());
            services.AddScoped<ILogonParameterProvider, LogonParameterProvider>();

            return services;
        }

        private static void InitializeTypesInfo() {
            XpoTypesInfoHelper.ForceInitialize();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}
