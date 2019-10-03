using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreODataService {
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
