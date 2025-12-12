using System;
using System.Collections.Generic;
using System.Text;

namespace XAFSecurityBenchmark.Models.Base {
    public interface IPhoneNumber {
        string Number { get; set;}
        string PhoneType { get;set;}
    }
}
