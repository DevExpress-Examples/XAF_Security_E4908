using System.ComponentModel;
using System.Runtime.CompilerServices;
using MAUI.Models;
using MAUI.Services;

namespace MAUI.ViewModels {
	public class BaseViewModel : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;
		bool _isBusy;
		string _title = string.Empty;
		public IDataStore<Post> DataStore => DependencyService.Get<IDataStore<Post>>();
		public INavigationService Navigation => DependencyService.Get<INavigationService>();

		public bool IsBusy {
			get => _isBusy;
			set => SetProperty(ref _isBusy, value);
		}

		public string Title {
			get => _title;
			set => SetProperty(ref _title, value);
		}

		public virtual Task InitializeAsync(object parameter) => Task.CompletedTask;

		protected bool SetProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "", Action onChanged = null) {
			if (EqualityComparer<T>.Default.Equals(backingStore, value))
				return false;

			backingStore = value;
			onChanged?.Invoke();
			OnPropertyChanged(propertyName);
			return true;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
			var changed = PropertyChanged;
			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

	}
}