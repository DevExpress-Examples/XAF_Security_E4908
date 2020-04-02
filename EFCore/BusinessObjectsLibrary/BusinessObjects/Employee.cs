using DevExpress.Persistent.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects {
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
