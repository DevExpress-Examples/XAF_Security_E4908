using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace BusinessObjectsLibrary.BusinessObjects {
    [DefaultClassOptions]
    public class Employee : Person {
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
