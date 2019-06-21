using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using XafSolution.Module.BusinessObjects;

namespace ConsoleApplication {
    class Program {
        static void Main() {
            RegisterEntities();
            AuthenticationStandard auth = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
            security.RegisterXPOAdapterProviders();

            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SecuredObjectSpaceProvider objectSpaceProvider = new SecuredObjectSpaceProvider(security, connectionString, null);

            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;

            string userName = "User";
            string password = string.Empty;
            auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
            security.Logon(loginObjectSpace);
            
            using(StreamWriter file = new StreamWriter("result.txt", false)) {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append($"{userName} is logged on.\n");
                stringBuilder.Append("List of the 'Employee' objects:\n");
                using(IObjectSpace securedObjectSpace = objectSpaceProvider.CreateObjectSpace()) {
                    foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
                        stringBuilder.Append("=========================================\n");
                        stringBuilder.Append($"Full name: {employee.FullName}\n");
                        if(security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, employee, nameof(Department)))) {
                            stringBuilder.Append($"Department: {employee.Department.Title}\n");
                        }
                        else {
                            stringBuilder.Append("Department: [Protected content]\n");
                        }
                    } 
                }
                file.Write(stringBuilder);
            }
        }
        private static void RegisterEntities() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(Person));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}