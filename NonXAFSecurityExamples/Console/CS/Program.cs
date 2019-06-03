using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
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
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            AuthenticationStandard auth = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
			SecurityAdapterHelper.Enable();
            SecuredObjectSpaceProvider osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);
            IObjectSpace nonSecuredObjectSpace = osProvider.CreateNonsecuredObjectSpace();

			DevExpress.Persistent.Base.PasswordCryptographer.EnableRfc2898 = true;
			DevExpress.Persistent.Base.PasswordCryptographer.SupportLegacySha512 = false;

			string userName = "User";
            string password = "";
            auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            security.Logon(nonSecuredObjectSpace);
            using(StreamWriter file = new StreamWriter("result.txt", false)) {
                StringBuilder stringBuilderb = new StringBuilder();
                stringBuilderb.Append(string.Format("{0} is logged on.\n", userName));
                IObjectSpace securedObjectSpace = osProvider.CreateObjectSpace();
                stringBuilderb.Append("List of the 'Employee' objects:\n");
                foreach(Employee employee in securedObjectSpace.GetObjects<Employee>()) {
                    stringBuilderb.Append("=========================================\n");
                    stringBuilderb.Append(string.Format("Full name: {0}\n", employee.FullName));
                    if(security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Employee), SecurityOperations.Read, employee, "Department"))) {
                        stringBuilderb.Append(string.Format("Department: {0}\n", employee.Department.Title));
                    }
                    else {
                        stringBuilderb.Append("Department: [Protected content]\n");
                    }
                }
                file.Write(stringBuilderb);
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