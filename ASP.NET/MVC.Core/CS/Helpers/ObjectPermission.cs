using System;
using System.Collections.Generic;

namespace AspNetCoreMvcApplication {
	public class ObjectPermission {
		public IDictionary<string, object> Data { get; set; }
		public string Key { get; set; }
		public bool Write { get; set; }
		public bool Delete { get; set; }
		public ObjectPermission() {
			Data = new Dictionary<string, object>();
		}
	}
}

