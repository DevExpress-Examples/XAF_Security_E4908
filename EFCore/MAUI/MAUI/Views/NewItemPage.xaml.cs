using MAUI.ViewModels;

namespace MAUI.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewItemPage {
		public NewItemPage() {
			InitializeComponent();
			BindingContext = new NewItemViewModel();
		}
	}
}