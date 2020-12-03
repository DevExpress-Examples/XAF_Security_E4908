using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using XamarinFormsDemo.ViewModels;
using XamarinFormsDemo;


namespace XamarinFormsDemo.Views {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage {
        LoginViewModel viewModel;
        public LoginPage() {
            InitializeComponent();

            BindingContext = this.viewModel = new LoginViewModel();
        }

        async void OnLoginClicked(object sender, EventArgs e) {
            try {
                XpoHelper.InitXpo("https://10.0.2.2:5001/xpo/", viewModel.Login, viewModel.Password);
                await Navigation.PopAsync();
                Application.Current.MainPage = new NavigationPage(new MainPage());
            } catch (Exception ex){
                await DisplayAlert("Login failed", ex.Message, "Try again");
            }
        }
    }
}