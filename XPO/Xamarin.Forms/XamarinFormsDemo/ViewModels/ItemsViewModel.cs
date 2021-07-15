using BusinessObjectsLibrary;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;

using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinFormsDemo.Services;
using XamarinFormsDemo.Views;

namespace XamarinFormsDemo.ViewModels {
    public class ItemsViewModel : XpoViewModel {
        public ItemsViewModel() {
            Title = "Browse";
            Items = new ObservableCollection<Employee>();
            Departments = new ObservableCollection<Department>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            AddItemCommand = new Command(async () => await ExecuteAddItemCommand(),
                () => XpoHelper.Security.CanCreate<Employee>());
            LogOutCommand = new Command(async () => await ExecuteLogOutCommand());
            ItemTapped = new Command<Employee>(OnItemSelected);
        }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Command LogOutCommand { get; set; }
        public Command<Employee> ItemTapped { get; }
        ObservableCollection<Department> departments;
        public ObservableCollection<Department> Departments {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }
        ObservableCollection<Employee> items;
        public ObservableCollection<Employee> Items {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public Command LoadDataCommand { get; set; }

        Department selectedDepartment;
        public Department SelectedDepartment {
            get { return selectedDepartment; }
            set { 
                SetProperty(ref selectedDepartment, value);
                _ = LoadEmployees(); 
            }
        }
        async Task ExecuteLoadItemsCommand() {
            IsBusy = true;
            try {
                Departments = new ObservableCollection<Department>(await UnitOfWork.Query<Department>().ToListAsync());
                await LoadEmployees();
            } catch(Exception ex) {
                await Shell.Current.DisplayAlert("Loading failed", ex.Message, "OK");
            } finally {
                IsBusy = false;
            }
        }
        async Task ExecuteAddItemCommand() {
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?itemGuid=");
        }
        async Task ExecuteLogOutCommand() {
            XpoHelper.Logoff();
            await App.ResetMainPage();
        }
        async Task LoadEmployees() {
            try {
                IQueryable<Employee> items = UnitOfWork.Query<Employee>().OrderBy(i => i.FirstName);
                if(SelectedDepartment != null) {
                    items = items.Where(i => i.Department == SelectedDepartment);
                }
                Items = new ObservableCollection<Employee>(await items.ToListAsync());
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }

        async void OnItemSelected(Employee item) {
            if(item == null)
                return;
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?itemGuid={item.Oid.ToString()}");
        }


        public void OnAppearing() {
            IsBusy = true;
        }
    }
}