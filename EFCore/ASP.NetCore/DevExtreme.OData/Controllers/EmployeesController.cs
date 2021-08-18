using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OData.Query;
using DevExpress.ExpressApp;
using BusinessObjectsLibrary.BusinessObjects;

//namespace DevExtreme.OData.Controllers {
public class EmployeesController : ODataController, IDisposable {
    SecurityProvider securityProvider;
    IObjectSpace objectSpace;
    public EmployeesController(SecurityProvider securityProvider) {
        this.securityProvider = securityProvider;
        objectSpace = securityProvider.ObjectSpaceProvider.CreateObjectSpace();
    }
    [HttpGet]
    [EnableQuery]
    public ActionResult Get() {
        // The EFCore way:
        // var dbContext = ((EFCoreObjectSpace)objectSpace).DbContext;
        // 
        // The XAF way:
        IQueryable<Employee> employees = objectSpace.GetObjectsQuery<Employee>().Include(e => e.Department);
        return Ok(employees);
    }
    [HttpDelete]
    public ActionResult Delete(int key) {
        Employee existing = objectSpace.GetObjectByKey<Employee>(key);
        if(existing != null) {
            objectSpace.Delete(existing);
            objectSpace.CommitChanges();
            return NoContent();
        }
        return NotFound();
    }
    [HttpPatch]
    public ActionResult Patch(int key, [FromBody] JsonElement serializedProperties) {
        Employee employee = objectSpace.FirstOrDefault<Employee>(e => e.ID == key);
        if(employee != null) {
            JsonParser.ParseJson<Employee>(serializedProperties, employee, objectSpace);
            objectSpace.CommitChanges();
            return Ok(employee);
        }
        return NotFound();
    }
    [HttpPost]
    public ActionResult Post([FromBody] JsonElement serializedProperties) {
        Employee newEmployee = objectSpace.CreateObject<Employee>();
        JsonParser.ParseJson<Employee>(serializedProperties, newEmployee, objectSpace);
        objectSpace.CommitChanges();
        return Ok(newEmployee);
    }
    public void Dispose() {
        objectSpace?.Dispose();
        securityProvider?.Dispose();
    }
}
//}
