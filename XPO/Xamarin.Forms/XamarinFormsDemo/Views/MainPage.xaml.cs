using DevExpress.Xpo.DB;
using System;
using System.IO;
using Xamarin.Forms;

namespace XamarinFormsDemo.Views {
     public partial class MainPage : TabbedPage {
        public MainPage() {
            Page ItemsPage, aboutPage = null;

            switch(Device.RuntimePlatform) {
                case Device.iOS:
                    ItemsPage = new NavigationPage(new ItemsPage()) {
                        Title = "Browse"
                    };

                    aboutPage = new NavigationPage(new AboutPage()) {
                        Title = "About"
                    };
                    ItemsPage.IconImageSource = "tab_feed.png";
                    aboutPage.IconImageSource = "tab_about.png";
                    break;
                default:
                    ItemsPage = new ItemsPage() {
                        Title = "Browse"
                    };

                    aboutPage = new AboutPage() {
                        Title = "About"
                    };
                    break;
            }

            Children.Add(ItemsPage);
            Children.Add(aboutPage);

            Title = Children[0].Title;
        }

        protected override void OnCurrentPageChanged() {
            base.OnCurrentPageChanged();
            Title = CurrentPage?.Title ?? string.Empty;
        }
    }
}
