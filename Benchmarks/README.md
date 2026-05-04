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

We ran all benchmarks against .NET 10 and used AnyCPU release builds (include warm-up). The test machine had Windows 11 Enterprise x64, local Microsoft SQL Server Express (64-bit) v16.00.1000, 12th Gen Intel(R) Core(TM) i7-12650H 2.70 GHz/ 32GB RAM / SSD. 

Needless to say, lower numbers are better.

### Scenario #1. Load Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/getContacts_smallDataSet.svg" width="100%" /> 
  <img src="../Benchmarks/images/getContacts_largeDataSet.svg" width="100%"/>
</p>

|Item Count                    |EF Core 10 (No Security), ms  |EF Core 10 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.987                         |11.168                        |3.864                         |8.804                         |
|20                            |2.177                         |13.259                        |4.984                         |10.581                        |
|50                            |2.799                         |19.820                        |6.318                         |13.425                        |
|100                           |3.872                         |25.696                        |9.561                         |18.861                        |
|250                           |6.702                         |36.459                        |16.393                        |29.944                        |
|500                           |9.455                         |32.243                        |29.160                        |33.167                        |
|1000                          |17.360                        |48.206                        |43.203                        |67.402                        |
|2500                          |24.094                        |115.120                       |106.802                       |145.323                       |
|5000                          |25.119                        |257.263                       |193.268                       |334.612                       |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)


### Scenario #2. Load Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/getTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/getTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 10 (No Security), ms  |EF Core 10 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |2.235                         |5.827                         |5.649                         |11.474                        |
|20                            |2.289                         |6.147                         |7.413                         |13.202                        |
|50                            |3.417                         |7.299                         |11.440                        |19.475                        |
|100                           |4.130                         |7.487                         |19.561                        |24.910                        |
|250                           |6.362                         |12.757                        |38.676                        |45.342                        |
|500                           |19.150                        |21.657                        |66.485                        |79.111                        |
|1000                          |24.985                        |24.729                        |136.841                       |147.240                       |
|2500                          |35.462                        |28.083                        |276.095                       |332.119                       |
|5000                          |44.091                        |48.831                        |425.427                       |594.826                       |


**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)


### Scenario #3. Create a Contact and its associated data (20 Tasks, PhoneNumbers, Positions, Addresses)

<p float="left">
  <img src="../Benchmarks/images/insertContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 10 (No Security), ms  |EF Core 10 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |23.575                        |47.057                        |14.308                        |17.721                        |
|20                            |32.467                        |61.868                        |21.242                        |22.680                        |
|50                            |58.899                        |107.373                       |35.353                        |39.894                        |
|100                           |103.612                       |183.424                       |49.248                        |60.994                        |
|250                           |165.645                       |346.137                       |130.576                       |145.806                       |
|500                           |278.350                       |618.442                       |253.346                       |301.930                       |
|1000                          |542.275                       |1224.729                      |537.297                       |645.064                       |
|2500                          |1448.027                      |3280.296                      |1534.084                      |1754.664                      |
|5000                          |2733.152                      |6058.940                      |3739.679                      |4233.914                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L75-L77)


### Scenario #4. Create a Contact without associated data

<p float="left">
  <img src="../Benchmarks/images/insertEmptyContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertEmptyContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 10 (No Security), ms  |EF Core 10 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |11.108                        |18.290                        |7.984                         |12.739                        |
|20                            |16.621                        |24.762                        |12.697                        |18.546                        |
|50                            |38.115                        |51.555                        |22.810                        |28.032                        |
|100                           |50.199                        |66.440                        |39.462                        |39.099                        |
|250                           |92.255                        |114.611                       |77.258                        |81.122                        |
|500                           |159.466                       |192.880                       |165.402                       |166.777                       |
|1000                          |279.544                       |331.368                       |316.790                       |348.622                       |
|2500                          |623.028                       |748.624                       |868.668                       |920.186                       |
|5000                          |1271.142                      |1471.408                      |2106.966                      |2232.796                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertEmptyContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L71-L73)


### Scenario #5. Load, update, and save Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateContacts_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateContacts_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 10 (No Security), ms  |EF Core 10 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |3.582                         |24.104                        |7.312                         |9.683                         |
|20                            |6.057                         |45.777                        |10.314                        |21.526                        |
|50                            |11.845                        |71.744                        |20.068                        |27.778                        |
|100                           |19.716                        |83.673                        |32.596                        |37.019                        |
|250                           |25.022                        |193.882                       |43.139                        |77.463                        |
|500                           |21.005                        |383.251                       |86.961                        |151.263                       |
|1000                          |45.176                        |768.622                       |180.533                       |310.952                       |
|2500                          |102.469                       |1893.912                      |504.055                       |910.639                       |
|5000                          |212.344                       |3879.762                      |992.383                       |1858.902                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L79-L81)


### Scenario #6. Load, update, and save Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 10 (No Security), ms  |EF Core 10 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.843                         |23.221                        |7.087                         |12.903                        |
|20                            |1.963                         |32.330                        |11.101                        |19.886                        |
|50                            |2.650                         |48.860                        |21.155                        |28.918                        |
|100                           |3.531                         |76.952                        |32.189                        |39.747                        |
|250                           |6.339                         |108.833                       |54.439                        |88.967                        |
|500                           |14.087                        |213.258                       |90.270                        |166.920                       |
|1000                          |21.205                        |401.454                       |182.683                       |346.450                       |
|2500                          |19.112                        |1001.664                      |430.375                       |829.440                       |
|5000                          |28.351                        |1986.213                      |804.189                       |1626.970                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L83-L85) 
