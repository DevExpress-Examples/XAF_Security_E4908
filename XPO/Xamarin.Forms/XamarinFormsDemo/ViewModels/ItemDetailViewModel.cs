using DevExpress.ExpressApp.Security;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

namespace XamarinFormsDemo.ViewModels {
    public class ItemDetailViewModel : BaseViewModel {
        public Employee Item { get; set; }
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
    }
}
