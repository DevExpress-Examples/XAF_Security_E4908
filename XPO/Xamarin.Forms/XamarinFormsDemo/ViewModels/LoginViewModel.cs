using System;
using Xamarin.Forms;

using XamarinFormsDemo.Services;
using XamarinFormsDemo.Views;

namespace XamarinFormsDemo.ViewModels {
    public class LoginViewModel : BaseViewModel {
        public Command LogInCommand { get; }

        public LoginViewModel() {
            Password = "";
            UserName = "";
            LogInCommand = new Command(OnLoginClicked);
        }
        string userName;
        public string UserName {
            get { return userName; }
            set { SetProperty(ref userName, value); }
        }

        string password;
        public string Password {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        
        private async void OnLoginClicked(object obj) {
            try {
                XpoHelper.Logon(UserName, Password);
                await Shell.Current.GoToAsync($"//{nameof(ItemsPage)}");
            } catch(Exception ex) {
                await Shell.Current.DisplayAlert("Login failed", ex.Message, "Try again");
            }
        }

    }
}
