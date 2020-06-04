using System.Collections.Generic;

namespace ASPNETCoreODataService {
	public class PermissionContainer {
		public IDictionary<string, object> Data { get; set; }
		public string Key { get; set; }
		public PermissionContainer() {
			Data = new Dictionary<string, object>();
		}
	}
}
