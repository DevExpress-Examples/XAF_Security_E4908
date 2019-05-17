using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using MainDemo.Module.BusinessObjects;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

namespace NonXAFSecurityConsoleApp {
    class Program {
        static void Main() {
            RegisterEntities();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            AuthenticationStandard auth = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), auth);
            security.RegisterXPOAdapterProviders();
            SecuredObjectSpaceProvider osProvider = new SecuredObjectSpaceProvider(security, connectionString, null);

            string userName = "John";
            string password = "";
            auth.SetLogonParameters(new AuthenticationStandardLogonParameters(userName, password));
            security.Logon(osProvider.CreateNonsecuredObjectSpace());
            using(StreamWriter file = new StreamWriter("result.txt", false)) {
                StringBuilder stringBuilderb = new StringBuilder();
                stringBuilderb.Append(string.Format("{0} is logged on.\n", userName));
                IObjectSpace securedObjectSpace = osProvider.CreateObjectSpace();
                stringBuilderb.Append("List of the 'Contact' objects:\n");
                foreach(Contact contact in securedObjectSpace.GetObjects<Contact>()) {
                    stringBuilderb.Append("=========================================\n");
                    stringBuilderb.Append(string.Format("Full name: {0}\n", contact.FullName));
                    if(security.IsGranted(new PermissionRequest(securedObjectSpace, typeof(Contact), SecurityOperations.Read, contact, "Department"))) {
                        stringBuilderb.Append(string.Format("Department: {0}\n", contact.Department.Title));
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
            XafTypesInfo.Instance.RegisterEntity(typeof(Contact));
            XafTypesInfo.Instance.RegisterEntity(typeof(Person));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
    }
}