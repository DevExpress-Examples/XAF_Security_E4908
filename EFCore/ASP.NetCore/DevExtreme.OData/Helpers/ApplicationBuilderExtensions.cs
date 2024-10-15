﻿using DatabaseUpdater;
using DevExpress.ExpressApp;

namespace Microsoft.Extensions.DependencyInjection {
    public static class ApplicationBuilderExtensions {
        public static WebApplication UseDemoData(this WebApplication app) {
            using var scope = app.Services.CreateScope();
            var updatingObjectSpaceFactory = scope.ServiceProvider.GetRequiredService<IUpdatingObjectSpaceFactory>();
            using var objectSpace = updatingObjectSpaceFactory
                .CreateUpdatingObjectSpace(typeof(BusinessObjectsLibrary.BusinessObjects.Employee), true);
            new Updater(objectSpace).UpdateDatabase();
            return app;
        }
    }
}
