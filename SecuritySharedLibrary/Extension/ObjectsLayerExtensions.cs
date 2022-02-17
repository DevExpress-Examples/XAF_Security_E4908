using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Services;
using Microsoft.Extensions.Options;
using SecutirySharedLibrary.Services;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ObjectsLayerExtensions{

        public class XafSecurityObjectsLayerEvents {
            public Action<ITypesInfo, IServiceProvider>? CustomizeTypesInfo { get; set; } 
        }

        public class XafSecurityObjectsLayerOptions {

            public XafSecurityObjectsLayerEvents Events { get; } = new XafSecurityObjectsLayerEvents();


        }
        public static IServiceCollection AddXafSecurityObjectsLayer<T>(this IServiceCollection services) where T : ObjectSpaceFactoryBase => services.AddXafSecurityObjectsLayer<T>(_ => { });
       
        public static IServiceCollection AddXafSecurityObjectsLayer<T>(this IServiceCollection services, Action<XafSecurityObjectsLayerOptions> options) where T : ObjectSpaceFactoryBase{
            if (options == null) {
                throw new ArgumentNullException(nameof(options));
            }
            services.Configure(options);

            services.AddSingleton<ITypesInfo>(s => {
                    TypesInfo typesInfo = new TypesInfo();
                    var o = s.GetRequiredService<IOptions<XafSecurityObjectsLayerOptions>>();
                    o.Value.Events.CustomizeTypesInfo?.Invoke(typesInfo, s);
                    return typesInfo;
            });
            

            services.AddScoped<IObjectSpaceFactory, T>();

            services.AddScoped<SecurityStandartAuthenticationService>();
            services.AddScoped<ISecurityProvider, SecurityProvider>();
            services.AddScoped<PrincipalAuthenticationService>();
            services.AddScoped<IPrincipalProvider, PrincipalProvider>();
            services.AddScoped(serviceProvider => (IPrincipalProviderInitializer)serviceProvider.GetRequiredService<IPrincipalProvider>());

            return services;
        }
    }
}
