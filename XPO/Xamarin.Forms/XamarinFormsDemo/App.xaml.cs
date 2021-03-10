using DevExpress.Persistent.Base;

using System;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinFormsDemo.Services;

namespace XamarinFormsDemo {
    public partial class App : Application {

        public App() {
            InitializeComponent();

            //..
            Tracing.UseConfigurationManager = false;
            Tracing.Initialize(3);
            //..
            if(!XpoHelper.Security.IsAuthenticated) {
                ResetMainPage();
            } else {
                MainPage = new AppShell();
            }
        }

        protected override void OnStart() {
        }

        protected override void OnSleep() {
        }

        protected override void OnResume() {
        }
        public static Task ResetMainPage() {
            Current.MainPage = new AppShell();
            return Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
