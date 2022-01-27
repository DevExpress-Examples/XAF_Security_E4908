using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp.EFCore;
using DatabaseUpdater;
using DevExpress.ExpressApp.DC;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData<TContext>(this WebApplication app, string connectionString, EFCoreDatabaseProviderHandler databaseProviderHandler) where TContext : DbContext {
            ITypesInfo typesInfo = app.Services.GetRequiredService<ITypesInfo>();
            using (var objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(TContext), typesInfo, connectionString, databaseProviderHandler))
            using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                new Updater(objectSpace).UpdateDatabase();
            }
            return app;
        }
    }
}
