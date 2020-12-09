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

        public ItemsViewModel() {
            Title = "Browse";
            Departments = new ObservableCollection<Department>();
            Items = new ObservableCollection<Employee>();
            LoadDataCommand = new Command(async () => { 
                await ExecuteLoadEmployeesCommand(); 
                await ExecuteLoadDepartmentsCommand();
            });
            AddItemCommand = new Command(async () => {
                await ExecuteAddItemCommand();
            }, ()=> XpoHelper.Security.CanCreate<Employee>());
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
        async Task ExecuteAddItemCommand() {
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel(null)));
        }

        public async Task LoadEmployeesAsync() {
            try {
                var items = await uow.Query<Employee>().OrderBy(i => i.FirstName).ToListAsync();
                Items = new ObservableCollection<Employee>(items);
                OnPropertyChanged(nameof(Items));
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        public async Task LoadDepartmentsAsync() {
            try {
                var items = await uow.Query<Department>().ToListAsync();
                Departments = new ObservableCollection<Department>(items);
                OnPropertyChanged(nameof(Departments));
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
        
        
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
