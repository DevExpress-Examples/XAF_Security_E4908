using System;
using System.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Security.Strategy;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Base;

namespace Security_Integrated_Console.Win {
    static class Program {
        private static void UpdateDatabase(IObjectSpace objectSpace) {
            PermissionPolicyUser userAdmin = objectSpace.CreateObject<PermissionPolicyUser>();
            userAdmin.UserName = "Admin";
            userAdmin.SetPassword("");
            PermissionPolicyRole adminRole = objectSpace.CreateObject<PermissionPolicyRole>();
            adminRole.IsAdministrative = true;
            userAdmin.Roles.Add(adminRole);

            PermissionPolicyUser userJohn = objectSpace.CreateObject<PermissionPolicyUser>();
            userJohn.UserName = "User";
            PermissionPolicyRole userRole = objectSpace.FindObject<PermissionPolicyRole>(new BinaryOperator("Name", "Users"));
            userRole = objectSpace.CreateObject<PermissionPolicyRole>();
            userRole.AddObjectPermission<Person>(SecurityOperations.Read, "[FirstName] == 'User person'", SecurityPermissionState.Allow);
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
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));

            Console.WriteLine("Update database...");
            DataSet dataSet = new DataSet();
            Console.WriteLine("Database has been updated successfully.");

            XPObjectSpaceProvider directProvider = new XPObjectSpaceProvider(new MemoryDataStoreProvider(dataSet));
            IObjectSpace directObjectSpace = directProvider.CreateObjectSpace();
            UpdateDatabase(directObjectSpace);

            AuthenticationStandard auth = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
            SecuritySystem.SetInstance(security);
            SecuredObjectSpaceProvider osProvider = new SecuredObjectSpaceProvider(security, new MemoryDataStoreProvider(dataSet));
            IObjectSpace securedObjectSpace = osProvider.CreateObjectSpace();

            auth.SetLogonParameters(new AuthenticationStandardLogonParameters("User", ""));
            Console.WriteLine("Logging 'User' user...");
            security.Logon(directObjectSpace);
            Console.WriteLine("'User' is logged on.");   
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
