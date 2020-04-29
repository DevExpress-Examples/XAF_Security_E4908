using BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects;
using DevExpress.EntityFrameworkCore.Security;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using System;
using System.Configuration;
using System.Windows.Forms;

namespace WindowsFormsApplication {
    static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main() {
            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;
            AuthenticationStandard authentication = new AuthenticationStandard();
            SecurityStrategyComplex security = new SecurityStrategyComplex(
                typeof(PermissionPolicyUser), typeof(PermissionPolicyRole),
                authentication
            );
            SecuredEFCoreObjectSpaceProvider objectSpaceProvider = new SecuredEFCoreObjectSpaceProvider(
                security, typeof(ApplicationDbContext),
                XafTypesInfo.Instance, ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString,
                (builder, connectionString) => builder.UseSqlServer(connectionString)
            );

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm(security, objectSpaceProvider);
            Application.Run(mainForm);
        }
    }
}
