using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinFormsDemo.ViewModels {
    public class AboutViewModel : BaseViewModel {
        public AboutViewModel():base(null) {
            Title = "About";
            OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));
        }
        public ICommand OpenWebCommand { get; }
    }
}