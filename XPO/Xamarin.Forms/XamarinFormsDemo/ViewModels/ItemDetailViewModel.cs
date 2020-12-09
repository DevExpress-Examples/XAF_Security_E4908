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
        bool canUpdate;
        bool canDelete;
        bool canReadDepartment;
        int department;
        List<Department> departments;
        bool isNewItem;
        INavigation navigation;


        public ItemDetailViewModel(Guid? Oid) {
            IsNewItem = (Oid == null);
            if(IsNewItem) {
                Item = new Employee(uow) { FirstName = "First name", LastName = "Last Name" };
            } else {
                Item = uow.GetObjectByKey<Employee>(Oid);
            }
            Title = Item?.FullName;

            CommandDelete = new Command(async () => {
                await DeleteItemAndGoBack();
            },
        () => CanDelete && !IsNewItem);
            CommandUpdate = new Command(async () => {
                await SaveItemAndGoBack();
            },
        () => CanUpdate);
            CanDelete = XpoHelper.Security.CanDelete(Item);
            CanUpdate = XpoHelper.Security.CanWrite(Item);
            CanReadDepartment = XpoHelper.Security.CanRead(Item, "Department");
            Departments = uow.Query<Department>().ToListAsync().GetAwaiter().GetResult();
            if(IsNewItem && CanReadDepartment) {
                Item.Department = Departments[0];
            }

        }

        private async Task DeleteItemAndGoBack() {
            uow.Delete(Item);
            await uow.CommitChangesAsync();
            await navigation.PopToRootAsync();
        }

        private async Task SaveItemAndGoBack() {
            uow.Save(Item);
            await uow.CommitChangesAsync();
            await navigation.PopToRootAsync();
        }
        public bool CanDelete {
            get { return canDelete; }
            set { SetProperty(ref canDelete, value); CommandDelete.ChangeCanExecute(); }
        }
        public bool CanReadDepartment {
            get { return canReadDepartment; }
            set { SetProperty(ref canReadDepartment, value); }
        }
        public bool CanUpdate {
            get { return canUpdate; }
            set { SetProperty(ref canUpdate, value); CommandUpdate.ChangeCanExecute(); }
        }
        async Task<List<Department>> GetDepartments() {
            return await uow.Query<Department>().ToListAsync();
        }
        public Employee Item { get; set; }
        public int Department {
            get { return department; }
            set { SetProperty(ref department, value); }
        }
        public List<Department> Departments {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }
        public bool IsNewItem {
            get { return isNewItem; }
            set { SetProperty(ref isNewItem, value); }
        }
        public Command CommandDelete { get; private set; }
        public Command CommandUpdate { get; private set; }

    }
}
