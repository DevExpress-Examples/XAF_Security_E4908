using System.ComponentModel.DataAnnotations;

public class TypePermission {
	public IDictionary<string, object> Data { get; set; }
    [Key]
    public string Key { get; set; }
	public bool Create { get; set; }
	public TypePermission() {
		Data = new Dictionary<string, object>();
	}
}

