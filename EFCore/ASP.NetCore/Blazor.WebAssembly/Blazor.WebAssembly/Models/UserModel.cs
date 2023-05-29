namespace Blazor.WebAssembly.Models;

public class UserModel {
    public Guid XafUserId { get; set; }
    public string LoginProviderUserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string? Email { get; set; }
}