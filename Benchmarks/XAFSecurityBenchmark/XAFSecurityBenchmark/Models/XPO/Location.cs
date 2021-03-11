using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.Persistent.BaseImpl;

namespace XAFSecurityBenchmark.Models.XPO {
    [System.ComponentModel.DefaultProperty(nameof(Latitude))]
    public class Location : BaseObject {
        private Contact contact;
        private double latitude;
        private double longitude;

        public Location(Session session) :
            base(session) {
        }

        public override string ToString() {
            string latitudePrefix = Latitude > 0 ? "N" : "S";
            string longitudePrefix =  Longitude > 0 ? "E" : "W";
            return string.Format("{0}{1:0.###}, {2}{3:0.###}", latitudePrefix, Math.Abs(Latitude), longitudePrefix, Math.Abs(Longitude));
        }

        [Browsable(false)]
        public Contact Contact {
            get {
                return contact;
            }
            set {
                SetPropertyValue(nameof(Contact), ref contact, value);
            }
        }

        [PersistentAlias("Contact.FullName")]
        public string Title {
            get { return Convert.ToString(EvaluateAlias(nameof(Title))); }
        }

        public double Latitude {
            get {
                return latitude;
            }
            set {
                SetPropertyValue(nameof(Latitude), ref latitude, value);
            }
        }

        public double Longitude {
            get {
                return longitude;
            }
            set {
                SetPropertyValue(nameof(Longitude), ref longitude, value);
            }
        }
    }
}
