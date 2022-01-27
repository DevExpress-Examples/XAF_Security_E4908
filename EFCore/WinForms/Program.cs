using System.Configuration;
using Microsoft.EntityFrameworkCore;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using BusinessObjectsLibrary.BusinessObjects;
using DatabaseUpdater;
using DevExpress.ExpressApp.DC;

namespace WindowsFormsApplication {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            TypesInfo typesInfo = new TypesInfo();
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CreateDemoData(connectionString, typesInfo);

            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(
                typeof(PermissionPolicyUser), typeof(PermissionPolicyRole),
                authentication,
                typesInfo
            );
            SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, typeof(ApplicationDbContext),
                typesInfo, connectionString, (builder, connectionString) => builder.UseSqlServer(connectionString));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm(security, objectSpaceProvider);
            Application.Run(mainForm);
        }
        private static void CreateDemoData(string connectionString, TypesInfo typesInfo) {
            using (var objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(ApplicationDbContext), typesInfo, connectionString,
        (builder, connectionString) => builder.UseSqlServer(connectionString)))
            using (var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                new Updater(objectSpace).UpdateDatabase();
            }
        }
    }
}
