using MAUI.ViewModels;

namespace MAUI.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemDetailPage {
		public ItemDetailPage() {
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}