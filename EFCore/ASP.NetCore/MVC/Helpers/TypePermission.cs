using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MvcApplication {
	public class TypePermission {
		public IDictionary<string, bool> Data { get; set; }
		public bool Create { get; set; }
		public TypePermission() {
			Data = new Dictionary<string, bool>();
		}
	}
}

