using System.Collections.ObjectModel;
using MAUI.Models;

namespace MAUI.ViewModels {
	public class ItemsViewModel : BaseViewModel {
		Post _selectedPost;

		public ItemsViewModel() {
			Title = "Browse";
			Items = new ObservableCollection<Post>();
			LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
			ItemTapped = new Command<Post>(OnItemSelected);
			AddItemCommand = new Command(OnAddItem);
			ShapeItCommand = new Command(async () => await DataStore.ShapeIt());
		}
		
		public ObservableCollection<Post> Items { get; }

		public Command LoadItemsCommand { get; }
		public Command ShapeItCommand { get; }

		public Command AddItemCommand { get; }

		public Command<Post> ItemTapped { get; }

		public Post SelectedPost {
			get => _selectedPost;
			set { 
				SetProperty(ref _selectedPost, value);
				OnItemSelected(value);
			}
		}

		async void OnAddItem(object obj) {
			if (await DataStore.UserCanCreatePostAsync()) {
				await Navigation.NavigateToAsync<NewItemViewModel>(null);
			}
			else {
				await Shell.Current.DisplayAlert("Error", "Access denied", "Ok");
			}
			
		}

		public async Task OnAppearing() {
			IsBusy = true;
			SelectedPost = null;
			await ExecuteLoadItemsCommand();
		}

		async Task ExecuteLoadItemsCommand() {
			IsBusy = true;
			try
			{
				Items.Clear();
				var items = await DataStore.GetItemsAsync(true);
				foreach (var item in items) {
					Items.Add(item);
				}
			}
			catch (Exception ex) {
				System.Diagnostics.Debug.WriteLine(ex);
			}
			finally {
				IsBusy = false;
			}
		}

		async void OnItemSelected(Post post) {
			if (post != null) await Navigation.NavigateToAsync<ItemDetailViewModel>(post.ID);
		}
	}
}