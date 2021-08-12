using System.Collections.Generic;

namespace DevExtreme.OData {
	public class PermissionContainer {
		public IDictionary<string, object> Data { get; set; }
		public string Key { get; set; }
		public PermissionContainer() {
			Data = new Dictionary<string, object>();
		}
	}
}
