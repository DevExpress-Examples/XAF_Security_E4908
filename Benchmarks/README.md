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

We ran all benchmarks against .NET 9 and used AnyCPU release builds (include warm-up). The test machine had Windows 10 Enterprise x64, local Microsoft SQL Server Express (64-bit) v15.00.4153, 12th Gen Intel(R) Core(TM) i7-12650H 2.70 GHz/ 32GB RAM / SSD. 

Needless to say, lower numbers are better.

### Scenario #1. Load Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/getContacts_smallDataSet.svg" width="100%" /> 
  <img src="../Benchmarks/images/getContacts_largeDataSet.svg" width="100%"/>
</p>

|Item Count                    |EF Core 9 (No Security), ms   |EF Core 9 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.837                         |15.052                        |3.717                         |8.335                         |
|20                            |1.997                         |20.104                        |4.711                         |11.256                        |
|50                            |3.091                         |30.086                        |6.637                         |14.399                        |
|100                           |5.052                         |42.187                        |9.782                         |26.546                        |
|250                           |7.491                         |42.790                        |17.494                        |39.216                        |
|500                           |10.408                        |65.683                        |27.593                        |70.526                        |
|1000                          |16.901                        |125.618                       |48.202                        |148.088                       |
|2500                          |14.707                        |325.489                       |113.783                       |320.442                       |
|5000                          |23.038                        |695.998                       |209.244                       |645.297                       |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)


### Scenario #2. Load Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/getTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/getTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 9 (No Security), ms   |EF Core 9 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |2.533                         |9.545                         |6.117                         |10.188                        |
|20                            |2.755                         |12.245                        |7.547                         |13.436                        |
|50                            |3.674                         |18.906                        |12.137                        |25.462                        |
|100                           |4.652                         |25.743                        |19.137                        |30.427                        |
|250                           |7.546                         |36.812                        |39.559                        |70.270                        |
|500                           |21.420                        |48.036                        |63.085                        |126.748                       |
|1000                          |22.509                        |82.185                        |132.546                       |236.037                       |
|2500                          |28.993                        |204.323                       |269.338                       |541.194                       |
|5000                          |43.741                        |389.491                       |428.257                       |948.787                       |


**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)


### Scenario #3. Create a Contact and its associated data (20 Tasks, PhoneNumbers, Positions, Addresses)

<p float="left">
  <img src="../Benchmarks/images/insertContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 9 (No Security), ms   |EF Core 9 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |19.989                        |37.555                        |8.664                         |11.938                        |
|20                            |30.989                        |53.821                        |13.501                        |15.996                        |
|50                            |55.940                        |112.579                       |28.574                        |33.252                        |
|100                           |95.653                        |203.495                       |52.041                        |64.179                        |
|250                           |165.519                       |402.895                       |132.045                       |154.410                       |
|500                           |263.350                       |768.721                       |271.285                       |324.041                       |
|1000                          |517.376                       |1503.747                      |558.737                       |664.346                       |
|2500                          |1344.010                      |3783.048                      |1575.286                      |1831.681                      |
|5000                          |2686.944                      |7506.177                      |3836.677                      |4343.852                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L75-L77)


### Scenario #4. Create a Contact without associated data

<p float="left">
  <img src="../Benchmarks/images/insertEmptyContact_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/insertEmptyContact_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 9 (No Security), ms   |EF Core 9 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |8.790                         |15.372                        |7.339                         |10.250                        |
|20                            |14.846                        |19.162                        |10.981                        |16.736                        |
|50                            |33.003                        |40.408                        |17.913                        |20.021                        |
|100                           |45.035                        |54.071                        |30.072                        |33.117                        |
|250                           |87.970                        |107.094                       |73.344                        |78.906                        |
|500                           |146.504                       |176.392                       |150.399                       |161.150                       |
|1000                          |255.095                       |314.105                       |304.185                       |335.930                       |
|2500                          |585.981                       |733.953                       |886.143                       |909.839                       |
|5000                          |1234.804                      |1478.232                      |2140.867                      |2250.100                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertEmptyContact](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L71-L73)


### Scenario #5. Load, update, and save Contacts for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateContacts_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateContacts_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 9 (No Security), ms   |EF Core 9 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |4.102                         |11.375                        |6.348                         |10.137                        |
|20                            |4.821                         |24.304                        |9.291                         |17.786                        |
|50                            |10.450                        |37.005                        |10.790                        |35.452                        |
|100                           |14.932                        |45.415                        |19.485                        |42.485                        |
|250                           |24.114                        |51.755                        |44.684                        |100.277                       |
|500                           |22.076                        |93.315                        |96.947                        |194.394                       |
|1000                          |40.197                        |181.329                       |195.456                       |408.230                       |
|2500                          |108.908                       |469.099                       |503.918                       |1140.784                      |
|5000                          |248.967                       |967.041                       |1052.163                      |2245.655                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateContacts](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L79-L81)


### Scenario #6. Load, update, and save Tasks for a specific User

<p float="left">
  <img src="../Benchmarks/images/updateTasks_smallDataSet.svg" width="100%"/>
  
  <img src="../Benchmarks/images/updateTasks_largeDataSet.svg" width="100%"/> 
</p>

|Item Count                    |EF Core 9 (No Security), ms   |EF Core 9 (Security), ms      |XPO (No Security), ms         |XPO (Security), ms            |
|------------------------------|------------------------------|------------------------------|------------------------------|------------------------------|
|10                            |1.741                         |8.787                         |6.897                         |17.909                        |
|20                            |1.889                         |11.607                        |10.521                        |24.124                        |
|50                            |2.438                         |18.788                        |17.041                        |33.162                        |
|100                           |3.465                         |23.183                        |26.724                        |57.055                        |
|250                           |6.229                         |36.089                        |43.748                        |131.653                       |
|500                           |15.754                        |47.822                        |87.420                        |257.497                       |
|1000                          |19.808                        |81.017                        |187.103                       |514.457                       |
|2500                          |22.583                        |197.415                       |440.048                       |1252.022                      |
|5000                          |27.707                        |385.794                       |840.026                       |2430.021                      |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateTasks](../Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L83-L85) 
