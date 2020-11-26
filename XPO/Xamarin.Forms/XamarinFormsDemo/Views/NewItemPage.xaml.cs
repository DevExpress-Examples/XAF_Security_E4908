using System;
using DevExpress.Xpo;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

namespace XamarinFormsDemo.Views {
    public partial class NewItemPage : ContentPage {
        public Employee Item { get; set; }

        public NewItemPage() {
            InitializeComponent();

            Item = new Employee(XpoDefault.Session) {
                FirstName = "First name",
                LastName = "Last name"
            };

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e) {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
