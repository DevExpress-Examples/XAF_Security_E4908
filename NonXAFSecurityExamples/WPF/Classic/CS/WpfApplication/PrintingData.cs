using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication {
	public class PrintingData {
		private string fullName;
		public string FullName {
			get { return fullName; }
			set { fullName = value; }
		}
		private string department;
		public string Department {
			get { return department; }
			set { department = value; }
		}
		public PrintingData(string FullName, string Department) {
			this.FullName = FullName;
			this.Department = Department;
		}
	}
}
