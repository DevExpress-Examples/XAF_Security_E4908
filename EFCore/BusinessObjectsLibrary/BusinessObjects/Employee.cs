using DevExpress.Persistent.BaseImpl.EF;

namespace BusinessObjectsLibrary.BusinessObjects {
    public class Employee : Person {
        private Department department;
        public virtual Department Department {
            get {
                return department;
            }
            set {
                SetReferencePropertyValue(ref department, value);
            }
        }
    }
}
