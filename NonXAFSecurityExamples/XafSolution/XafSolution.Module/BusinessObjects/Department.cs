using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XafSolution.Module.BusinessObjects {
	[DefaultClassOptions]
	[System.ComponentModel.DefaultProperty("Title")]
	public class Department : BaseObject {
		private string title;
		private string office;
		public Department(Session session)
			: base(session) {
		}
		public string Title {
			get {
				return title;
			}
			set {
				SetPropertyValue(nameof(Title), ref title, value);
			}
		}
		public string Office {
			get {
				return office;
			}
			set {
				SetPropertyValue(nameof(Office), ref office, value);
			}
		}
		[Association("Department-Employees")]
		public XPCollection<Employee> Employees {
			get {
				return GetCollection<Employee>(nameof(Employees));
			}
		}
	}
}
