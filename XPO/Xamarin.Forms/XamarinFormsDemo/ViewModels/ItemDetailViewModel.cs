using DevExpress.Data.Extensions;
using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
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

        public ItemDetailViewModel(Employee item = null) {
            Title = item?.FullName;
            Item = item;
            TempCommandDelete = new Command(() => {
            },
        () => CheckDelete);
            TempCommandUpdate = new Command(() => {
            },
        () => CheckUpdate);
            CheckDelete = XpoHelper.security.CanDelete(Item);
            CheckUpdate = XpoHelper.security.CanWrite(Item);
            if(Item.Department != null) {
                Task<List<Department>> myTask = Task.Run(() => task());
                myTask.Wait();
                Departments = myTask.Result;
                Department = Departments.IndexOf((Department)Item.Department);
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
        bool checkUpdate;
        bool checkDelete;
        public Command TempCommandDelete { get; private set; }
        public Command TempCommandUpdate { get; private set; }

        async Task<List<Department>> task() {
            return await ((UnitOfWork)Item.Session).Query<Department>().ToListAsync();
        }
    }
}
