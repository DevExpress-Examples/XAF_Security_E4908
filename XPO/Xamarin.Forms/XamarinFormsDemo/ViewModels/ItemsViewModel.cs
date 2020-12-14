using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.Views;

namespace XamarinFormsDemo.ViewModels {
    public class ItemsViewModel : BaseViewModel {
        
        ObservableCollection<Employee> items;
        ObservableCollection<Department> departments;
        Department selectedDepartment;
        Employee selectedItem;
        public ItemsViewModel(INavigation _navigation):base(_navigation) {
            Title = "Browse";
            Departments = new ObservableCollection<Department>();
            Items = new ObservableCollection<Employee>();

            ExecuteLoadEmployeesCommand();
            ExecuteLoadDepartmentsCommand();
            LoadDataCommand = new Command(() => { 
                ExecuteLoadEmployeesCommand(); 
                ExecuteLoadDepartmentsCommand();
            });
            AddItemCommand = new Command(async () => {
                await ExecuteAddItemCommand();
            }, ()=> XpoHelper.Security.CanCreate<Employee>());
            LogOutCommand = new Command(() => Application.Current.MainPage = new LoginPage());
        }
        void FilterByDepartment() {
            if(SelectedDepartment != null) {
                LoadEmployees();
                var items = Items.Where(w => w.Department == SelectedDepartment);
                Items = new ObservableCollection<Employee>(items);
            } else {
                LoadEmployees();
            }
        }
        void ExecuteSelectItem() {
            if(SelectedItem == null)
                return;
            var tempGuid = SelectedItem.Oid;
            SelectedItem = null;
            Navigation.PushAsync(new ItemDetailPage(tempGuid));
        }
        public Department SelectedDepartment {
            get { return selectedDepartment; }
            set { SetProperty(ref selectedDepartment, value); FilterByDepartment(); }
        }
        public Employee SelectedItem {
            get { return selectedItem; }
            set { 
                SetProperty(ref selectedItem, value); 
                if(value != null) ExecuteSelectItem(); 
            }
        }
        void ExecuteLoadEmployeesCommand() {
            if(IsBusy)
                return;

            IsBusy = true;
            LoadEmployees();
            IsBusy = false;
        }
        void ExecuteLoadDepartmentsCommand() {
            if(IsBusy)
                return;

            IsBusy = true;
            LoadDepartments();
            IsBusy = false;
        }
        async Task ExecuteAddItemCommand() {
            await Navigation.PushAsync(new ItemDetailPage(null));
        }

        public void LoadEmployees() {
            try {
                var items = uow.Query<Employee>().OrderBy(i => i.FirstName).ToList();
                Items = new ObservableCollection<Employee>(items);
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        public void LoadDepartments() {
            try {
                var items = uow.Query<Department>().ToList();
                Departments = new ObservableCollection<Department>(items);
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        public Command LogOutCommand { get; set; }
        public Command AddItemCommand { get; set; }
        public Command LoadDataCommand { get; set; }
        public ObservableCollection<Employee> Items {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public ObservableCollection<Department> Departments {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }
    }
}
