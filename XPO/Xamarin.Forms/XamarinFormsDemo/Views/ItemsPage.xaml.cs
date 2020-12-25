using System;
using System.Collections.ObjectModel;
using System.Linq;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.ViewModels;

namespace XamarinFormsDemo.Views {
    public partial class ItemsPage : ContentPage {

        public ItemsPage() {
            InitializeComponent();
            BindingContext  = new ItemsViewModel(Navigation);
        } 
    }
}
