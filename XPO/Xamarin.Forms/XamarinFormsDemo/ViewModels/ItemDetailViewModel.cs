using DevExpress.Data.Extensions;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

namespace XamarinFormsDemo.ViewModels {
    public class ItemDetailViewModel : BaseViewModel {
        public Employee Item { get; set; }
        public int Department {
            get { return department; }
            set { SetProperty(ref department, value);  }
        }
        int department;
        public List<Department> Departments {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }
        List<Department> departments;

        public ItemDetailViewModel(Guid Oid) {
            Item = uow.GetObjectByKey<Employee>(Oid);
            Title = Item?.FullName;

            TempCommandDelete = new Command(async () => {
                uow.Delete(Item);
                await uow.CommitChangesAsync();
                uow.Dispose();
            },
        () => CheckDelete);
            TempCommandUpdate = new Command(async () => {
                uow.Save(Item);
                await uow.CommitChangesAsync();
                uow.Dispose();
            },
        () => CheckUpdate);
            CheckDelete = XpoHelper.security.CanDelete(Item);
            CheckUpdate = XpoHelper.security.CanWrite(Item);
            if(Item.Department != null) {
                Departments = Task.Run(async () => await GetDepartments()).GetAwaiter().GetResult();
                Department = -1;
                for(int i = 0; i < Departments.Count; i++)  {
                    if(Departments[i].Oid == Item.Department.Oid) {
                        Department = i;
                        break;
                    }
                }
                //Department = Departments.((Department)Item.Department);
            }
        }
        public bool CheckDelete {
            get { return checkDelete; }
            set { SetProperty(ref checkDelete, value); TempCommandDelete.ChangeCanExecute(); }
        }
        public bool CheckUpdate {
            get { return checkUpdate; }
            set { SetProperty(ref checkUpdate, value); TempCommandUpdate.ChangeCanExecute(); }
        }
        async Task<List<Department>> GetDepartments() {
            return await uow.Query<Department>().ToListAsync();
        }

        bool checkUpdate;
        bool checkDelete;
        public Command TempCommandDelete { get; private set; }
        public Command TempCommandUpdate { get; private set; }

    }
}
