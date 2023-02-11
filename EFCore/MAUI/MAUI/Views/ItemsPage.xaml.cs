using MAUI.ViewModels;

namespace MAUI.Views {
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage {
		public ItemsPage() {
			InitializeComponent();
			BindingContext = ViewModel = new ItemsViewModel();
		}

		ItemsViewModel ViewModel { get; }

		protected override void OnAppearing() {
			base.OnAppearing();
#pragma warning disable CS4014
			ViewModel.OnAppearing();
#pragma warning restore CS4014
		}
	}
}