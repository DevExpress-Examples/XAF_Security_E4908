namespace MAUI.Services {
	public interface IDataStore<T> {
		Task<bool> AddItemAsync(T item);

		Task<T> GetItemAsync(string id);

		Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

		Task<bool> UserCanCreatePostAsync();

		Task<byte[]> GetAuthorPhotoAsync(Guid postId);

		Task ArchivePostAsync(T post);

		Task ShapeIt();
	}
}