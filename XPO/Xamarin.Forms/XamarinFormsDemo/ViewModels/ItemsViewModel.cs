using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;
using Xamarin.Forms;

using XamarinFormsDemo.Views;

namespace XamarinFormsDemo.ViewModels {
    public class ItemsViewModel : BaseViewModel {
        public ObservableCollection<Employee> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ItemsViewModel() {
            Title = "Browse";
            Items = new ObservableCollection<Employee>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewItemPage, Employee>(this, "AddItem", async (obj, item) => {
                var _item = item as Employee;
                Items.Add(_item);
                await DataStore.AddItemAsync(_item);
            });
            MessagingCenter.Subscribe<ItemsPage, Employee>(this, "DeleteItem", async (obj, item) => {
                var _item = item as Employee;
                Items.Remove(_item);
                await DataStore.DeleteItemAsync(_item.Oid);
            });
            MessagingCenter.Subscribe<ItemDetailPage, Employee>(this, "UpdateItem", async (obj, item) => {
                var _item = item as Employee;
                Items.Remove(Items.Single(i => i.Oid == _item.Oid));
                Items.Add(_item);
                await DataStore.UpdateItemAsync(_item);
            });
        }

        async Task ExecuteLoadItemsCommand() {
            if(IsBusy)
                return;

            IsBusy = true;
            await LoadItemsAsync();
            IsBusy = false;
        }

        public void UpdateItems() {
            OnPropertyChanged(nameof(Items));
        }

        public async Task LoadItemsAsync() {
            try {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach(var item in items) {
                    Items.Add(item);
                }
            } catch(Exception ex) {
                Debug.WriteLine(ex);
            }
        }
    }
}
