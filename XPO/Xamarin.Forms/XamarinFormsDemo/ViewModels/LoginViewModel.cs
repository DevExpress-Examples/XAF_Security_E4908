using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.Views;

namespace XamarinFormsDemo.ViewModels {
    public class LoginViewModel : INotifyPropertyChanged {
        public string Login {
            get { return login; }
            set { SetProperty(ref login, value); LogInCommand.ChangeCanExecute(); }
        }
        string login;
        public string Password {
            get { return password; }
            set { SetProperty(ref password, value); }
        }
        string password;

        public Command LogInCommand { get; private set; }
        void ExecuteLogInCommand() {
            try {
                if (IsBusy) {
                    return;
                }
                IsBusy = true;
                XpoHelper.InitXpo(WebApiDataStoreClient.GetConnectionString("https://10.0.2.2:5001/xpo/"), Login, Password);
                IsBusy = false;
                if(Device.RuntimePlatform == Device.iOS)
                    Application.Current.MainPage = new MainPage();
                else
                    Application.Current.MainPage = new NavigationPage(new MainPage());
            } catch(Exception ex) {
                Application.Current.MainPage.DisplayAlert("Login failed", ex.Message, "Try again");
            }
        }

        public LoginViewModel() {
            Title = "DevExpress XAF Security Demo";
            LogInCommand = new Command(() => ExecuteLogInCommand(), ()=>Login?.Length>0);
        }

        bool isBusy = false;
        public bool IsBusy {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        string title = string.Empty;
        public string Title {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null) {
            if(EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
            var changed = PropertyChanged;
            if(changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
