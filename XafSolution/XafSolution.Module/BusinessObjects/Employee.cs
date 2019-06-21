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
	public class Employee : Person{
		public Employee(Session session) :
			base(session) {
		}
		private Department department;
		[Association("Department-Employees"), ImmediatePostData]
		public Department Department {
			get {
				return department;
			}
			set {
				SetPropertyValue("Department", ref department, value);
			}
		}
	}
}
