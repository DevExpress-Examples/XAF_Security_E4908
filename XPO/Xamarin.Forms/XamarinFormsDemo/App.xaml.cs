using DevExpress.Persistent.Base;
using System;
using Xamarin.Forms;
using XamarinFormsDemo.Views;

namespace XamarinFormsDemo {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            Tracing.UseConfigurationManager = false;
            Tracing.Initialize(3);

            if(Device.RuntimePlatform == Device.iOS)
                MainPage = new LoginPage();
            else
                MainPage = new NavigationPage(new LoginPage());
        }
    }
}
