using System.Net.Http.Headers;
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
            var authToken = SecureStorage.GetAsync("auth_token").Result;
            if (!string.IsNullOrEmpty(authToken)) {
                WebAPIService.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", authToken);
                navigationService.NavigateToAsync<ItemsViewModel>(true);
            }
            else
                navigationService.NavigateToAsync<LoginViewModel>(true);

        }
	}
}
