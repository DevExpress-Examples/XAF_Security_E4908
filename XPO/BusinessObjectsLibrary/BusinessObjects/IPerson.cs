using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessObjectsLibrary.BusinessObjects {
    public interface IPerson {
        void SetFullName(string fullName);
        string FirstName { get;set;}
        string LastName { get;set;}
        string MiddleName { get;set;}
        DateTime Birthday { get;set;}
        string FullName { get;}
        string Email { get;set;}
    }
}
