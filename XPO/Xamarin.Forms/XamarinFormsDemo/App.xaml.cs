using DevExpress.Persistent.Base;
using DevExpress.Xpo.DB;
using System;
using System.Diagnostics;
using System.IO;
using Xamarin.Forms;
using XamarinFormsDemo.Views;

namespace XamarinFormsDemo {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Tracing.UseConfigurationManager = false;
            Tracing.Initialize(3);

            if(Device.RuntimePlatform == Device.iOS)
                MainPage = new LoginPage();
            else
                MainPage = new NavigationPage(new LoginPage());
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e) {
            Debug.WriteLine(e.ExceptionObject);
        }
    }
}
