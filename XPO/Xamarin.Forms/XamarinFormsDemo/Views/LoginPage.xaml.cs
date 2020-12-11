using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinFormsDemo.ViewModels;


namespace XamarinFormsDemo.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage {
        public LoginPage() {
            InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}