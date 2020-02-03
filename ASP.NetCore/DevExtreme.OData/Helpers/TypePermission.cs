using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreODataService {
	public class TypePermission {
		public IDictionary<string, object> Data { get; set; }
        [Key]
        public string Key { get; set; }
		public bool Create { get; set; }
		public TypePermission() {
			Data = new Dictionary<string, object>();
		}
	}
}
