using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ODataService {
	public class AuthorizationData {
		private string userName;
		public string UserName {
			get { return userName; }
			set { userName = value; }
		}
		private string password;
		public string Password {
			get { return password; }
			set { password = value; }
		}
	}
}