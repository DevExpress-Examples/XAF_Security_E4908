using DevExpress.Persistent.Base;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    [DefaultClassOptions]
    public class Employee : Person {
        private Department department;
        [ImmediatePostData]
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
