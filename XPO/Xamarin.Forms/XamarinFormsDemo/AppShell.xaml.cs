using System;
using System.Collections.Generic;
using Xamarin.Forms;
using XamarinFormsDemo.ViewModels;
using XamarinFormsDemo.Views;

namespace XamarinFormsDemo {
    public partial class AppShell : Xamarin.Forms.Shell {
        public AppShell() {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
//Routing.RegisterRoute(nameof(ItemsPage), typeof(ItemsPage));
        }

    }
}
