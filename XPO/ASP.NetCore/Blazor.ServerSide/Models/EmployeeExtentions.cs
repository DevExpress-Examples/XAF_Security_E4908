using BusinessObjectsLibrary.BusinessObjects;

namespace Blazor.ServerSide.Models;

public static class EmployeeExtensions {
    public static EditableEmployee ToModel(this Employee employee) {
        return new EditableEmployee {
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            Department = employee.Department
        };
    }
    public static void FromModel(this EditableEmployee editableEmployee, Employee employee) {
        employee.FirstName = editableEmployee.FirstName;
        employee.LastName = editableEmployee.LastName;
        employee.Email = editableEmployee.Email;
        employee.Department = editableEmployee.Department;
    }
}