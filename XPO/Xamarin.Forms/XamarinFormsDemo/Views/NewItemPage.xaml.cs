using System;
using DevExpress.Xpo;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

namespace XamarinFormsDemo.Views {
    public partial class NewItemPage : ContentPage {
        public Employee Item { get; set; }

        public NewItemPage() {
            InitializeComponent();
            var uow = XpoHelper.CreateUnitOfWork();
            Item = new Employee(uow) {
                FirstName = "First name",
                LastName = "Last name",
                Department = new Department(uow) { Title = "A", Office = "B" }
            };


            BindingContext = this;


        }

        async void Save_Clicked(object sender, EventArgs e) {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
        
        
    }
}
