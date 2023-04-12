using MAUI.ViewModels;

namespace MAUI.Views {
	public partial class MainPage {
		public MainPage() {
			InitializeComponent();
			BindingContext = new MainViewModel();
		}
	}
}