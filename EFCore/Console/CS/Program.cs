using BusinessObjectsLibrary.EFCore.NetCore;
using BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.IO;
using System.Text;

namespace ConsoleApplication {
    class Program {
        static void Main() {
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(
                security, typeof(ApplicationDbContext), 
                XafTypesInfo.Instance, connectionString,
                (builder, connectionString) => builder.UseSqlServer(connectionString)
            );

            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;
            string userName = "User";
            string password = string.Empty;
            authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            IObjectSpace loginObjectSpace = objectSpaceProvider.CreateNonsecuredObjectSpace();
            try {
                security.Logon(loginObjectSpace);
            }
            catch(SqlException sqlEx) {
                if(sqlEx.Number == 4060) {
                    throw new Exception(sqlEx.Message + Environment.NewLine + MessageHelper.OpenDatabaseFailed, sqlEx);
                }
            }

            using(StreamWriter file = new StreamWriter("result.txt", false)) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"{userName} is logged on.\n");
                stringBuilder.Append("List of the 'Employee' objects:\n");
                using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
                    foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
                        stringBuilder.Append("=========================================\n");
                        stringBuilder.Append($"Full Name: {employee.FullName}\n");
                        if(security.CanRead(securedObjectSpace, employee, nameof(Employee.Department))) {
                            stringBuilder.Append($"Department: {employee.Department.Title}\n");
                        }
                        else {
                            stringBuilder.Append("Department: [Protected content]\n");
                        }
                    }
                }
                file.Write(stringBuilder);
            }
            Console.WriteLine(string.Format(@"The result.txt file has been created in the {0} directory.", Environment.CurrentDirectory));
            Console.WriteLine("Press any key to close a the console...");
            Console.ReadLine();
        }
    }
}