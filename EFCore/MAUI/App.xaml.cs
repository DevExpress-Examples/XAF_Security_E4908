using MAUI.Services;
using MAUI.ViewModels;
using MAUI.Views;


namespace MAUI {
	public partial class App {
		public App() {
			InitializeComponent();
			DependencyService.Register<NavigationService>();
			DependencyService.Register<WebAPIService>();

			Routing.RegisterRoute(typeof(ItemDetailPage).FullName, typeof(ItemDetailPage));
			Routing.RegisterRoute(typeof(NewItemPage).FullName, typeof(NewItemPage));
			Routing.RegisterRoute(typeof(ItemsPage).FullName, typeof(ItemsPage));
			
			MainPage = new MainPage();
			var navigationService = DependencyService.Get<INavigationService>();
			navigationService.NavigateToAsync<LoginViewModel>(true);
		}
	}
}
