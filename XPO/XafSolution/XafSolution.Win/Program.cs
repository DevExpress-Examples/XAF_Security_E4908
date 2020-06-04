using System;
using System.Configuration;
using System.Windows.Forms;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraEditors;

namespace XafSolution.Win
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if EASYTEST
            DevExpress.ExpressApp.Win.EasyTest.EasyTestRemotingRegistration.Register();
#endif
            WindowsFormsSettings.LoadApplicationSettings();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if NETCOREAPP
            DevExpress.ExpressApp.BaseObjectSpace.ThrowExceptionForNotRegisteredEntityType = true;
#else
            EditModelPermission.AlwaysGranted = System.Diagnostics.Debugger.IsAttached;
#endif
            if (Tracing.GetFileLocationFromSettings() == DevExpress.Persistent.Base.FileLocation.CurrentUserApplicationDataFolder) {
                Tracing.LocalUserAppDataPath = Application.LocalUserAppDataPath;
            }
            Tracing.Initialize();
            XafSolutionWindowsFormsApplication winApplication = new XafSolutionWindowsFormsApplication();
            winApplication.LastLogonParametersReading += winApplication_LastLogonParametersReading;
            SecurityStrategy security = (SecurityStrategy)winApplication.Security;
            security.RegisterXPOAdapterProviders();
            if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                winApplication.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && winApplication.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                winApplication.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
            try {
                winApplication.Setup();
                winApplication.Start();
            }
            catch(Exception e) {
#if NETCOREAPP
                winApplication.StopSplash();
#endif
                winApplication.HandleException(e);
            }
        }
        static void winApplication_LastLogonParametersReading(object sender, LastLogonParametersReadingEventArgs e)
        {
            if (string.IsNullOrEmpty(e.SettingsStorage.LoadOption("", nameof(AuthenticationStandardLogonParameters.UserName))))
            {
                // This user is created in the XafSolution.Module\DatabaseUpdate\Updater.cs file.
                e.SettingsStorage.SaveOption("", nameof(AuthenticationStandardLogonParameters.UserName), "Admin");
            }
        }
    }
}
