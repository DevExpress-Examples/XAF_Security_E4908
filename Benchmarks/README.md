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

We ran all benchmarks against .NET 6 and used AnyCPU release builds (include warm-up). The test machine had Windows 10 Enterprise x64, local Microsoft SQL Server Express (64-bit) v15.00.4153, i7-8565U CPU 1.80GHz / 16GB RAM / SSD. 

Needless to say, lower numbers are better.

### Scenario #1. Load Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/getContacts_smallDataSet.svg" width="100%" /> 
  <img src="../Benchmarks/images/getContacts_largeDataSet.svg" width="100%"/>
</p>

|Item Count                    |EF Core 6 (No Security), ms   |EF Core 6 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.904                         |13.237                        |3.475                         |10.158                        |
|20                            |2.249                         |17.960                        |4.524                         |15.155                        |
|50                            |2.754                         |30.723                        |6.009                         |28.735                        |
|100                           |3.523                         |51.336                        |9.222                         |46.193                        |
|250                           |5.505                         |108.844                       |18.272                        |105.792                       |
|500                           |8.761                         |204.412                       |31.274                        |206.597                       |
|1000                          |13.526                        |411.823                       |63.029                        |403.873                       |
|2500                          |28.082                        |1027.268                      |169.442                       |999.144                       |
|5000                          |50.257                        |2024.886                      |290.555                       |1959.855                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)


### Scenario #2. Load Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/getTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/getTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 6 (No Security), ms   |EF Core 6 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |2.222                         |23.294                        |5.123                         |14.047                        |
|20                            |2.535                         |36.954                        |6.564                         |22.771                        |
|50                            |3.318                         |77.508                        |12.511                        |44.269                        |
|100                           |4.160                         |140.400                       |20.393                        |74.721                        |
|250                           |6.812                         |342.428                       |48.244                        |178.487                       |
|500                           |25.695                        |648.484                       |88.491                        |341.301                       |
|1000                          |30.496                        |1293.597                      |199.272                       |642.491                       |
|2500                          |45.456                        |3179.451                      |403.430                       |1529.463                      |
|5000                          |69.999                        |6415.489                      |659.386                       |2891.915                      |


**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)


### Scenario #3. Create a Contact and its associated data (20 Tasks, PhoneNumbers, Positions, Addresses)

<p float="left">
  <img src="../Benchmarks/images/insertContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 6 (No Security), ms   |EF Core 6 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |21.740                        |54.970                        |11.757                        |18.214                        |
|20                            |33.928                        |75.622                        |19.265                        |39.455                        |
|50                            |83.777                        |158.878                       |43.589                        |57.055                        |
|100                           |152.922                       |286.023                       |81.416                        |113.413                       |
|250                           |277.036                       |575.107                       |204.544                       |282.959                       |
|500                           |450.097                       |1031.882                      |395.238                       |545.208                       |
|1000                          |857.256                       |2004.542                      |802.991                       |1093.585                      |
|2500                          |2196.741                      |5030.960                      |2059.204                      |2790.443                      |
|5000                          |4192.897                      |9783.412                      |4353.825                      |5978.855                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L75-L77)


### Scenario #4. Create a Contact without associated data

<p float="left">
  <img src="../Benchmarks/images/insertEmptyContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertEmptyContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 6 (No Security), ms   |EF Core 6 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |12.351                        |18.745                        |7.931                         |11.182                        |
|20                            |17.377                        |26.514                        |12.484                        |15.663                        |
|50                            |42.629                        |69.339                        |25.344                        |31.612                        |
|100                           |56.552                        |82.926                        |48.453                        |67.295                        |
|250                           |107.527                       |163.434                       |116.923                       |133.885                       |
|500                           |171.849                       |268.958                       |221.225                       |262.004                       |
|1000                          |304.661                       |482.842                       |438.013                       |515.553                       |
|2500                          |711.427                       |1153.885                      |1110.996                      |1326.815                      |
|5000                          |1492.304                      |2360.035                      |2306.539                      |2792.178                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertEmptyContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L71-L73)


### Scenario #5. Load, update, and save Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateContacts_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateContacts_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 6 (No Security), ms   |EF Core 6 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |3.152                         |17.421                        |6.970                         |19.567                        |
|20                            |4.102                         |24.231                        |10.258                        |32.139                        |
|50                            |6.410                         |40.011                        |18.313                        |59.657                        |
|100                           |9.657                         |70.590                        |32.809                        |110.091                       |
|250                           |19.926                        |150.610                       |73.680                        |254.369                       |
|500                           |38.294                        |283.889                       |152.715                       |518.112                       |
|1000                          |74.632                        |561.530                       |282.567                       |1031.548                      |
|2500                          |206.279                       |1385.225                      |737.442                       |2737.980                      |
|5000                          |470.039                       |2925.012                      |1433.632                      |5225.906                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L79-L81)


### Scenario #6. Load, update, and save Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 6 (No Security), ms   |EF Core 6 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.780                         |25.029                        |7.230                         |23.505                        |
|20                            |2.081                         |41.265                        |10.562                        |37.975                        |
|50                            |2.844                         |87.256                        |20.130                        |77.818                        |
|100                           |3.513                         |159.189                       |33.420                        |138.679                       |
|250                           |6.205                         |392.286                       |72.776                        |337.622                       |
|500                           |15.883                        |788.095                       |134.111                       |651.398                       |
|1000                          |22.818                        |1528.384                      |284.931                       |1345.228                      |
|2500                          |41.588                        |3834.232                      |650.563                       |3169.336                      |
|5000                          |75.629                        |7398.532                      |1203.302                      |6218.685                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L83-L85) 
