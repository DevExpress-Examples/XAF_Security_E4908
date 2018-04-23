using System;
using System.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace Security_Integrated_Console.Win {
    static class Program {
        private static void UpdateDatabase(IObjectSpace objectSpace) {
            SecuritySystemUser userAdmin = objectSpace.CreateObject<SecuritySystemUser>();
            userAdmin.UserName = "Admin";
            userAdmin.SetPassword("");
            SecuritySystemRole adminRole = objectSpace.CreateObject<SecuritySystemRole>();
            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);

            SecuritySystemUser userJohn = objectSpace.CreateObject<SecuritySystemUser>();
            userJohn.UserName = "User";
            SecuritySystemRole userRole = objectSpace.FindObject<SecuritySystemRole>(new BinaryOperator("Name", "Users"));
            userRole = objectSpace.CreateObject<SecuritySystemRole>();
            userRole.AddObjectAccessPermission<Person>("[FirstName] == 'User person'", SecurityOperations.Read);
            userJohn.Roles.Add(userRole);

            Person adminPerson = objectSpace.FindObject<Person>(new BinaryOperator("FirstName", "Person for Admin"));
            adminPerson = objectSpace.CreateObject<Person>();
            adminPerson.FirstName = "Admin person";

            Person userPerson = objectSpace.CreateObject<Person>();
            userPerson.FirstName = "User person";
            objectSpace.CommitChanges();
        }

        [STAThread]
        static void Main() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Person));
            XafTypesInfo.Instance.RegisterEntity(typeof(SecuritySystemUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(SecuritySystemRole));

            Console.WriteLine("Update database...");
            DataSet dataSet = new DataSet();
            Console.WriteLine("Database has been updated successfully.");
            XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(new MemoryDataStoreProvider(dataSet));
            IObjectSpace directObjectSpace = directProvider.CreateObjectSpace();
            UpdateDatabase(directObjectSpace);

            AuthenticationStandard auth = new AuthenticationStandard();
            auth.SetLogonParameters(new AuthenticationStandardLogonParameters("User", ""));

            SecurityStrategyComplex security = new SecurityStrategyComplex(
                typeof(SecuritySystemUser), typeof(DevExpress.ExpressApp.Security.Strategy.SecuritySystemRole), auth);
            SecuritySystem.SetInstance(security);
            Console.WriteLine("Logging 'User' user...");
            security.Logon(directObjectSpace);
            Console.WriteLine("'User' is logged on.");

            SecuredObjectSpaceProvider osProvider = new SecuredObjectSpaceProvider(security, new MemoryDataStoreProvider(dataSet));
            IObjectSpace securedObjectSpace = osProvider.CreateObjectSpace();
       
            Console.WriteLine("List of the 'Person' objects:");
            foreach (Person person in securedObjectSpace.GetObjects<Person>()) {
                Console.WriteLine(person.FirstName);
            }
            auth.SetLogonParameters(new AuthenticationStandardLogonParameters("Admin", ""));
            Console.WriteLine("Logging 'Admin' user...");
            security.Logon(directObjectSpace);
            Console.WriteLine("Admin is logged on.");

            securedObjectSpace = osProvider.CreateObjectSpace();
            Console.WriteLine("List of the 'Person' objects:");
            foreach (Person person in securedObjectSpace.GetObjects<Person>()) {
                Console.WriteLine(person.FirstName);
            }

            Console.WriteLine("Press enter to exit...");
            Console.ReadLine();
        }
    }
}
