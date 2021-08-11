using BusinessObjectsLibrary.BusinessObjects;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinFormsDemo.Services;

namespace XamarinFormsDemo.ViewModels {
    [QueryProperty(nameof(ItemGuid), "itemGuid")]
    public class ItemDetailViewModel : XpoViewModel {
        string _itemGuid;
        bool canDelete;

        bool canReadDepartment;
        bool canWriteDepartment;

        ObservableCollection<Department> departments;

        bool isNewItem;
        Employee item;
        bool readOnly;

        public ItemDetailViewModel() {
            CommandDelete = new Command(async () => await DeleteItemAndGoBack(),
                () => CanDelete && !isNewItem);
            CommandUpdate = new Command(async () => await SaveItemAndGoBack(),
                () => !ReadOnly);
        }
        async void Load() {
            Departments = new ObservableCollection<Department>(await UnitOfWork.Query<Department>().ToListAsync());
            IsNewItem = String.IsNullOrEmpty(ItemGuid);
            if(isNewItem) {
                Item = new Employee(UnitOfWork) { FirstName = "First name", LastName = "Last Name" };
                Title = "New Employee";
            } else {
                Item = UnitOfWork.GetObjectByKey<Employee>(Guid.Parse(ItemGuid));
                Title = Item?.FullName;
            }

            CanDelete = XpoHelper.Security.CanDelete(Item);
            ReadOnly = !XpoHelper.Security.CanWrite(Item);
            CanReadDepartment = XpoHelper.Security.CanRead(Item, "Department");
            CanWriteDepartment = XpoHelper.Security.CanWrite(Item, "Department");
            if(isNewItem && CanWriteDepartment) {
                Item.Department = Departments?[0];
            }

        }
        async Task SaveItemAndGoBack() {
            try {
                await UnitOfWork.CommitChangesAsync();
            } catch(Exception ex) {
                await Shell.Current.DisplayAlert("Saving failed", ex.Message, "OK");
            }
            await Shell.Current.Navigation.PopAsync();
        }
        async Task DeleteItemAndGoBack() {
            UnitOfWork.Delete(Item);
            try {
                await UnitOfWork.CommitChangesAsync();
            } catch(Exception ex) {
                await Shell.Current.DisplayAlert("Deleting failed", ex.Message, "OK");
            }
            await Shell.Current.Navigation.PopAsync(); 
        }
        public bool CanDelete {
            get { return canDelete; }
            set { 
                SetProperty(ref canDelete, value); 
                CommandDelete.ChangeCanExecute(); 
            }
        }
        public bool CanReadDepartment {
            get { return canReadDepartment; }
            set { SetProperty(ref canReadDepartment, value); }
        }
        public bool CanWriteDepartment {
            get { return canWriteDepartment; }
            set { SetProperty(ref canWriteDepartment, value); }
        }
        public Command CommandDelete { get; private set; }
        public Command CommandUpdate { get; private set; }
        public ObservableCollection<Department> Departments {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }
        public bool IsNewItem {
            get { return isNewItem; }
            set { SetProperty(ref isNewItem, value); }
        }
        public Employee Item {
            get { return item; }
            set { SetProperty(ref item, value); }
        }
        public string ItemGuid {
            get { return _itemGuid; }
            set {
                SetProperty(ref _itemGuid, value);
                Load();
            }
        }
        public bool ReadOnly {
            get { return readOnly; }
            set { 
                SetProperty(ref readOnly, value); 
                CommandUpdate.ChangeCanExecute(); 
            }
        }
    }
}
