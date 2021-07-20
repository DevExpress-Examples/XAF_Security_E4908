using BusinessObjectsLibrary;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo.Metadata;
using System;
using System.Configuration;

namespace DatabaseUpdater {
    class Program {
        static void Main() {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
            XPObjectSpaceProvider objectSpaceProvider = new XPObjectSpaceProvider(connectionString);

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
