using System.Configuration;
using Microsoft.EntityFrameworkCore;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp.EFCore;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using BusinessObjectsLibrary.BusinessObjects;
using DatabaseUpdater;

namespace WindowsFormsApplication {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            CreateDemoData(connectionString);

            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(
                typeof(PermissionPolicyUser), typeof(PermissionPolicyRole),
                authentication
            );
            SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(security, typeof(ApplicationDbContext),
                (builder, _) => builder.UseSqlServer(connectionString));

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm(security, objectSpaceProvider);
            Application.Run(mainForm);
        }
        private static void CreateDemoData(string connectionString) {
            using(var objectSpaceProvider = new EFCoreObjectSpaceProvider(typeof(ApplicationDbContext), (builder, _) => builder.UseSqlServer(connectionString)))
            using(var objectSpace = objectSpaceProvider.CreateUpdatingObjectSpace(true)) {
                new Updater(objectSpace).UpdateDatabase();
            }
        }
    }
}
