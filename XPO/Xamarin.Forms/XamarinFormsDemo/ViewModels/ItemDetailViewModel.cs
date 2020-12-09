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

        public ItemDetailViewModel() {
            IsNewItem = true;
            Departments = Task.Run(async () => await GetDepartments()).GetAwaiter().GetResult();
            Department = -1;
            CommandUpdate = new Command(async () => {
                await SaveItemAndGoBack();
            },
        () => CanUpdate);
            CommandDelete = new Command(async () => {
                await DeleteItemAndGoBack();
            },
        () => true);
            Item = new Employee(uow) { FirstName = "First name", LastName = "Last Name"};
            Title = "New Employee";
            CanDelete = XpoHelper.Security.CanDelete(Item);
            CanUpdate = XpoHelper.Security.CanWrite(Item);
            CanReadDepartment = XpoHelper.Security.CanRead(Item, "Department");
        }


        public ItemDetailViewModel(Guid Oid) {
            IsNewItem = false;
            Item = uow.GetObjectByKey<Employee>(Oid);
            Title = Item?.FullName;

            CommandDelete = new Command(async () => {
                await DeleteItemAndGoBack();
            },
        () => CanDelete);
            CommandUpdate = new Command(async () => {
                await SaveItemAndGoBack();
            },
        () => CanUpdate);
            CanDelete = XpoHelper.Security.CanDelete(Item);
            CanUpdate = XpoHelper.Security.CanWrite(Item);
            CanReadDepartment = XpoHelper.Security.CanRead(Item, "Department");
            if(Item.Department != null && CanReadDepartment) {
                Departments = Task.Run(async () => await GetDepartments()).GetAwaiter().GetResult();
                Department = -1;
                for(int i = 0; i < Departments.Count; i++) {
                    if(Departments[i].Oid == Item.Department.Oid) {
                        Department = i;
                        break;
                    }
                }
            }
        }

        private async Task DeleteItemAndGoBack() {
            uow.Delete(Item);
            await uow.CommitChangesAsync();
            uow.Dispose();
            await navigation.PopToRootAsync();
        }

        private async Task SaveItemAndGoBack() {
            uow.Save(Item);
            await uow.CommitChangesAsync();
            uow.Dispose();
            await navigation.PopToRootAsync();
        }
        public INavigation Navigation {
            get { return navigation; }
            set { SetProperty(ref navigation, value); }
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
