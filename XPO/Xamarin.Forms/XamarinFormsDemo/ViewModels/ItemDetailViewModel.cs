using System;
using XafSolution.Module.BusinessObjects;

namespace XamarinFormsDemo.ViewModels {
    public class ItemDetailViewModel : BaseViewModel {
        public Employee Item { get; set; }
        public ItemDetailViewModel(Employee item = null) {
            Title = item?.FullName;
            Item = item;
        }
    }
}
