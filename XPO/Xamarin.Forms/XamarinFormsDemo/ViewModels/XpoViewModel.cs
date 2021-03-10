using DevExpress.Xpo;

using XamarinFormsDemo.Services;

namespace XamarinFormsDemo.ViewModels {
    public class XpoViewModel : BaseViewModel {
        UnitOfWork unitOfWork;
        protected UnitOfWork UnitOfWork {
            get {
                if(unitOfWork == null) {
                    unitOfWork = XpoHelper.CreateUnitOfWork();
                }
                return unitOfWork;
            }
        }
        public XpoViewModel() {
            if(!XpoHelper.Security.IsAuthenticated) {
                App.ResetMainPage();
            }
        }
    }
}
