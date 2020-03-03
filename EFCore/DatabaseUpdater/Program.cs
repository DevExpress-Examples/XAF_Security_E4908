using BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;

namespace DatabaseUpdater {
    class Program {
        static void Main(string[] args) {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            EFCoreObjectSpaceProvider objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(ConsoleDbContext), XafTypesInfo.Instance, connectionString,
             (builder, cs) =>
             builder.UseSqlServer(cs).
             UseLazyLoadingProxies());

            RegisterEntities();

            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;

            Console.WriteLine("Starting database update...");

            using (IObjectSpace objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                Updater updater = new Updater(objectSpace);
                updater.UpdateDatabase();
            }

            Console.WriteLine("Database is updated. Press any key to close.");
            Console.ReadKey();
        }
        private static void RegisterEntities() {
            XafTypesInfo.Instance.RegisterEntity(typeof(Person));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}
