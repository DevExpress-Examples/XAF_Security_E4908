using System.Text.Json.Serialization;

namespace Blazor.WebAssembly.Models;

public class Post {
    public Guid ID { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
}