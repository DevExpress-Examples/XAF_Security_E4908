using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using DevExpress.Maui.Core.Internal;
using MAUI.Models;

namespace MAUI.Services;
public static partial class HttpMessageHandler {
    static readonly System.Net.Http.HttpMessageHandler PlatformHttpMessageHandler;
    public static System.Net.Http.HttpMessageHandler GetMessageHandler() => PlatformHttpMessageHandler;
}

public class WebAPIService : IDataStore<Post> {
    private static readonly HttpClient HttpClient = new(HttpMessageHandler.GetMessageHandler()) { Timeout = new TimeSpan(0, 0, 10) };
    private readonly string _apiUrl = ON.Platform(android:"https://10.0.2.2:5001/api/", iOS:"https://localhost:5001/api/");
    private readonly string _postEndPointUrl;
    private const string ApplicationJson = "application/json";

    public WebAPIService()
        => _postEndPointUrl = _apiUrl + "odata/" + nameof(Post);

    public async Task<bool> UserCanCreatePostAsync()
        => (bool)JsonNode.Parse(await HttpClient.GetStringAsync($"{_apiUrl}CustomEndpoint/CanCreate?typename=Post"));

    public async Task<byte[]> GetAuthorPhotoAsync(int postId)
        => await HttpClient.GetByteArrayAsync($"{_apiUrl}CustomEndPoint/AuthorPhoto/{postId}");

    public async Task ArchivePostAsync(Post post) {
        var httpResponseMessage = await HttpClient.PostAsync($"{_apiUrl}CustomEndPoint/Archive", new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, ApplicationJson));
        if (httpResponseMessage.IsSuccessStatusCode) {
            await Shell.Current.DisplayAlert("Success", "This post is saved to disk", "Ok");
        }
        else {
            await Shell.Current.DisplayAlert("Error", await httpResponseMessage.Content.ReadAsStringAsync(), "Ok");
        }
    }

    public async Task ShapeIt() {
        var bytes = await HttpClient.GetByteArrayAsync($"{_apiUrl}report/DownloadByName(Post Report)");
#if ANDROID
		var fileName = $"{FileSystem.Current.AppDataDirectory}/Report.pdf";
		await File.WriteAllBytesAsync(fileName, bytes);
		var intent = new Android.Content.Intent(Android.Content.Intent.ActionView);
		intent.SetDataAndType(AndroidX.Core.Content.FileProvider.GetUriForFile(Android.App.Application.Context,
			$"{Android.App.Application.Context.ApplicationContext?.PackageName}.provider",new Java.IO.File(fileName)),"application/pdf");
		intent.SetFlags(Android.Content.ActivityFlags.ClearWhenTaskReset | Android.Content.ActivityFlags.NewTask | Android.Content.ActivityFlags.GrantReadUriPermission);
		Android.App.Application.Context.ApplicationContext?.StartActivity(intent);
#else
        var path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        var fileName = $"{path}/Report.pdf";
        await File.WriteAllBytesAsync(fileName, bytes);
        var filePath = Path.Combine(path, "Report.pdf");
        var viewer = UIKit.UIDocumentInteractionController.FromUrl(Foundation.NSUrl.FromFilename(filePath));
        viewer.PresentOpenInMenu(new System.Drawing.RectangleF(0, -260, 320, 320), Platform.GetCurrentUIViewController()!.View!, true);
#endif
    }

    public async Task<bool> AddItemAsync(Post post) {
        var httpResponseMessage = await HttpClient.PostAsync(_postEndPointUrl,
            new StringContent(JsonSerializer.Serialize(post), Encoding.UTF8, ApplicationJson));
        if (!httpResponseMessage.IsSuccessStatusCode) {
            await Shell.Current.DisplayAlert("Error", await httpResponseMessage.Content.ReadAsStringAsync(), "OK");
        }
        return httpResponseMessage.IsSuccessStatusCode;
    }

    public async Task<Post> GetItemAsync(string id)
        => (await RequestItemsAsync($"?$filter={nameof(Post.PostId)} eq {id}")).FirstOrDefault();

    public async Task<IEnumerable<Post>> GetItemsAsync(bool forceRefresh = false)
        => await RequestItemsAsync();


    private async Task<IEnumerable<Post>> RequestItemsAsync(string query = null)
        => JsonNode.Parse(await HttpClient.GetStringAsync($"{_postEndPointUrl}{query}"))!["value"].Deserialize<IEnumerable<Post>>();

    public async Task<string> Authenticate(string userName, string password) {
        var tokenResponse = await RequestTokenAsync(userName, password);
        var reposeContent = await tokenResponse.Content.ReadAsStringAsync();
        if (tokenResponse.IsSuccessStatusCode) {
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", reposeContent);
            return string.Empty;
        }
        return reposeContent;
    }

    private async Task<HttpResponseMessage> RequestTokenAsync(string userName, string password) {
	    try {
            return await HttpClient.PostAsync($"{_apiUrl}Authentication/Authenticate",
		            new StringContent(JsonSerializer.Serialize(new { userName, password = $"{password}" }), Encoding.UTF8, ApplicationJson));
	    }
	    catch (Exception) {
		    return new HttpResponseMessage(System.Net.HttpStatusCode.BadGateway) { Content = new StringContent("An error occurred during the processing of the request. Please consult the demo's ReadMe file (Step 1,10) to discover potential causes and find solutions.") };
	    }
    }

}

