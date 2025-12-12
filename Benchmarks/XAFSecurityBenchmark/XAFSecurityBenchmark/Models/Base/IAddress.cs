using System;
using System.Collections.Generic;
using System.Text;

namespace XAFSecurityBenchmark.Models.Base {
    public interface IAddress {
        string Street { get;set;}
        string City { get;set;}
        string StateProvince { get;set;}
        string ZipPostal { get;set;}
        ICountry Country { get;set;}
        string FullAddress { get;}
    }
}
