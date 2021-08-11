using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XamarinFormsDemo.Services;

[assembly: Xamarin.Forms.Dependency(typeof(XamarinFormsDemo.XpoDataStore))]
namespace XamarinFormsDemo {
    public class XpoDataStore : IDataStore<Employee> {
        public async Task<bool> AddItemAsync(Employee item) {
            try {
                using(var uow = XpoHelper.CreateUnitOfWork()) {
                    Guid.NewGuid().ToString();
                    uow.Save(item);
                    await uow.CommitChangesAsync();
                    return true;
                }
            } catch(Exception) {
                return false;
            }
        }

        public async Task<bool> DeleteItemAsync(Guid id) {
            try {
                using(var uow = XpoHelper.CreateUnitOfWork()) {
                    var itemToDelete = uow.GetObjectByKey<Employee>(id);
                    if(itemToDelete != null) {
                        uow.Delete(itemToDelete);
                        await uow.CommitChangesAsync();
                    }
                    return true;
                }
            } catch(Exception) {
                return false;
            }
        }

        public Task<Employee> GetItemAsync(Guid id) {
            using(var uow = XpoHelper.CreateUnitOfWork()) {
                return uow.GetObjectByKeyAsync<Employee>(id);
            }
        }

        public async Task<IEnumerable<Employee>> GetItemsAsync(bool forceRefresh = false) {
            using(var uow = XpoHelper.CreateUnitOfWork()) {
                var res = await uow.Query<Employee>().OrderBy(i => i.FirstName).ToListAsync();
                return res;
            }
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
