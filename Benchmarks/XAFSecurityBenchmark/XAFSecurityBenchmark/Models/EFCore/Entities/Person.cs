using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;

namespace XAFSecurityBenchmark.Models.EFCore {

    [DefaultProperty(nameof(FullName))]
    [ImageName("BO_Person")]
    public class Person : Party, IPerson {

        public virtual String FirstName { get; set; }
        public virtual String LastName { get; set; }
        public virtual String MiddleName { get; set; }
        public virtual DateTime? Birthday { get; set; }

        [FieldSize(255)]
        public virtual String Email { get; set; }

        [NotMapped, SearchMemberOptions(SearchMemberMode.Exclude)]
        public String FullName {
            get { return $"{FirstName} {MiddleName} {LastName}"; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override String DisplayName {
            get { return FullName; }
        }

        DateTime IPerson.Birthday { get { return Birthday.GetValueOrDefault(); } set { Birthday = value; } }

        public void SetFullName(string fullName) {
            throw new NotImplementedException();
        }
    }
}
