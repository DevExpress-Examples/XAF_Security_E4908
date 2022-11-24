using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using DevExpress.Persistent.BaseImpl.EF;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class Location: BaseObject {
        public override string ToString() {
            string latitudePrefix = Latitude > 0 ? "N" : "S";
            string longitudePrefix = Longitude > 0 ? "E" : "W";
            return string.Format("{0}{1:0.###}, {2}{3:0.###}", latitudePrefix, Math.Abs(Latitude), longitudePrefix, Math.Abs(Longitude));
        }

        public virtual Guid ContactRef { get; set; }

        [Browsable(false)]
        public virtual Contact Contact { get; set; }

        [NotMapped]
        public string Title {
            get { return Contact.FullName; }
        }

        public virtual double Latitude { get; set; }
        public virtual double Longitude { get; set; }
    }
}
