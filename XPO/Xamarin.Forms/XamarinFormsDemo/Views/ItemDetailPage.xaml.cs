using System;
using DevExpress.Xpo;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.ViewModels;

namespace XamarinFormsDemo.Views {
    public partial class ItemDetailPage : ContentPage {
        public ItemDetailPage(Guid? Oid) {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel(Oid, Navigation);
        }
    }
}
