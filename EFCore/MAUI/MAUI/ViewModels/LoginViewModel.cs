using MAUI.Services;
using System.Threading.Channels;
using static System.Net.Mime.MediaTypeNames;

namespace MAUI.ViewModels {
    public class LoginViewModel : BaseViewModel {
        string userName;
        string password = string.Empty;
        string errorText;
        bool hasError;
        bool isAuthInProcess;

        public LoginViewModel() {
            LoginCommand = new Command(OnLoginClicked);
            SignUpCommand = new Command(OnSignUpClicked);
            PropertyChanged +=
                (_, __) => LoginCommand.ChangeCanExecute();

        }

        public string UserName {
            get => userName;
            set => SetProperty(ref userName, value);
        }

        public string Password {
            get => password;
            set => SetProperty(ref password, value);
        }

        public string ErrorText {
            get => errorText;
            set => SetProperty(ref errorText, value);
        }

        public bool HasError {
            get => hasError;
            set => SetProperty(ref hasError, value);
        }

        public bool IsAuthInProcess {
            get => isAuthInProcess;
            set => SetProperty(ref isAuthInProcess, value);
        }

        public Command LoginCommand { get; }
        public Command SignUpCommand { get; }

        async void OnLoginClicked() {
            IsAuthInProcess = true;
            var response = await ((WebAPIService)DataStore).Authenticate(userName, password);
            IsAuthInProcess = false;
            if (!string.IsNullOrEmpty(response)) {
                ErrorText = response;
                HasError = true;
                return;
            }
            HasError = false;
            await Navigation.NavigateToAsync<ItemsViewModel>();
        }
        

        async void OnSignUpClicked() 
	        => await Shell.Current.DisplayAlert("Sign up", "Please ask your system administrator to register you in the corporate system", "OK");
    }
}