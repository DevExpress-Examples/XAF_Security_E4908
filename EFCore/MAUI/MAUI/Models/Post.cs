using System.Text.Json;
using System.Text.Json.Serialization;

namespace MAUI.Models {
	public class Post {
		[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
		public Guid ID { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		
	}
}