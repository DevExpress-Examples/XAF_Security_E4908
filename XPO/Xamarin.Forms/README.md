This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Xamarin Forms application. 
You will also learn how to execute Create, Write and Delete data operations taking into account security permissions.

>For simplicity, the instructions include only C# code snippets. For the complete C# code, see the [CS](CS) sub-directorie.


### Prerequisites
- Add the *XafSolution.Module* assembly reference to your application.

> **!NOTE:** If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

***

## Step 1. Database Connection and Security System Initialization
- In this example we will use WebApi DataStore, which requires server-side API alongside Xamarin application. Read more on how to set-up RESTFull API using XPO here: (Link)

- Turn off Configuration Manager in App.xaml.cs before calling InitXpo method. Configuration Manager is not supported by Xamarin

```csharp

Tracing.UseConfigurationManager = false; 
Tracing.Initialize(3); 

```

### Implement the XpoHelper class


 ```csharp

public static class XpoHelper {
    public static SecuredObjectSpaceProvider objectSpaceProvider;
    public static AuthenticationStandard authentication; 
    public static SecurityStrategyComplex security;
    //...
}

  ```

#### Add `RegisterEnteties` Method
  
  The `RegisterEntities` method needs to initialize the [Types Info](https://docs.devexpress.com/eXpressAppFramework/113669/concepts/business-model-design/types-info-subsystem) 
  system and register the business objects, which you will access from your code.

  ```csharp

private static void RegisterEntities(SecuredObjectSpaceProvider objectSpaceProvider) {
   objectSpaceProvider.TypesInfo.RegisterEntity(typeof(Employee));
   objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyUser));
   objectSpaceProvider.TypesInfo.RegisterEntity(typeof(PermissionPolicyRole));
}

  ```
####  Add `InitSecurity` Method
The GetSecurity method initializes the Security System instance and registers authentication providers.
 ```csharp

static void InitSecurity() {
   authentication = new AuthenticationStandard();
   security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
   security.RegisterXPOAdapterProviders();
}

  ```
####  Add `LogIn` Method

 ```csharp

static void LogIn(string login, string password) {
   authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(login, password));
   IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
   security.Logon(loginObjectSpace);
}

  ```

####  Add `CreateUnitOfWork` Method

 ```csharp

public static UnitOfWork CreateUnitOfWork() {
   var space = objectSpaceProvider.CreateObjectSpace() as XPObjectSpace;
   return space.Session as UnitOfWork;
}

  ```

####  Implement `WebApiDataStoreProvider` Class
*Explanation TODO*
 ```csharp
private class WebApiDataStoreProvider : IXpoDataStoreProvider {
    string fConnectionString;
    public string ConnectionString {
        get => fConnectionString;
    }
    HttpClientHandler GetInsecureHandler() {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        return handler;
    }
    public WebApiDataStoreProvider(string connectionString) {
        fConnectionString = connectionString;
    }

    public IDataStore CreateSchemaCheckingStore(out IDisposable[] disposableObjects) {
        throw new NotImplementedException();
    }

    public IDataStore CreateUpdatingStore(bool allowUpdateSchema, out IDisposable[] disposableObjects) {
        throw new NotImplementedException();
    }

    public IDataStore CreateWorkingStore(out IDisposable[] disposableObjects) {
        HttpClient httpClient = new HttpClient(GetInsecureHandler());
        Uri uri = new Uri(ConnectionString);
        httpClient.BaseAddress = uri;
        disposableObjects = new[] { httpClient };
        return WebApiDataStoreClient(httpClient, AutoCreateOption.SchemaAlreadyExists);
  }
}

  ```
####  Add `InitXpo` Method

 ```csharp

public static void InitXpo(string connectionString, string login, string password) {
    GetDataStore(connectionString);
    RegisterEntities();
    GetSecurity();
    objectSpaceProvider = new SecuredObjectSpaceProvider(security, new WebApiDataStoreProvider());
    LogIn(login, password);
    XpoDefault.Session = null;
}

```
## Step 2. Login Page and ViewModel implementation

- Create two folders in your project: `ViewModels` and `Views`. 
- In the `Views` folder create XAML Page and give it `LoginPage` name. 
- In the `ViewModels` folder create add new class file `LoginViewModel.cs`.
- Insert the code into these files from corresponding sources: `LoginPage.xaml`, `LoginViewModel.cs`

Login Page and ViewModel collect Log In information and pass it to the `InitXpo` method from the `XpoHelper` class.  
## Step 3. Base ViewModel implementation
Every other ViewModel will inherit Base ViewModel.

- Make every ViewModel use it's own UnitOfWork
```csharp
public UnitOfWork uow = XpoHelper.CreateUnitOfWork();
```
- Add INavigation property for navigation purposes
```csharp
INavigation navigation;
public INavigation Navigation {
    get { return navigation; }
    set { SetProperty(ref navigation, value); }
}
```
- Implement standart ViewModel features
```csharp
bool isBusy = false;
public bool IsBusy {
    get { return isBusy; }
    set { SetProperty(ref isBusy, value); }
}

string title = string.Empty;
public string Title {
    get { return title; }
    set { SetProperty(ref title, value); }
}

protected bool SetProperty<T>(ref T backingStore, T value,
    [CallerMemberName] string propertyName = "",
    Action onChanged = null) {
    if(EqualityComparer<T>.Default.Equals(backingStore, value))
        return false;

    backingStore = value;
    onChanged?.Invoke();
    OnPropertyChanged(propertyName);
    return true;
}

#region INotifyPropertyChanged
public event PropertyChangedEventHandler PropertyChanged;
protected void OnPropertyChanged([CallerMemberName] string propertyName = "") {
    var changed = PropertyChanged;
    if(changed == null)
        return;

    changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
```  

  
## Step 4. Items Page and ViewModel implemetation
- In the `Views` folder create new xaml page and call it `ItemsPage.xaml`. 
- In the `ViewModels` folder create new class and call it `ItemsViewModel`
- In the `ItemsViewModel` class:
  - Add Employees Observable Collection 
    ```csharp
    ObservableCollection<Employee> items;
    public ObservableCollection<Employee> Items {
        get { return items; }
        set { SetProperty(ref items, value); }
    }
    ```
  - Add Departments Observable Collection
    ```csharp
    ObservableCollection<Department> departments;
    public ObservableCollection<Department> Departments {
        get { return departments; }
        set { SetProperty(ref departments, value); }
    }
    ```
  - Add `AddItemCommand` and `LoadDataCommand`
    ```csharp
    public Command AddItemCommand { get; set; }
    public Command LoadDataCommand { get; set; }
    ```
  - Add LoadDataCommand logic
     ```csharp
    async Task ExecuteLoadEmployeesCommand() {
        if(IsBusy)
            return;

        IsBusy = true;
        await LoadEmployeesAsync();
        IsBusy = false;
    }
    public async Task LoadEmployeesAsync() {
        try {
            Items.Clear();
            var items = await XpoDataStore.GetEmployeesAsync(uow, true);
            foreach(var item in items) {
                Items.Add(item);
            }
            OnPropertyChanged(nameof(Items));
        } catch(Exception ex) {
            Debug.WriteLine(ex);
        }
    }
    ```
   - Add AddItemsCommand logic
      ```csharp
      async Task ExecuteAddItemCommand() {
          await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel()));
      }
      ```
  - Add constructor
      ```csharp
      public ItemsViewModel() {
          Title = "Browse";
          Items = new ObservableCollection<Employee>();
          LoadDataCommand = new Command(async () => { 
              await ExecuteLoadEmployeesCommand(); 
          });
          AddItemCommand = new Command(async () => {
              await ExecuteAddItemCommand();
          }, ()=> XpoHelper.Security.CanCreate<Employee>());
      }
      ```
- In the `ItemsPage` xaml page
    - Copy structure and bindings from corresponding `ItemsPage.xaml` file
- In the `ItemsPage.xaml.cs` class
    - Bind the `ItemsVievModel` to the `ItemsPage` and Pass `Navigation` to `ItemsViewModel`
      ```csharp
      ItemsViewModel viewModel;
      public ItemsPage() {
          InitializeComponent();
          BindingContext = viewModel = new ItemsViewModel();
          viewModel.Navigation = Navigation;
      }
      ```
    - Override `OnAppearing` method
      ```csharp
      protected override async void OnAppearing() {
          base.OnAppearing();
          if(viewModel.Items.Count == 0) {
              await viewModel.LoadEmployeesAsync();
              await viewModel.LoadDepartmentsAsync();
          } else {
              viewModel.UpdateItems();
          }
      }
      ```
## Step 5. Item Detail page and ViewModel implementation
- In the `Views` folder create new xaml page and call it `ItemDetailPage.xaml`. 
- In the `ViewModels` folder create new class and call it `ItemDetailViewModel`
- In the `ItemDetailViewModel` class:
    - dddd
- In the `ItemDetailPage` xaml page
    - Copy structure and bindings from corresponding `ItemDetailPage.xaml` file
- In the `ItemDetailPage.xaml.cs` class
    - Bind the `ItemDetailViewModel` to the `ItemDetailPage` and Pass `Navigation` to `ItemDetailViewModel`
      ```csharp
      ItemDetailViewModel viewModel;
      public ItemDetailPage(ItemDetailViewModel viewModel) {
          InitializeComponent();
          BindingContext = this.viewModel = viewModel;
          viewModel.Navigation = Navigation;
      }
      ```
    - Add parameter-less constructor requered by Xamarin previewer
      ```csharp
      public ItemsPage() {
          InitializeComponent();
          BindingContext = viewModel = new ItemsViewModel();
          viewModel.Navigation = Navigation;
      }
      ```
## Step 4: Run and Test the App
 - Log in under 'User' with an empty password.
   
 - Notice that secured data is not displayed.

 - Press the Logout button and log in under 'Admin' to see all records.
