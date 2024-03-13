using BusinessObjectsLibrary.BusinessObjects;

namespace Blazor.ServerSide.Models;
public class EditableEmployee {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public Department Department { get; set; }
}
