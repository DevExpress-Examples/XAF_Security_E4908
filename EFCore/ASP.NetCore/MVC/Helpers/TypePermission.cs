public class TypePermission {
	public IDictionary<string, bool> Data { get; set; }
	public bool Create { get; set; }
	public TypePermission() {
		Data = new Dictionary<string, bool>();
	}
}


