using System.Net;
using System.Net.Http.Json;
using Blazor.WebAssembly.Models;

namespace Blazor.WebAssembly.Services;
public interface IWebAPI {
    Task<HttpResponseMessage> LoginAsync(string? userName,string? password);
    Task<(string message, UserModel? user)> GetUserAsync();
    Task<bool> LogoutAsync();
    Task<bool> CanCreateAsync();
    Task ArchiveAsync(Post post);
    Task<byte[]> GetAuthorPhotoAsync(Guid postId);
    Task<byte[]> ShapePostsAsync();
}
public class WebAPI: IWebAPI {
    private readonly HttpClient _httpClient;

    public WebAPI(IHttpClientFactory httpClientFactory) 
        => _httpClient = httpClientFactory.CreateClient("API");

    public async Task<HttpResponseMessage> LoginAsync(string? userName,string? password) 
        => (await _httpClient.PostAsJsonAsync("Authentication/LoginAsync", new{userName,password}));

    public async Task<(string message, UserModel? user)> GetUserAsync() {
        try {
            var response = await _httpClient.GetAsync("Authentication/UserInfo");
            return response.IsSuccessStatusCode ? ("Success", await response.Content.ReadFromJsonAsync<UserModel>())
                : response.StatusCode == HttpStatusCode.Unauthorized ? ("Unauthorized", null) : ("Failed", null);
        }
        catch (Exception e) {
            return ("Failed", null);
        }
    }

    public async Task<bool> LogoutAsync() 
        => (await _httpClient.PostAsync("Authentication/LogoutAsync", null)).IsSuccessStatusCode;

    public async Task<bool> CanCreateAsync() 
        => await _httpClient.GetFromJsonAsync<bool>("CustomEndpoint/CanCreate?typename=Post");

    public async Task ArchiveAsync(Post post) 
        => await _httpClient.PostAsJsonAsync("CustomEndPoint/Archive", post);

    public async Task<byte[]> GetAuthorPhotoAsync(Guid postId) 
        => await _httpClient.GetByteArrayAsync($"CustomEndPoint/AuthorPhoto/{postId}");

    public async Task<byte[]> ShapePostsAsync() 
        => await _httpClient.GetByteArrayAsync("report/DownloadByName(Post Report)");
}