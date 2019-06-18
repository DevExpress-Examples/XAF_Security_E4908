using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataService {
	public class Test {
		Guid oid;
		public Guid Oid {
			get { return oid; }
			set { oid = value; }
		}
		IDictionary<string, object> data;
		public IDictionary<string, object> Data {
			get { return data; }
			set { data = value; }
		}
		public Test() {
			data = new Dictionary<string, object>();
		}
	}
}