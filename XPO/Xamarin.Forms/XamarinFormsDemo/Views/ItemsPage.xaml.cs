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
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args) {
            var item = args.SelectedItem as Employee;
            if(item == null)
                return;
            UnitOfWork unitOfWork = XpoHelper.CreateUnitOfWork();
            await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel( item.Oid)));

            // Manually deselect item
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e) {
            await Navigation.PushAsync(new NewItemPage());
        }

        async void FilterByDepartment(object sender, EventArgs e) {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if(selectedIndex != -1) {
                await viewModel.LoadEmployeesAsync();
                viewModel.Items= (ObservableCollection<Employee>)viewModel.Items.Where(w=> w.Department == viewModel.Departments[selectedIndex] );
            }
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
