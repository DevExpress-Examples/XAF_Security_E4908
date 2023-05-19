using MAUI.ViewModels;

namespace MAUI.Services {
	public interface INavigationService {
		Task NavigateToAsync<TViewModel>() where TViewModel : BaseViewModel;

		Task NavigateToAsync<TViewModel>(bool isAbsoluteRoute) where TViewModel : BaseViewModel;

		Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : BaseViewModel;

		Task GoBackAsync();
	}
}