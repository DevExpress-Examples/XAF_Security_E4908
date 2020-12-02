using System;
using DevExpress.Xpo;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.ViewModels;

namespace XamarinFormsDemo.Views {
    public partial class ItemDetailPage : ContentPage {
        ItemDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public ItemDetailPage() {
            InitializeComponent();

            viewModel = new ItemDetailViewModel(new Guid());
            BindingContext = viewModel;
        }

        public ItemDetailPage(ItemDetailViewModel viewModel) {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }
        async void Update_Clicked(object sender, EventArgs e) {
            //MessagingCenter.Send(this, "UpdateItem", viewModel.Item);
            await Navigation.PopToRootAsync();
        }
        void OnPickerSelectedIndexChanged(object sender, EventArgs e) {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if(selectedIndex != -1) {
                viewModel.Item.Department = viewModel.Departments[selectedIndex];
            }
        }
        async void Delete_Clicked(object sender, EventArgs e) {
            if(viewModel.Item != null) {
                var result = true;// await DisplayAlert("Delete", $"Are you sure you want to delete the \"{viewModel.Item.FullName}\" item?", "Yes", "No");
                if(result) {
                    //MessagingCenter.Send(this, "DeleteItem", viewModel.Item);
                    await Navigation.PopToRootAsync();
                }
            }
        }
    }
}
