# .NET App Security API Benchmark for EF Core and XPO 

## About the Project

We built this project to test the performance of Object-Relational Mapping (ORM) libraries used with [XAF's Security System](https://www.devexpress.com/products/net/application_framework/security.xml). We tested two libraries with [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet):

 - [Microsoft Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/) (EF Core)
 - [DevExpress eXpress Persistent Objects™](https://www.devexpress.com/Products/NET/ORM/) (XPO)
 
You can run the benchmarks on your computer or review our test results below.

## Run Benchmarks in Your Environment

You can download the project and run the benchmarks on a system different from the one listed in our Test Results section. You can also modify the data model and test cases: measure memory consumption, include scenarios with BLOBs, add reference or collection properties, and so on. 

Once you download the project, follow the steps below to run benchmark tests in your environment:

1. Download and run the [DevExpress Unified Component Installer](https://www.devexpress.com/Products/Try/) to add [DevExpress.Xpo](https://www.nuget.org/packages/DevExpress.Xpo/) and other libraries to project references.
2. Edit the connection string in [App.config](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/App.config).
3. Update the ORM library and target framework versions, if necessary.

## Get Support
Contact us if you cannot compile or run any of these demo apps or have questions about our tutorials or supported functionality. Submit questions to the DevExpress [Support Center](https://www.devexpress.com/ask) or switch to [the Issues tab above](https://github.com/DevExpress-Examples/XAF_how-to-use-the-integrated-mode-of-the-security-system-in-non-xaf-applications-e4908/issues). We are here to help.

## Benchmark Configuration
### Data Model Structure
This project uses the following business objects:

<p float="left">
  <img src="../Benchmarks/images/ClassDiagram.png"  /> 
</p>

### Users and Permissions

Our project creates and loads data objects according to the following data access rules:

1. A user can access a Contact if their Departments match.
2. A user can access a Task in two cases: a user from the same department is specified in the AssignedTo field, or such a user exists in the task's Contacts collection.

We use the following code to implement these rules.

#### In Apps without the Security System

```csharp
    Expression<Func<Contact, bool>> ContactsFilterPredicate(ICustomPermissionPolicyUser currentUser) =>
        contact => contact.Department == currentUser.Department;

    Expression<Func<DemoTask, bool>> TasksFilterPredicate(ICustomPermissionPolicyUser currentUser) =>
        task => task.Contacts.Any(contact => contact.Department.Users.Any(user => user == currentUser)) ||
        ((Contact)task.AssignedTo).Department == currentUser.Department;
```

We use these filter predicates to load objects in security-free XPO and EF Core tests. This way we obtain the numbers that we compare to tests with integrated Security System. 

#### In Apps with the Security System

```csharp
    userRole.AddTypePermission<ContactType>(SecurityOperations.FullObjectAccess, SecurityPermissionState.Deny);
    userRole.AddObjectPermission<ContactType>(SecurityOperations.FullObjectAccess,
      $"[Department].[Users][[{keyPropertyName}] == CurrentUserId()].Exists()", SecurityPermissionState.Allow);

    userRole.AddTypePermission<TaskType>(SecurityOperations.FullObjectAccess, SecurityPermissionState.Deny);
    userRole.AddObjectPermission<TaskType>(SecurityOperations.FullObjectAccess,
      $"[Contacts][[Department].[Users][[{keyPropertyName}] == CurrentUserId()].Exists()]", SecurityPermissionState.Allow);

    if(typeof(TaskType).IsSubclassOf(typeof(DevExpress.Persistent.BaseImpl.Task))
        || typeof(TaskType).IsSubclassOf(typeof(XAFSecurityBenchmark.Models.EFCore.Task))) {
        userRole.AddObjectPermission<TaskType>(SecurityOperations.FullObjectAccess,
          $"[AssignedTo].<Contact>[Department].[Users][[{keyPropertyName}] == CurrentUserId()].Exists()", SecurityPermissionState.Allow);
    }
    else {
        userRole.AddObjectPermission<TaskType>(SecurityOperations.FullObjectAccess,
          "Upcast(AssignedTo, 'XAFSecurityBenchmark.Models.EFCore.Contact', 'Department') == CurrentUserDepartment()", SecurityPermissionState.Allow);
    }
```
 **Source:** [DBUpdaterBase.CreateSecurityObjects](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/DBUpdater/DBUpdaterBase.cs#L114-131)

### Initial Data

1) Tests that create new objects start with an empty database. The code cleans the database after every test iteration cycle.
2) Tests that load collections and modify data use the following prepared dataset: 
     - The database updater creates five test users specified by the [TestSetConfig.Users](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/TestSetConfig.cs#L20) array.
     - For every User, we generate 5,000 Contacts (a grand total of 25,000 Contacts in the database). The tests read varying number of contacts on each test iteration (see graphs below). The [TestSetConfig.ContactCountPerUserToCreate](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/TestSetConfig.cs#L21) array specifies the numbers for each test run.
     - For every Contact, we generate Tasks. The [TestSetConfig.TasksAssignedToContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/TestSetConfig.cs#L22) and [TestSetConfig.TasksLinkedToContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/TestSetConfig.cs#L23) specify the number of Tasks assigned to and linked to the Contact, respectively. The database holds a grand total of 500,000 Tasks.
    - For every Contact, we initialize its associated data such as PhoneNumber, Position and Address.

For more information, see the test object creation logic in the [TemporaryTestObjectsHelper](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/DBUpdater/TempDataCreationHelpers/TemporaryTestObjectsHelper.cs) class.


## Test Results

We ran all benchmarks against .NET 8 and used AnyCPU release builds (include warm-up). The test machine had Windows 10 Enterprise x64, local Microsoft SQL Server Express (64-bit) v15.00.4153, 12th Gen Intel(R) Core(TM) i7-12650H 2.70 GHz/ 32GB RAM / SSD. 

Needless to say, lower numbers are better.

### Scenario #1. Load Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/getContacts_smallDataSet.svg" width="100%" /> 
  <img src="../Benchmarks/images/getContacts_largeDataSet.svg" width="100%"/>
</p>

|Item Count                    |EF Core 8 (No Security), ms   |EF Core 8 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |2.234                         |14.987                        |3.815                         |11.482                        |
|20                            |2.669                         |19.481                        |4.961                         |15.172                        |
|50                            |3.436                         |30.458                        |6.277                         |21.930                        |
|100                           |4.334                         |41.139                        |9.064                         |30.025                        |
|250                           |7.230                         |43.110                        |16.968                        |48.657                        |
|500                           |11.804                        |79.753                        |27.039                        |93.529                        |
|1000                          |11.454                        |150.577                       |44.442                        |176.549                       |
|2500                          |17.225                        |400.539                       |98.362                        |425.034                       |
|5000                          |23.415                        |866.079                       |179.518                       |900.544                       |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)


### Scenario #2. Load Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/getTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/getTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 8 (No Security), ms   |EF Core 8 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |2.219                         |24.734                        |5.326                         |15.062                        |
|20                            |2.512                         |37.465                        |6.705                         |18.199                        |
|50                            |3.233                         |44.322                        |10.767                        |31.091                        |
|100                           |3.998                         |73.071                        |15.470                        |36.861                        |
|250                           |6.542                         |170.721                       |31.738                        |86.903                        |
|500                           |19.737                        |344.163                       |57.534                        |165.848                       |
|1000                          |24.767                        |703.591                       |121.547                       |329.389                       |
|2500                          |30.323                        |1763.419                      |229.774                       |767.909                       |
|5000                          |42.078                        |3408.558                      |407.824                       |1398.508                      |


**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)


### Scenario #3. Create a Contact and its associated data (20 Tasks, PhoneNumbers, Positions, Addresses)

<p float="left">
  <img src="../Benchmarks/images/insertContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 8 (No Security), ms   |EF Core 8 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |20.363                        |41.920                        |11.665                        |16.994                        |
|20                            |28.305                        |49.539                        |18.395                        |25.352                        |
|50                            |47.684                        |96.471                        |29.643                        |34.855                        |
|100                           |85.840                        |171.111                       |47.299                        |61.265                        |
|250                           |134.601                       |328.722                       |126.804                       |150.116                       |
|500                           |211.332                       |599.735                       |247.521                       |303.876                       |
|1000                          |439.329                       |1154.576                      |522.997                       |642.392                       |
|2500                          |1239.950                      |2972.454                      |1497.780                      |1789.877                      |
|5000                          |2372.419                      |6018.860                      |3640.603                      |4197.200                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L75-L77)


### Scenario #4. Create a Contact without associated data

<p float="left">
  <img src="../Benchmarks/images/insertEmptyContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertEmptyContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 8 (No Security), ms   |EF Core 8 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |8.065                         |11.941                        |6.844                         |11.034                        |
|20                            |11.920                        |16.012                        |7.556                         |15.244                        |
|50                            |27.784                        |33.215                        |15.643                        |19.669                        |
|100                           |35.825                        |43.363                        |29.144                        |32.256                        |
|250                           |62.116                        |82.338                        |70.542                        |78.818                        |
|500                           |98.494                        |133.814                       |142.557                       |153.469                       |
|1000                          |167.101                       |227.586                       |281.699                       |323.668                       |
|2500                          |393.581                       |555.752                       |828.234                       |909.25                        |
|5000                          |848.626                       |1126.611                      |2039.024                      |2162.288                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertEmptyContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L71-L73)


### Scenario #5. Load, update, and save Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateContacts_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateContacts_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 8 (No Security), ms   |EF Core 8 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |3.815                         |11.954                        |6.093                         |13.960                        |
|20                            |4.954                         |25.883                        |10.022                        |25.763                        |
|50                            |9.717                         |37.544                        |19.325                        |39.230                        |
|100                           |15.973                        |35.949                        |31.917                        |49.477                        |
|250                           |26.287                        |61.668                        |39.766                        |113.974                       |
|500                           |25.778                        |109.723                       |80.860                        |224.699                       |
|1000                          |43.771                        |219.667                       |164.908                       |470.353                       |
|2500                          |119.416                       |594.867                       |456.014                       |1299.245                      |
|5000                          |286.360                       |1267.876                      |952.631                       |2597.163                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L79-L81)


### Scenario #6. Load, update, and save Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 8 (No Security), ms   |EF Core 8 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.843                         |23.809                        |6.912                         |22.064                        |
|20                            |2.151                         |40.160                        |9.978                         |29.349                        |
|50                            |2.627                         |60.777                        |18.728                        |37.576                        |
|100                           |3.628                         |78.147                        |30.050                        |69.711                        |
|250                           |6.515                         |174.942                       |42.031                        |167.772                       |
|500                           |15.632                        |331.136                       |82.236                        |325.734                       |
|1000                          |17.035                        |639.519                       |169.156                       |668.151                       |
|2500                          |27.015                        |1642.954                      |402.374                       |1584.791                      |
|5000                          |30.678                        |3157.133                      |765.834                       |3174.236                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L83-L85) 
