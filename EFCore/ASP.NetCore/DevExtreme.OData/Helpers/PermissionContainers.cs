using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevExtreme.OData {
    public class MemberPermission {
        [Key]
        public Guid Key { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public MemberPermission() {
            Key = Guid.NewGuid();
        }
    }
    public class ObjectPermission {
        public IDictionary<string, object> Data { get; set; }
        [Key]
        public string Key { get; set; }
        public bool Write { get; set; }
        public bool Delete { get; set; }
        public ObjectPermission() {
            Data = new Dictionary<string, object>();
        }
    }
    public class TypePermission {
        public IDictionary<string, object> Data { get; set; }
        [Key]
        public string Key { get; set; }
        public bool Create { get; set; }
        public TypePermission() {
            Data = new Dictionary<string, object>();
        }
    }
}
