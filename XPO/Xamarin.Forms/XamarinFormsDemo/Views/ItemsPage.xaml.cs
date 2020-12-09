using DevExpress.Xpo;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.ViewModels;

namespace XamarinFormsDemo.Views {
    public partial class ItemsPage : ContentPage {
        ItemsViewModel viewModel;

        public ItemsPage() {
            InitializeComponent();
            BindingContext = viewModel = new ItemsViewModel();
            viewModel.Navigation = Navigation;
        }
        protected override async void OnAppearing() {
            base.OnAppearing();
            if(viewModel.Items.Count == 0) {
                await viewModel.LoadEmployeesAsync();
                await viewModel.LoadDepartmentsAsync();
            } else {
                viewModel.UpdateItems();
            }
        }
    }
}
