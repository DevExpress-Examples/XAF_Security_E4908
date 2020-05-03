using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Base;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    [DefaultProperty(nameof(FullName))]
    [ImageName("BO_Person")]
    public class Person : Party, IPerson {
        private String firstName;
        private String lastName;
        private String middleName;
        private Nullable<DateTime> birthday;
        private String email;

        public String FirstName {
            get { return firstName; }
            set { SetPropertyValue(ref firstName, value); }
        }
        public String LastName {
            get { return lastName; }
            set { SetPropertyValue(ref lastName, value); }
        }
        public String MiddleName {
            get { return middleName; }
            set { SetPropertyValue(ref middleName, value); }
        }
        public DateTime? Birthday {
            get { return birthday; }
            set { SetPropertyValue(ref birthday, value); }
        }
        [FieldSize(255)]
        public String Email {
            get { return email; }
            set { SetPropertyValue(ref email, value); }
        }

        [NotMapped, SearchMemberOptions(SearchMemberMode.Exclude)]
        public String FullName {
            get { return ObjectFormatter.Format(FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty); }
        }
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override String DisplayName {
            get { return FullName; }
        }

        public static String FullNameFormat = "{FirstName} {MiddleName} {LastName}";

        DateTime IPerson.Birthday {
            get { return Birthday.HasValue ? Birthday.Value : DateTime.MinValue; }
            set { Birthday = value; }
        }

        public void SetFullName(String fullName) {
            FirstName = fullName;
        }
    }

    public interface IPerson {
        void SetFullName(string fullName);
        string FirstName { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        DateTime Birthday { get; set; }
        string FullName { get; }
        string Email { get; set; }
    }
}
