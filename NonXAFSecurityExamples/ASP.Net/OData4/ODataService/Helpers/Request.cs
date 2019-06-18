using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataService {
	public class Request {
		private Guid oid;
		private Guid employeeID;
		private string member;
		private bool permission;
		public Guid EmployeeID {
			get { return employeeID; }
			set { employeeID = value; }
		}
		public string Member {
			get { return member; }
			set { member = value; }
		}
		public bool Permission {
			get { return permission; }
			set { permission = value; }
		}
		public Guid Oid {
			get { return oid; }
		}
		public Request() {
			oid = oid = Guid.NewGuid();
		}
		public Request(Guid employeeID, string member, bool permission) {
			oid = Guid.NewGuid();
			this.employeeID = employeeID;
			this.member = member;
			this.permission = permission;
		}
	}
}