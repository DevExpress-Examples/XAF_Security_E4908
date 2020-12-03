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
        public ObservableCollection<Employee> Items {
            get { return items; }
            set { SetProperty(ref items, value); }
        }
        public ObservableCollection<Department> Departments {
            get { return departments; }
            set { SetProperty(ref departments, value); }
        }
        ObservableCollection<Employee> items;
        ObservableCollection<Department> departments;
        public Command LoadDataCommand { get; set; }
        public bool CheckCreate { get => XpoHelper.security.CanCreate(typeof(Employee)); }

        public ItemsViewModel() {
            Title = "Browse";
            Departments = new ObservableCollection<Department>();
            Items = new ObservableCollection<Employee>();
            LoadDataCommand = new Command(async () => { 
                await ExecuteLoadEmployeesCommand(); 
                await ExecuteLoadDepartmentsCommand();
            });
        }

        async Task ExecuteLoadEmployeesCommand() {
            if(IsBusy)
                return;

            IsBusy = true;
            await LoadEmployeesAsync();
            IsBusy = false;
        }
        async Task ExecuteLoadDepartmentsCommand() {
            if(IsBusy)
                return;

            IsBusy = true;
            await LoadDepartmentsAsync();
            IsBusy = false;
        }
        public void UpdateItems() {
            OnPropertyChanged(nameof(Items));
        }

        public async Task LoadEmployeesAsync() {
            try {
                Items.Clear();
                var items = await DataStore.GetEmployeesAsync(uow, true);
                foreach(var item in items) {
                    Items.Add(item);
                }
                OnPropertyChanged(nameof(Items));
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        public async Task LoadDepartmentsAsync() {
            try {
                Departments.Clear();
                var items = await DataStore.GetDepartmentsAsync(uow, true);
                foreach(var item in items) {
                    Departments.Add(item);
                }
                OnPropertyChanged(nameof(Departments));
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
    }
}
