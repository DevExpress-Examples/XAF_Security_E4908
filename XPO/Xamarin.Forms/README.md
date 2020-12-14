This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Xamarin Forms application. 
You will also learn how to execute Create, Write and Delete data operations taking into account security permissions.

>For simplicity, the instructions include only C# code snippets. For the complete C# code, see the sub-directories.


### Prerequisites
- Add the *XafSolution.Module* assembly reference to your application.

> **!NOTE:** If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

***
## Step 1. Create a Mobile App

1. Open Visual Studio and create a new project.
2. Search for the **Mobile App (Xamarin Forms)** template. 
3. Set the project name to **XamarinFormsDemo** and click **Create**.
4. Select the **Master-Detail** application template and click **OK**.
5. In the **Solution Explorer**, remove the unnecessary files:

    * *Models\HomeMenuItem.cs*
    * *Services\MockDataStore.cs*
    * *Views\MenuPage.xaml*

For more information, see the following:

- [Build your first Xamarin.Forms App](https://docs.microsoft.com/en-us/xamarin/get-started/first-app/)
- [Xamarin.Forms - Quick Starts](https://docs.microsoft.com/en-us/xamarin/get-started/quickstarts/)



## Step 2. Add the NuGet Packages

The application you build in this tutorial requires the following NuGet Packages:

- DevExpress.ExpressApp.Security.Xpo
- DevExpress.ExpressApp.Validation
- DevExpress.Persistent.BaseImpl
- Microsoft.Data.SqlClient

From Visual Studio's **Tools** menu, select **NuGet Package Manager > Package Manager Console**.

Make sure **Package source** is set to **All** or **nuget.org** and run the following commands: 

```console
Install-Package DevExpress.ExpressApp.Security.Xpo
```
```console
Install-Package DevExpress.ExpressApp.Validation
```
```console
Install-Package Persistent.BaseImpl
```
```console
Install-Package Microsoft.Data.SqlClient
```
## Step 3. Database Connection and Security System Initialization
- In this example we will use WebApi DataStore, which requires server-side API alongside Xamarin application. Read more on how to set-up RESTFull API using XPO here: (Link)

- Turn off Configuration Manager in App.xaml.cs before calling InitXpo method. Configuration Manager is not supported by Xamarin

```csharp
//..
Tracing.UseConfigurationManager = false; 
Tracing.Initialize(3); 
//..
```

### Implement the XpoHelper class


 ```csharp

public static class XpoHelper {
    static SecuredObjectSpaceProvider ObjectSpaceProvider;
    static AuthenticationStandard Authentication; 
    public static SecurityStrategyComplex Security;
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
   Security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
   Security.RegisterXPOAdapterProviders();
}

  ```
####  Add `LogIn` Method

 ```csharp
static void LogIn(string login, string password) {
   authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(login, password));
   IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
   Security.Logon(loginObjectSpace);
}
```

####  Add `CreateUnitOfWork` Method

 ```csharp

public static UnitOfWork CreateUnitOfWork() {
    var space = (XPObjectSpace)ObjectSpaceProvider.CreateObjectSpace();
    return (UnitOfWork)space.Session;
}

  ```

####  Implement `CreateWebApiDataStoreFromString` Method
When working with Android or IOS, SSL configuration is required. Currently `ObjectSpaceProvider` doesn't have parameters, provided alongside with Connection string, to configure HttpClient. Because of that we need to register custom `CreateDataStoreFromString` method. To work with self signed sertificate during debugging, using of `InsecureHandler` is required.
 ```csharp
static IDataStore CreateWebApiDataStoreFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
    ConnectionStringParser parser = new ConnectionStringParser(connectionString);
    if(!parser.PartExists("uri"))
        throw new ArgumentException("Connection string does not contain the 'uri' part.");
    string uri = parser.GetPartByName("uri");
    HttpClient client = new HttpClient(GetInsecureHandler());
    client.BaseAddress = new Uri(uri);
    objectsToDisposeOnDisconnect = new IDisposable[] { client };
    return new WebApiDataStoreClient(client, autoCreateOption);
}
static HttpClientHandler GetInsecureHandler() {
    HttpClientHandler handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
    return handler;
}
  ```
####  Add `InitXpo` Method

 ```csharp

public static void InitXpo(string connectionString, string login, string password) {
    GetDataStore(connectionString);
    RegisterEntities();
    GetSecurity();
    XpoDefault.RegisterBonusProviders();
    DataStoreBase.RegisterDataStoreProvider(WebApiDataStoreClient.XpoProviderTypeString, CreateWebApiDataStoreFromString);
    ObjectSpaceProvider = new SecuredObjectSpaceProvider(Security, connectionString, null);
    LogIn(login, password);
    XpoDefault.Session = null;
}

```
## Step 4. Login Page and ViewModel implementation

- In the `Views` folder create XAML Page and give it `LoginPage` name. 
- In the `ViewModels` folder create add new class file `LoginViewModel.cs`.
- Insert the code into these files from corresponding sources: [LoginPage.xaml](XamarinFormsDemo/Views/LoginPage.xaml), [LoginViewModel.cs](XamarinFormsDemo/ViewModels/LoginViewModel.cs)

Login Page and ViewModel collect Log In information and pass it to the `InitXpo` method from the `XpoHelper` class.  

## Step 5. Base ViewModel implementation
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

  
## Step 6. Items Page and ViewModel implemetation
- In the `Views` folder create new xaml page and call it `ItemsPage.xaml`. 
- In the `ViewModels` folder create new class and call it `ItemsViewModel`
  
To create Items Page we have to implement ListView with the list of items, filter for the ListView, and Add button in the Toolbar
- ListView
    
    To implement ListView we will place data and commands in the `ItemsViewModel` class. 
    ```csharp
    public ItemsViewModel(INavigation _navigation):base(_navigation) {
          Title = "Browse";
          Items = new ObservableCollection<Employee>();
          ExecuteLoadEmployeesCommand();
          LoadDataCommand = new Command(() => { 
              ExecuteLoadEmployeesCommand(); 
              //..
          });
          //..
    }
    ObservableCollection<Employee> items;
    public ObservableCollection<Employee> Items {
        get { return items; }
        set { SetProperty(ref items, value); }
    }
    public Command LoadDataCommand { get; set; }
    void ExecuteLoadEmployeesCommand() {
        if(IsBusy)
            return;

        IsBusy = true;
        LoadEmployees();
        IsBusy = false;
    }
    public void LoadEmployees() {
        try {
            var items = uow.Query<Employee>().OrderBy(i => i.FirstName).ToList();
            Items = new ObservableCollection<Employee>(items);
        } catch(Exception ex) {
            Debug.WriteLine(ex);
        }
    }
    
    Employee selectedItem;
    public Employee SelectedItem {
            get { return selectedItem; }
            set { 
                SetProperty(ref selectedItem, value); 
                if(value != null) ExecuteSelectItem(); 
            }
        }
    void ExecuteSelectItem() {
            if(SelectedItem == null)
                return;
            var tempGuid = SelectedItem.Oid;
            SelectedItem = null;
            Navigation.PushAsync(new ItemDetailPage(tempGuid));
        }
    ```
    In the `ItemsPage.xaml` file use following format and bindings. Please note that the order of the  paramenters matters.

    ```xaml
    <ListView x:Name="ItemsListView" 
            ItemsSource="{Binding Items}"
            VerticalOptions="FillAndExpand"
            HasUnevenRows="true"
            RefreshCommand="{Binding LoadDataCommand}"
            IsPullToRefreshEnabled="true"
            IsRefreshing="{Binding IsBusy, Mode=OneWay}"
            CachingStrategy="RecycleElement"
            SelectedItem="{Binding SelectedItem, Mode=TwoWay}">
        <ListView.ItemTemplate>
            <DataTemplate>
                <ViewCell>
                    <StackLayout Orientation="Horizontal">
                        <StackLayout Padding="10" HorizontalOptions="FillAndExpand">
                            <Label Text="{Binding FullName}" 
                    LineBreakMode="NoWrap" 
                    Style="{DynamicResource ListItemTextStyle}" 
                    FontSize="16" />
                            <Label Text="{Binding Department}" 
                    LineBreakMode="NoWrap"
                    Style="{DynamicResource ListItemDetailTextStyle}"
                    FontSize="13" />
                        </StackLayout>
                    </StackLayout>
                </ViewCell>
            </DataTemplate>
        </ListView.ItemTemplate>
    </ListView>
    ```

    In the corresponding `ItemsPage.xaml.cs` file modify Constructor to add `BindingContext`
    ```csharp
    public ItemsPage() {
        InitializeComponent();
        BindingContext  = new ItemsViewModel(Navigation);
    } 
    ```
    


- Add item button
    
    If button or action availability depends on security rights, that button will be binded to command with `canExecute` parameneter, which depends on user's security rights.
    In this example only `Admin` can add new items to the data. `AddItemCommand` executability is determined by `Security.CanCreate()` method.
    
    In the `ItemsViewModel` class 
    Add AddItemsCommand  and its logic

    ```csharp
      public ItemsViewModel() {
          //...
          AddItemCommand = new Command(async () => {
              await ExecuteAddItemCommand();
          }, ()=> XpoHelper.Security.CanCreate<Employee>());
      }
      public Command AddItemCommand { get; set; }
      async Task ExecuteAddItemCommand() {
          await Navigation.PushAsync(new ItemDetailPage(new ItemDetailViewModel()));
      }
    ```
    In the `ItemsPage` xaml page add ToolBar item with following text and binding
    ```xaml
    <ContentPage.ToolbarItems>
        <ToolbarItem Text="Add" Command="{Binding AddItemCommand}" />
    </ContentPage.ToolbarItems>
    ```
- Add Department filter

    Since Departments are specific values, we need to load list of departments. To apply filter we will use `Picker`. 

    
    In the `ItemsViewModel` class add Observable collection of departments and add load logic to LoadData command
    ```csharp
    ObservableCollection<Department> departments;
    public ObservableCollection<Department> Departments {
        get { return departments; }
        set { SetProperty(ref departments, value); }
    }
    
    Department selectedDepartment;
    public Department SelectedDepartment {
        get { return selectedDepartment; }
        set { SetProperty(ref selectedDepartment, value); FilterByDepartment(); }
    }

    void FilterByDepartment() {
        if(SelectedDepartment != null) {
            LoadEmployees();
            var items = Items.Where(w => w.Department == SelectedDepartment);
            Items = new ObservableCollection<Employee>(items);
        } else {
            LoadEmployees();
        }
    }

    public ItemsViewModel(INavigation _navigation):base(_navigation) {
        //..
        Departments = new ObservableCollection<Department>();
        //..
        ExecuteLoadDepartmentsCommand();
        LoadDataCommand = new Command(() => { 
            //..
            ExecuteLoadDepartmentsCommand();
        });
        //..
    }

    public void LoadDepartments() {
        try {
            var items = uow.Query<Department>().ToList();
            Departments = new ObservableCollection<Department>(items);
        } catch(Exception ex) {
            Debug.WriteLine(ex);
        }
    }

    void ExecuteLoadDepartmentsCommand() {
        if(IsBusy)
            return;

        IsBusy = true;
        LoadDepartments();
        IsBusy = false;
    }
    ```
    In the `ItemsPage.xaml` file add picker item on top of the `ListView` with the following parameters
    ```xaml
    <Picker Title="Filter" ItemsSource="{Binding Departments}" SelectedItem="{Binding SelectedDepartment}"/>
    ```


## Step 7. Item Detail page and ViewModel implementation
- In the `Views` folder create new xaml page and call it `ItemDetailPage.xaml`. 
- In the `ViewModels` folder create new class and call it `ItemDetailViewModel`

We have same Page and ViewModel for both editing existing items and creating new items. The page will have `Delete` and `Save` toolbar items and `Grid` to display item's data, featuring `Picker` to select department.

- `Grid`

    In the `ItemDetailViewModel` class add 

    ```csharp
    public Employee Item { get; set; }


    bool isNewItem;
    public bool IsNewItem {
        get { return isNewItem; }
        set { SetProperty(ref isNewItem, value); }
    }
    public ItemDetailViewModel(Guid? Oid,INavigation navigation):base(navigation) {
        IsNewItem = (Oid == null);
        if(isNewItem) {
            Item = new Employee(uow) { FirstName = "First name", LastName = "Last Name" };
        } else {
            Item = uow.GetObjectByKey<Employee>(Oid);
        }
        Title = Item?.FullName;
        
        //..
    }
    ```
    In the `ItemDetailPage.xaml` add `Grid` with following parameters
    ```
    <Grid ColumnSpacing="20" Padding="15">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>
        <Label Text="FirstName" FontSize="Medium" Grid.Row="0" Grid.Column="0" />
        <Entry Text="{Binding Item.FirstName}" IsReadOnly="{Binding  ReadOnly}" FontSize="Small" Margin="0" Grid.Row="1" Grid.Column="0"  />
        <Label Text="LastName" FontSize="Medium" Grid.Row="2" Grid.Column="0" />
        <Entry Text="{Binding Item.LastName}" IsReadOnly="{Binding  ReadOnly}" FontSize="Small" Margin="0" Grid.Row="3" Grid.Column="0"  />
        <Label Text="Department" IsVisible="{Binding CanReadDepartment}"  FontSize="Medium" Grid.Row="4" Grid.Column="0" />
        <Picker IsVisible="{Binding CanReadDepartment}" IsEnabled="{Binding CanReadDepartment}" ItemsSource="{Binding Departments}" ItemDisplayBinding="{Binding Title}" SelectedItem="{Binding Item.Department}" FontSize="Small" Margin="0" Grid.Row="5" Grid.Column="0"/>
    </Grid>
    ```
- `Picker`

    If user's rights allow to modify the `Department` property, `Picker` with selectable options is shown. In the `ItemDetailVeiwModel` class add following code
    ```csharp
    
    List<Department> departments;
    public List<Department> Departments {
        get { return departments; }
        set { SetProperty(ref departments, value); }
    }
    
    bool canReadDepartment;
    public bool CanReadDepartment {
        get { return canReadDepartment; }
        set { SetProperty(ref canReadDepartment, value); }
    }
    public ItemDetailViewModel(Guid? Oid,INavigation navigation):base(navigation) {
        //..
        Departments = uow.Query<Department>().ToList();
        CanReadDepartment = XpoHelper.Security.CanRead(Item, "Department");
        CanWriteDepartment = XpoHelper.Security.CanWrite(Item, "Department");
        if (isNewItem && CanWriteDepartment) {
            Item.Department = Departments?[0];
        }
    }
    ```

- Buttons

    `Save` and `Delete` buttons availability depends on security. We will bind them to commands, so we can control is button active or not.

    In the `ItemDetailViewModel` class add commands and security properties
    ```csharp
    bool readOnly;
    public bool ReadOnly {
            get { return readOnly; }
            set { SetProperty(ref readOnly, value); CommandUpdate.ChangeCanExecute(); }
        }
    bool canDelete;
    public bool CanDelete {
            get { return canDelete; }
            set { SetProperty(ref canDelete, value); CommandDelete.ChangeCanExecute(); }
        }
    public Command CommandDelete { get; private set; }
    public Command CommandUpdate { get; private set; }

    public ItemDetailViewModel(Guid? Oid,INavigation navigation):base(navigation) {
        //..
        CommandDelete = new Command(async () => {
            await DeleteItemAndGoBack();
        },
    () => CanDelete && !isNewItem);
        CommandUpdate = new Command(async () => {
            await SaveItemAndGoBack();
        },
    () => !ReadOnly);
        CanDelete = XpoHelper.Security.CanDelete(Item);
        ReadOnly = !XpoHelper.Security.CanWrite(Item);
        //..
    }
    async Task DeleteItemAndGoBack() {
        uow.Delete(Item);
        await uow.CommitChangesAsync();
        await Navigation.PopToRootAsync();
    }
    async Task SaveItemAndGoBack() {
        uow.Save(Item);
        await uow.CommitChangesAsync();
        await Navigation.PopToRootAsync();
    }
    ```
    In the `ItemDetailPage.xaml` add Toolbar items with following parameters
    ```xaml
    <ContentPage.ToolbarItems>
        <ToolbarItem Text="Delete" Command="{Binding CommandDelete}" />
        <ToolbarItem Text="Save" Command="{Binding CommandUpdate}"  />
    </ContentPage.ToolbarItems>
    ```
    Finally add constructor to the `ItemsDetailPage.xaml.cs` class to bind ViewModel and Page together.
    ```csharp
    public ItemDetailPage(Guid? Oid) {
        InitializeComponent();
        BindingContext = new ItemDetailViewModel(Oid, Navigation);
    }
    ```
## Step 8. Populate the Databse
To seed the data in the database, Add `UpdateDataBase` method, call it in the `InitXpo` method. Also add correspongding files into the `Core` folder: [Employees.xml](XamarinFormsDemo/Core/Employees.xml) , [DBUpdater.cs](XamarinFormsDemo/Core/DBUpdater.cs)

```csharp
public static void InitXpo(string connectionString, string login, string password) {
    //..
    UpdateDataBase();
    LogIn(login, password);
    //..
}
static void UpdateDataBase() {
    var space = ObjectSpaceProvider.CreateUpdatingObjectSpace(true);
    Updater updater = new Updater(space);
    updater.UpdateDatabase();
}
```
## Step 9. Run and Test the App
 - Log in under 'User' with an empty password.
   
 - Notice that secured data is not displayed.

 - Press the Logout button and log in under 'Admin' to see all records.
