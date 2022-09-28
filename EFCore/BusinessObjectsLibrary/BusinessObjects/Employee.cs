namespace BusinessObjectsLibrary.BusinessObjects {
    public class Employee : Person {

        public virtual Department Department { get; set; }

        public new string FullName {
            get { return $"{FirstName} {LastName}"; }
        }
    }
}
