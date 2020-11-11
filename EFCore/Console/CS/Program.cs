using BusinessObjectsLibrary.EFCore.BusinessObjects;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Diagnostics;

namespace ConsoleApplication {
    // ## Prerequisites. 
    // 1) Add the 'DevExpress.ExpressApp.EFCore' and 'Microsoft.EntityFrameworkCore*' NuGet packages; 
    // 2) Define your ORM data model and DbContext (explore the 'BusinessObjectsLibrary' project);
    // 3) Create a database with user, role and permission data (run the 'DatabaseUpdater' project).
    class Program {
        static void Main() {
            // ## Step 1. Initialization. Create a Secured Data Store and Set Authentication Options
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(
                typeof(PermissionPolicyUser), typeof(PermissionPolicyRole),
                authentication
            );
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, typeof(ApplicationDbContext),
                (builder, _) => builder.UseSqlServer(connectionString));

            // ## Step 2. Authentication. Log in as a 'User' with an Empty Password
            authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName: "User", password: string.Empty));
            IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
            try {
                security.Logon(loginObjectSpace);
            }
            catch(SqlException sqlEx) {
                if(sqlEx.Number == 4060) {
                    throw new Exception(sqlEx.Message + Environment.NewLine + ApplicationDbContext.DatabaseConnectionFailedMessage, sqlEx);
                }
            }

            // ## Step 3. Authorization. Access and Manipulate Data/UI Based on User/Role Rights
            Console.WriteLine($"{"Full Name",-40}{"Department",-40}");
            using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
                // User cannot read protected entities like PermissionPolicyRole.
                Debug.Assert(securedObjectSpace.GetObjects<PermissionPolicyRole>().Count == 0);
                foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) { // User can read Employee data.
                    // User can read Department data by criteria.
                    bool canRead = security.CanRead(securedObjectSpace, employee, memberName: nameof(Employee.Department));
                    Debug.Assert(!canRead == (employee.Department == null));
                    // Mask protected property values when User has no 'Read' permission.
                    var department = canRead ? employee.Department.Title : "*******";
                    Console.WriteLine($"{employee.FullName,-40}{department,-40}");
                }
            }
            security.Logoff();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}