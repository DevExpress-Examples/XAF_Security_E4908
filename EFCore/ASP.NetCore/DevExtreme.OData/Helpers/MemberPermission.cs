using System;
using System.ComponentModel.DataAnnotations;

namespace DevExtreme.OData {
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
