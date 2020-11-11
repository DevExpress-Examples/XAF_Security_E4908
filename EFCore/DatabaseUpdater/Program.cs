using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.Persistent.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace DatabaseUpdater.EFCore {
    class Program {
        static void Main() {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            EFCoreObjectSpaceProvider objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(ApplicationDbContext),
                (builder, _) => builder.UseSqlServer(connectionString));

            Console.WriteLine("Starting database update...");

            using(IObjectSpace objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                Updater updater = new Updater(objectSpace);
                updater.UpdateDatabase();
            }

            Console.WriteLine("Database is updated. Press any key to close.");
            Console.ReadKey();
        }
    }
}
