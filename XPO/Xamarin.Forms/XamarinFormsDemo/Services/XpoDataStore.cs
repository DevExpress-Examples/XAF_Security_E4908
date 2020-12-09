using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using XamarinFormsDemo.Services;
using XamarinFormsDemo.ViewModels;

[assembly: Xamarin.Forms.Dependency(typeof(XamarinFormsDemo.XpoDataStore))]
namespace XamarinFormsDemo {
    public class XpoDataStore {
        
        public async Task<IEnumerable<Employee>> GetEmployeesAsync(UnitOfWork uow , bool forceRefresh = false) {
            var res = await uow.Query<Employee>().OrderBy(i => i.FirstName).ToListAsync();
            return res;
        }
        public async Task<IEnumerable<Department>> GetDepartmentsAsync(UnitOfWork uow, bool forceRefresh = false) {
            var res = await uow.Query<Department>().ToListAsync();
            return res;
        }
        public async Task<bool> UpdateItemAsync(Employee item) {
            try {
                using(var uow = XpoHelper.CreateUnitOfWork()) {
                    var itemToUpdate = await uow.GetObjectByKeyAsync<Employee>(item.Oid);
                    if(itemToUpdate == null) {
                        return false;
                    }
                    itemToUpdate.FirstName = item.FirstName;
                    itemToUpdate.LastName = item.LastName;
                    itemToUpdate.Department = item.Department;
                    uow.Save(itemToUpdate);
                    await uow.CommitChangesAsync();
                    return true;
                }
            } catch(Exception) {
                return false;
            }
        }
    }
}
