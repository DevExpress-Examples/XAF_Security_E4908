namespace Blazor.WebAssembly.Models;

public class UserModel {
    public Guid ID { get; set; }
    public string UserName { get; set; } = null!;
    public string IsActive { get; set; } = null!;
    
}