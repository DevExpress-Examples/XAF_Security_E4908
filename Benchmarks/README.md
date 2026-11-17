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

We ran all benchmarks against .NET 11 and used AnyCPU release builds (include warm-up). The test machine had Windows 11 Enterprise x64, local Microsoft SQL Server Express (64-bit) v16.00.1000, 12th Gen Intel(R) Core(TM) i7-12650H 2.70 GHz/ 32GB RAM / SSD. 

Needless to say, lower numbers are better.

### Scenario #1. Load Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/getContacts_smallDataSet.svg" width="100%" /> 
  <img src="../Benchmarks/images/getContacts_largeDataSet.svg" width="100%"/>
</p>

|Item Count                    |EF Core 11 (No Security), ms  |EF Core 11 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.211                         |3.801                         |1.945                         |2.403                         |
|20                            |1.232                         |3.952                         |2.672                         |2.974                         |
|50                            |1.360                         |5.370                         |3.502                         |4.733                         |
|100                           |1.540                         |6.506                         |5.114                         |7.412                         |
|250                           |2.282                         |10.529                        |10.663                        |16.118                        |
|500                           |2.809                         |21.002                        |19.213                        |30.818                        |
|1000                          |4.570                         |39.921                        |38.798                        |68.046                        |
|2500                          |12.007                        |101.953                       |104.361                       |153.436                       |
|5000                          |19.152                        |217.124                       |180.384                       |311.186                       |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)


### Scenario #2. Load Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/getTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/getTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 11 (No Security), ms  |EF Core 11 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.356                         |2.588                         |3.482                         |4.146                         |
|20                            |1.444                         |2.630                         |5.065                         |5.579                         |
|50                            |1.953                         |3.006                         |9.023                         |9.517                         |
|100                           |2.193                         |3.283                         |15.702                        |15.631                        |
|250                           |3.550                         |4.093                         |32.280                        |35.985                        |
|500                           |15.815                        |10.227                        |64.274                        |69.728                        |
|1000                          |19.425                        |11.840                        |134.753                       |141.200                       |
|2500                          |27.084                        |25.215                        |267.320                       |318.631                       |
|5000                          |42.054                        |45.131                        |421.432                       |604.531                       |


**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)


### Scenario #3. Create a Contact and its associated data (20 Tasks, PhoneNumbers, Positions, Addresses)

<p float="left">
  <img src="../Benchmarks/images/insertContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 11 (No Security), ms  |EF Core 11 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |18.560                        |32.381                        |7.273                         |8.555                         |
|20                            |27.197                        |46.403                        |11.888                        |13.463                        |
|50                            |55.843                        |98.741                        |25.545                        |28.247                        |
|100                           |104.067                       |174.895                       |47.828                        |56.796                        |
|250                           |166.716                       |337.326                       |132.673                       |146.925                       |
|500                           |280.165                       |622.725                       |252.613                       |311.686                       |
|1000                          |555.626                       |1224.742                      |527.097                       |614.071                       |
|2500                          |1451.398                      |3104.046                      |1356.100                      |1584.405                      |
|5000                          |2907.226                      |6065.265                      |2840.210                      |3351.829                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L75-L77)


### Scenario #4. Create a Contact without associated data

<p float="left">
  <img src="../Benchmarks/images/insertEmptyContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertEmptyContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 11 (No Security), ms  |EF Core 11 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |8.887                         |12.641                        |4.609                         |5.452                         |
|20                            |13.820                        |15.957                        |7.443                         |8.456                         |
|50                            |33.791                        |36.531                        |15.594                        |16.429                        |
|100                           |46.405                        |51.228                        |28.378                        |30.729                        |
|250                           |94.427                        |102.431                       |68.602                        |73.450                        |
|500                           |149.829                       |169.860                       |140.134                       |146.609                       |
|1000                          |267.454                       |309.763                       |278.562                       |316.509                       |
|2500                          |630.248                       |728.017                       |796.449                       |826.651                       |
|5000                          |1298.194                      |1481.385                      |1546.194                      |1670.594                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertEmptyContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L71-L73)


### Scenario #5. Load, update, and save Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateContacts_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateContacts_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 11 (No Security), ms  |EF Core 11 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |2.038                         |13.333                        |3.530                         |5.306                         |
|20                            |2.336                         |19.942                        |5.418                         |7.825                         |
|50                            |4.077                         |48.002                        |9.819                         |14.988                        |
|100                           |5.986                         |92.995                        |17.840                        |30.677                        |
|250                           |12.217                        |216.833                       |40.857                        |70.026                        |
|500                           |24.898                        |421.559                       |85.878                        |146.287                       |
|1000                          |50.200                        |832.700                       |167.685                       |317.403                       |
|2500                          |127.420                       |2064.256                      |464.163                       |889.161                       |
|5000                          |271.490                       |4194.832                      |913.089                       |1760.489                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L79-L81)


### Scenario #6. Load, update, and save Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 11 (No Security), ms  |EF Core 11 (Security), ms     |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.087                         |9.041                         |4.452                         |6.560                         |
|20                            |1.292                         |13.389                        |5.959                         |9.804                         |
|50                            |1.453                         |32.286                        |11.158                        |19.139                        |
|100                           |1.694                         |52.484                        |19.397                        |33.958                        |
|250                           |2.298                         |123.902                       |43.565                        |85.107                        |
|500                           |8.837                         |243.556                       |89.719                        |173.521                       |
|1000                          |10.004                        |485.871                       |182.316                       |343.614                       |
|2500                          |14.162                        |1179.939                      |420.314                       |828.732                       |
|5000                          |27.071                        |2413.979                      |797.565                       |1570.289                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L83-L85) 
