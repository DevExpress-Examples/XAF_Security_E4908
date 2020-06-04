using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETCoreODataService {
	public class MemberPermission {
        [Key]
        public Guid Key { get; set; }
		public bool Read { get; set; }
		public bool Write { get; set; }

		public MemberPermission() {
			Key = Guid.NewGuid();
		}
	}
}
