This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Xamarin Forms application. 

From this tutorial, you will learn how to perform CRUD (create-read-update-delete) operations with respect to security permissions. 

> You can find the complete project code in the sub-directories.


### Prerequisites
- [Visual Studio 2019](https://visualstudio.microsoft.com/vs/) with the **Mobile development with .NET** workload.
- (Optional) A paired Mac to build the application on iOS.

> **NOTE:** If you have a pre-release version of our components, for example, provided with the hotfix, you also have a pre-release version of NuGet packages. These packages will not be restored automatically and you need to update them manually as described in the [Updating Packages](https://docs.devexpress.com/GeneralInformation/118420/Installation/Install-DevExpress-Controls-Using-NuGet-Packages/Updating-Packages) article using the [Include prerelease](https://docs.microsoft.com/en-us/nuget/create-packages/prerelease-packages#installing-and-updating-pre-release-packages) option.

***

## Step 1. Create a Mobile App

1. Open Visual Studio and create a new project.
2. Search for the **Mobile App (Xamarin Forms)** template. 
3. Specify the project name (we use "_XamarinFormsDemo_" in this demo project) and click **Create**.
4. Select an application template (we use the **Tabbed** template in this demo project) and click **OK**.

> **Getting started with XamarinForms**
> - [Build your first Xamarin.Forms App](https://docs.microsoft.com/en-us/xamarin/get-started/first-app/)
> - [Xamarin.Forms - Quick Starts](https://docs.microsoft.com/en-us/xamarin/get-started/quickstarts/)



## Step 2. Add the NuGet Packages

The application you will build in this tutorial requires the following NuGet Packages:

- DevExpress.ExpressApp.Security.Xpo
- DevExpress.ExpressApp.Validation
- DevExpress.Persistent.BaseImpl

From Visual Studio's **Tools** menu, select **NuGet Package Manager > Package Manager Console**.

Make sure of the following:
- **Package source** is set to **All** or **DevExpress XX.Y Local**
- Default project is set to a class library project (_XamarinFormsDemo_ in this example) 

Run the following commands: 

```console
Install-Package DevExpress.ExpressApp.Security.Xpo
```
```console
Install-Package DevExpress.ExpressApp.Validation
```
```console
Install-Package Persistent.BaseImpl
```

## Step 3. Add XPO Model

### Scenario #1
To reuse existing data models and Security System settings (users, roles and permissions) stored in an XAF application database, add a reference to the [YourXafProject].Module.NetCore project. We use the following project in this tutorial: [XafSolution.Module.NetCore](https://github.com/DevExpress-Examples/XAF_Security_E4908/blob/master/XPO/XafSolution/XafSolution.Module/XafSolution.Module.NetCore.csproj). See also: [Port an Existing XAF Application to .NET Core 3.0+](https://docs.devexpress.com/eXpressAppFramework/401264/net-core-3-0-support-in-winforms-application/port-an-application-to-net-core?p=netcore).

### Scenario #2
To create a new data model, use [XPO Data Model Wizard](https://docs.devexpress.com/XPO/14810/design-time-features/data-model-wizard). In this scenario, you can re-use built-in classes (PermissionPolicyUser, PermissionPolicyRole) or create custom security objects. Refer to the following help topics for additional information:
  - [How to: Implement a Custom Security System User Based on an Existing Business Class](https://docs.devexpress.com/eXpressAppFramework/113452/task-based-help/security/how-to-implement-a-custom-security-system-user-based-on-an-existing-business-class).
  - [How to: Implement Custom Security Objects (Users, Roles, Operation Permissions)](https://docs.devexpress.com/eXpressAppFramework/113384/task-based-help/security/how-to-implement-custom-security-objects-users-roles-operation-permissions).

## Step 4. Database Connection and Security System Initialization

A mobile application does not have direct access to server-based resources, such as a database. In this demo, we will use an intermediate Web API service to communicate with a remote database server. Add an [ASP.NET Core WebApi](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api) application to your project and follow this tutorial: [Transfer Data via REST API](https://docs.devexpress.com/XPO/402182/connect-to-a-data-store/transfer-data-via-rest-api).


### Implement the XpoHelper class

The static `XpoHelper` class exposes the following members:
  - The `CreateUnitOfWork` method - returns a new [UnitOfWork](https://docs.devexpress.com/XPO/DevExpress.Xpo.UnitOfWork) instance connected to a secured [Object Access Layer](https://docs.devexpress.com/XPO/2123/connect-to-a-data-store#object-access-layer).
  - The `SecuritySystem` property - returns a [SecurityStrategyComplex](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.SecurityStrategyComplex) object. Use this object with the following extension methods to check user permissions: [IsGrantedExtensions Methods](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.IsGrantedExtensions._methods).

#### Add the XpoHelper class

1. Remove the following line in the App.xaml.cs file:
    ```csharp
    // Remove this line:
    DependencyService.Register<MockDataStore>();
    ```
2. Replace the IDataStore.cs and MockDataStore.cs files in the Services folder with the XpoHelper.cs file.
    ```csharp
    using DevExpress.ExpressApp.Security;
    using DevExpress.Xpo;

    namespace XamarinFormsDemo.Services {
        public static class XpoHelper {
            static readonly SecurityStrategyComplex fSecurity;
            public static SecurityStrategyComplex Security {
                get {
                    if(fSecurity == null) {
                        throw new InvalidOperationException("The security system is not initialized. Call the InitSecuritySystem method first.");
                    }
                    return fSecurity; 
                }
            }
            public static UnitOfWork CreateUnitOfWork() {
                throw new System.NotImplementedException();
            }
        }
    }
    ```
2. Add a static constructor and initialize XAF Security System.
    ```csharp
    using DevExpress.ExpressApp.Xpo;
    using DevExpress.ExpressApp;
    using XafSolution.Module.BusinessObjects;
    using DevExpress.Persistent.BaseImpl.PermissionPolicy;
    // ...
    static XpoHelper() {
        RegisterEntities();
        fSecurity = InitSecuritySystem();
    }
    static void RegisterEntities() {
        XpoTypesInfoHelper.GetXpoTypeInfoSource();
        XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
        XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
        XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
    }
    static SecurityStrategyComplex InitSecuritySystem() {
        AuthenticationStandard authentication = new AuthenticationStandard();
        var security = new SecurityStrategyComplex(
            typeof(PermissionPolicyUser),
            typeof(PermissionPolicyRole),
            authentication);
        security.RegisterXPOAdapterProviders();
        return security;
    }
    ```
3. Add the following lines to the static constructor to configure XAF Tracing System and disable configuration manager, which is not supported by Xamarin.
    ```csharp
    using DevExpress.Persistent.Base;
    // ...
    Tracing.UseConfigurationManager = false;
    Tracing.Initialize(3);
    ```
4. To use a self-signed SSL certificate for development, add the following methiod and call it in the static constructor.
    ```csharp
    using DevExpress.Xpo.DB;
    using System;
    using DevExpress.Xpo.DB.Helpers;
    using System.Net.Http;
    // ...
    #if DEBUG
    ConfigureXpoForDevEnvironment();
    #endif
    // ...
    static void ConfigureXpoForDevEnvironment() {
        XpoDefault.RegisterBonusProviders();
        DataStoreBase.RegisterDataStoreProvider(WebApiDataStoreClient.XpoProviderTypeString, CreateWebApiDataStoreFromString);
    }
    static IDataStore CreateWebApiDataStoreFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
        ConnectionStringParser parser = new ConnectionStringParser(connectionString);
        if(!parser.PartExists("uri"))
            throw new ArgumentException("The connection string does not contain the 'uri' part.");
        string uri = parser.GetPartByName("uri");
        HttpClient client = new HttpClient(GetInsecureHandler());
        client.BaseAddress = new Uri(uri);
        objectsToDisposeOnDisconnect = new IDisposable[] { client };
        return new WebApiDataStoreClient(client, autoCreateOption);
    }
    /// <summary>
    /// Disables an SSL sertificate validation to support self-signed developer certificates.
    /// </summary>
    /// <returns></returns>
    static HttpClientHandler GetInsecureHandler() {
        HttpClientHandler handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        return handler;
    }
5. Add the `GetObjectSpaceProvider`, `Logon`, and `Logoff` methods.
    ```csharp
    using DevExpress.ExpressApp.Security.ClientServer;
    // ...
    const string ConnectionString = @"XpoProvider=WebApi;uri=https://localhost:5001/xpo/";
    static IObjectSpaceProvider ObjectSpaceProvider;
    static IObjectSpaceProvider GetObjectSpaceProvider() {
        if(ObjectSpaceProvider == null) {
            ObjectSpaceProvider = new SecuredObjectSpaceProvider(Security, ConnectionString, null);
        }
        return ObjectSpaceProvider;
    }
    public static void Logon(string userName, string password) {
        var logonParameters = new AuthenticationStandardLogonParameters(userName, password);
        Security.Authentication.SetLogonParameters(logonParameters);
        IObjectSpace logonObjectSpace = GetObjectSpaceProvider().CreateObjectSpace();
        Security.Logon(logonObjectSpace);
    }
    public static void Logoff() {
        Security.Logoff();
    }
    ```
6. Implement the `CreateUnitOfWork` method.
    ```csharp
    public static UnitOfWork CreateUnitOfWork() {
        var space = (XPObjectSpace)GetObjectSpaceProvider().CreateObjectSpace();
        return (UnitOfWork)space.Session;
    }
    ```
    
## Step 5. Base ViewModel and XPO view model implementation

Change the ViewModels\BaseViewModel.cs file as follows.

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using DevExpress.Xpo;

using XamarinFormsDemo.Services;

namespace XamarinFormsDemo.ViewModels {
    public class BaseViewModel : INotifyPropertyChanged {
        
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
        #endregion
    }
}
```  

add new class to the `ViewModels` folder, name it `XpoViewModel` and change it accordingly:

```csharp
using DevExpress.Xpo;
using Xamarin.Forms;
using XamarinFormsDemo.Services;
using XamarinFormsDemo.Views;

namespace XamarinFormsDemo.ViewModels {
    public class XpoViewModel : BaseViewModel {
        UnitOfWork unitOfWork;
        protected UnitOfWork UnitOfWork {
            get {
                if(unitOfWork == null) {
                    unitOfWork = XpoHelper.CreateUnitOfWork();
                }
                return unitOfWork;
            }
        }
        public XpoViewModel() {
            if(!XpoHelper.Security.IsAuthenticated) {
                App.ResetMainPage();
            }
        }
    }
}
```

Make every other ViewModel, except `LogIn`, inherit `XpoViewModel` instead of `BaseViewModel`.

## Step 6. Login Page and ViewModel implementation

1. In the ViewModels/LoginViewModel.cs file, add the `UserName` and `Password` properties, and change the `OnLoginClicked` method.

    ```csharp
    string userName;
    public string UserName {
        get { return userName; }
        set { SetProperty(ref userName, value); }
    }

    string password;
    public string Password {
        get { return password; }
        set { SetProperty(ref password, value); }
    }

    private async void OnLoginClicked(object obj) {
        try {
            XpoHelper.Logon(UserName, Password);
            await Shell.Current.GoToAsync($"{nameof(ItemsPage)}");
        } catch(Exception ex) {
            await Shell.Current.DisplayAlert("Login failed", ex.Message, "Try again");
        }
    }
    ```
    
2. In the Views/LoginPage.xaml file, add the `UserName` and `Password` fields.

    ```xaml
    <ContentPage.Content>
        <Grid ColumnSpacing="20" Padding="15">
            <Grid.RowDefinitions>
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
            <Label Text="Login" FontSize="Medium" Grid.Row="0" Grid.Column="0" />
            <Entry Text="{Binding UserName}" FontSize="Small" Margin="0" Grid.Row="1" Grid.Column="0"  />
            <Label Text="Password" FontSize="Medium" Grid.Row="2" Grid.Column="0" />
            <Entry Text="{Binding Password}" IsPassword="True" FontSize="Small" Margin="0" Grid.Row="3" Grid.Column="0"  />
            <Button Text="Log In" Command="{Binding LoginCommand}" BackgroundColor="#ff7200" TextColor="White" FontSize="Medium" Margin="0" Grid.Row="4" Grid.Column="0"/>
        </Grid>
    </ContentPage.Content>
    ```  
3. In the `App.xaml.cs` change shell creation accordingly:

    - add `ResetMainPage` method
        ```csharp
        public static Task ResetMainPage() {
            Current.MainPage = new AppShell();
            return Shell.Current.GoToAsync("//LoginPage");
        }
        ``` 
    - Call `ResetMainPage()` in the `App` class constructor instead of `MainPage = new AppShell();`
    - In the `AppShell.xaml` file add routing parameter to the `Browse` shell content

        ```xaml
        <ShellContent Title="Browse" Icon="icon_feed.png" Route="ItemsPage" ContentTemplate="{DataTemplate local:ItemsPage}" />
        ```

## Step 6. Items Page and ViewModel ImplemeNtation

Change the _ViewModels\ItemsViewModel.cs_ and _ViewModels\ItemsPage.xaml_ files to implement a ListView with the list of items, a filter bar, and Toolbar items.

1. Create the properties and commands in the `ItemsViewModel` class. 
    ```csharp
    public ItemsViewModel() {
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
            var items = UnitOfWork.Query<Employee>().OrderBy(i => i.FirstName).ToList();
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
    async Task ExecuteSelectItem() {
            if(SelectedItem == null)
                return;
            var tempGuid = SelectedItem.Oid;
            SelectedItem = null;
            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?itemGuid={tempGuid.Tostring()}");
        }
    ```
    In the `ItemsPage.xaml` file, add the ListView component with a custom DataTemplate instead of refreshing collection. 

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
- Add the _Add Item_ and _Log Out_ buttons.
    
    Note: In the command's `canExecute` function, you can use the [IsGrantedExtensions Methods](https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Security.IsGrantedExtensions._methods) to disable a command if the current user is not authorized to do the corresponding opeation.

    ```csharp
    public ItemsViewModel() {
        //...
        AddItemCommand = new Command(async () => {
            await ExecuteAddItemCommand();
        }, ()=> XpoHelper.Security.CanCreate<Employee>());
        LogOutCommand = new Command(() => ExecuteLogOutCommand());
    }
    public Command AddItemCommand { get; set; }
    async Task ExecuteAddItemCommand() {
        await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?itemGuid=");
    }
    public Command LogOutCommand { get; set; }
    async Task ExecuteLogOutCommand() {
        XpoHelper.Logoff();
        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
    }
    
    ```
    In the ItemsPage.xaml file, add the following ToolBar items.

    ```xaml
    <ContentPage.ToolbarItems>
        <ToolbarItem Text="Add" Command="{Binding AddItemCommand}" />
        <ToolbarItem Text="LogOut" Command="{Binding LogOutCommand}" />
    </ContentPage.ToolbarItems>
    ```
- Implement the filter bar.

    A user may want to see a list of employees from a specific depertment. To implement this, put a Picker control and bind it to the `Department` and `SelectedDepartments` ViewModel properties.

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

    public ItemsViewModel() {
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
            var items = UnitOfWork.Query<Department>().ToList();
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
    ```xaml
    <StackLayout>
        <Picker Title="Filter" ItemsSource="{Binding Departments}" SelectedItem="{Binding SelectedDepartment}"/>
        <ListView />
    </StackLayout>
    ```


## Step 7. Item Detail page and ViewModel implementation

Change the _ViewModels\ItemDetailViewModel.cs_ and _ViewModels\ItemDetailPage.xaml_ files as shown below.

- In the `ItemDetailViewModel` class add 

    ```csharp
    [QueryProperty(nameof(ItemGuid), "itemGuid")]
    public class ItemDetailViewModel : XpoViewModel {
        Employee item;
        public Employee Item {
            get { return item; }
            set { SetProperty(ref item, value); }
        }

        bool isNewItem;
        public bool IsNewItem {
            get { return isNewItem; }
            set { SetProperty(ref isNewItem, value); }
        }
        Guid? oid;
        public Guid? Oid {
            get { return Oid; }
            set { SetProperty(ref Oid, value); Load();}
        }
        public Load() {
            //..
            IsNewItem = (Oid == null);
            if(isNewItem) {
                Item = new Employee(UnitOfWork) { FirstName = "First name", LastName = "Last Name" };
                Title = "New employee";
            } else {
                Item = UnitOfWork.GetObjectByKey<Employee>(Oid);
                Title = Item?.FullName;
            }
            //..
        }
    }
    ```
    In the `ItemDetailPage.xaml` add a `Grid` with following parameters
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

    If a user is allowed to modify the `Department` property, a `Picker` with selectable options is shown. In the `ItemDetailVeiwModel` class add following code:
    
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
    public Load() {
        Departments = UnitOfWork.Query<Department>().ToList(); //Has to be the first line;
        //..
        CanReadDepartment = XpoHelper.Security.CanRead(Item, "Department");
        CanWriteDepartment = XpoHelper.Security.CanWrite(Item, "Department");
        if (isNewItem && CanWriteDepartment) {
            Item.Department = Departments?[0];
        }
    }
    ```

- Buttons

    `Save` and `Delete` buttons availability depends on security. We will bind them to commands, so we can control whether a button is active.

    In the `ItemDetailViewModel` class, add commands and security properties:
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

    public ItemDetailViewModel() {
        CommandDelete = new Command(async () => {
            await DeleteItemAndGoBack();
        },
    () => CanDelete && !isNewItem);
        CommandUpdate = new Command(async () => {
            await SaveItemAndGoBack();
        },
    () => !ReadOnly);
    }
    public Load(){
        //..
        CanDelete = XpoHelper.Security.CanDelete(Item);
        ReadOnly = !XpoHelper.Security.CanWrite(Item);
        //..
    }
    async Task DeleteItemAndGoBack() {
        UnitOfWork.Delete(Item);
        await UnitOfWork.CommitChangesAsync();
        await Shell.Current.Navigation.PopAsync();
    }
    async Task SaveItemAndGoBack() {
        UnitOfWork.Save(Item);
        await UnitOfWork.CommitChangesAsync();
        await Shell.Current.Navigation.PopAsync();
    }
    ```
    In the `ItemDetailPage.xaml`, add Toolbar items with the following parameters:
    ```xaml
    <ContentPage.ToolbarItems>
        <ToolbarItem Text="Delete" Command="{Binding CommandDelete}" />
        <ToolbarItem Text="Save" Command="{Binding CommandUpdate}"  />
    </ContentPage.ToolbarItems>
    ```
    Finally add a constructor to the `ItemsDetailPage.xaml.cs` class to bind ViewModel and Page together.
    ```csharp
    public ItemDetailPage() {
        InitializeComponent();
        BindingContext = new ItemDetailViewModel();
    }
    ```

## Step 8. Populate the Data Base

To seed the data in the database, add the `UpdateDataBase` method and call this method from the `InitXpo` method: 

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

Add these files into the `Core` folder: [Employees.xml](XamarinFormsDemo/Core/Employees.xml) , [DBUpdater.cs](XamarinFormsDemo/Core/DBUpdater.cs).

## Step 9. Run and Test the App

 - Log in as 'User' with an empty password.
 - Notice that secured data is not displayed.
 - Press the Logout button and log in as 'Admin' to see all records.
