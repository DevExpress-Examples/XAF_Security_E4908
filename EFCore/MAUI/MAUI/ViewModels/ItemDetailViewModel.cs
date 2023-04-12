using System.Web;
using MAUI.Models;

namespace MAUI.ViewModels {
	public class ItemDetailViewModel : BaseViewModel, IQueryAttributable {
		public const string ViewName = "ItemDetailPage";
		string _content;

		public ItemDetailViewModel() => ArchiveCommand = new Command(OnArchive);

		private async void OnArchive(object obj) {
			if (!await DataStore.UserCanCreatePostAsync()) {
				await Shell.Current.DisplayAlert("Error", "Permission denied", "OK");
			}
			else {
				await DataStore.ArchivePostAsync(Post);
			}
		}

		public Command ArchiveCommand { get; }

		public ImageSource Thumbnail 
			=> ImageSource.FromStream(() => new MemoryStream(_photoBytes));

		public Guid Id { get; set; }
		public Post Post { get; set; }
		string _title;
		private string _photo;
		private byte[] _photoBytes;

		public new string Title {
			get => _title;
			set => SetProperty(ref _title, value);
		}

		public string Content {
			get => _content;
			set => SetProperty(ref _content, value);
		}
		public string Photo {
			get => _photo;
			set => SetProperty(ref _photo, value);
		}

		public async Task LoadItemId(string itemId) {
			try {
				_photoBytes = await DataStore.GetAuthorPhotoAsync(Guid.Parse(itemId));
				OnPropertyChanged(nameof(Thumbnail));
				Post = await DataStore.GetItemAsync(itemId);
				Id = Post.ID;
				Title = Post.Title;
				Content = Post.Content;
				
			}
			catch (Exception e) {
				System.Diagnostics.Debug.WriteLine($"Failed to Load Post {e}");
			}
		}

		public override async Task InitializeAsync(object parameter) 
			=> await LoadItemId(parameter as string);

		public async void ApplyQueryAttributes(IDictionary<string, object> query) 
			=> await LoadItemId(HttpUtility.UrlDecode(query["id"] as string));
	}
}