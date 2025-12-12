using System;
using System.ComponentModel;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Xpo;

namespace BusinessObjectsLibrary.BusinessObjects {
    [DefaultProperty(nameof(FullName))]
    [ImageName("BO_Person")]
    [CalculatedPersistentAliasAttribute(nameof(FullName), nameof(FullNamePersistentAlias))]
    public class Person : Party, IPerson {
        private const string defaultFullNameFormat = "{FirstName} {MiddleName} {LastName}";
        private const string defaultFullNamePersistentAlias = "concat(FirstName,' ', MiddleName,' ', LastName)";
#if MediumTrust
		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false)]
		public PersonImpl person = new PersonImpl();
#else
        private PersonImpl person = new PersonImpl();
#endif
        static Person() {
            PersonImpl.FullNameFormat = defaultFullNameFormat;
        }
        private static string fullNamePersistentAlias = defaultFullNamePersistentAlias;
        public static string FullNamePersistentAlias {
            get { return fullNamePersistentAlias; }
        }
        public static void SetFullNameFormat(string format, string persistentAlias) {
            PersonImpl.FullNameFormat = format;
            fullNamePersistentAlias = persistentAlias;
        }
        public Person(Session session) : base(session) { }

        public void SetFullName(string fullName) {
            person.SetFullName(fullName);
        }
        public string FirstName {
            get { return person.FirstName; }
            set {
                string oldValue = person.FirstName;
                person.FirstName = value;
                OnChanged(nameof(FirstName), oldValue, person.FirstName);
            }
        }
        public string LastName {
            get { return person.LastName; }
            set {
                string oldValue = person.LastName;
                person.LastName = value;
                OnChanged(nameof(LastName), oldValue, person.LastName);
            }
        }
        public string MiddleName {
            get { return person.MiddleName; }
            set {
                string oldValue = person.MiddleName;
                person.MiddleName = value;
                OnChanged(nameof(MiddleName), oldValue, person.MiddleName);
            }
        }
        public DateTime Birthday {
            get { return person.Birthday; }
            set {
                DateTime oldValue = person.Birthday;
                person.Birthday = value;
                OnChanged(nameof(Birthday), oldValue, person.Birthday);
            }
        }
        [SearchMemberOptions(SearchMemberMode.Include)]
        public string FullName {
            get { return GetFullName(); }
        }
        protected virtual string GetFullName() {
            return ObjectFormatter.Format(PersonImpl.FullNameFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
        }
        protected override string GetDisplayName() {
            return GetFullName();
        }
        [Size(255)]
        public string Email {
            get { return person.Email; }
            set {
                string oldValue = person.Email;
                person.Email = value;
                OnChanged(nameof(Email), oldValue, person.Email);
            }
        }
    }
}
