using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevExtreme.OData {
	public class ObjectPermission {
		public IDictionary<string, object> Data { get; set; }
        [Key]
        public string Key { get; set; }
		public bool Write { get; set; }
		public bool Delete { get; set; }
		public ObjectPermission() {
			Data = new Dictionary<string, object>();
		}
	}
}
