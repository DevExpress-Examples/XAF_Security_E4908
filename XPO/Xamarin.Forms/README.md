This example demonstrates how to access data protected by the [Security System](https://docs.devexpress.com/eXpressAppFramework/113366/concepts/security-system/security-system-overview) from a non-XAF Xamarin Forms application. 
You will also learn how to execute Create, Write and Delete data operations taking into account security permissions.

>For simplicity, the instructions include only C# code snippets. For the complete C# code, see the [CS](CS) sub-directorie.


### Prerequisites
- Build the solution and run the *XafSolution.Win* project to log in under 'User' or 'Admin' with an empty password. The application will generate a database with business objects from the *XafSolution.Module* project. 
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

## Step 2. Login Page Implementation


  
## Step 3. Default Page Implementation
  

  
  ## Step 4: Run and Test the App
 - Log in under 'User' with an empty password.
   
  

 - Notice that secured data is displayed as '*******'.


 - Press the Logout button and log in under 'Admin' to see all records.
