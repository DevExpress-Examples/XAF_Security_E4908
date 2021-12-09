using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace BusinessObjectsLibrary.BusinessObjects {
    public class Employee : Person {
        public Employee(Session session) :
            base(session) {
        }
        private Department department;
        [Association("Department-Employees")]
        public Department Department {
            get {
                return department;
            }
            set {
                SetPropertyValue(nameof(Department), ref department, value);
            }
        }
    }
}
