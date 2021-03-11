# .NET XAF Security Benchmark
This project is a [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)-based benchmark. We used it to test the performance of the following Object-Relational Mapping (ORM) libraries for .NET Core 5.0:<br/>
 - [Entity Framework Core 5.0](https://docs.microsoft.com/en-us/ef/core/) (EF Core);<br/>
 - [eXpress Persistent Objects™ 20.2.5](https://www.devexpress.com/Products/NET/ORM/) (XPO).<br/>
 
You can run these benchmarks or review our test results below. Needless to say, the lower the execution time the better.

All benchmarks were executed using .NET Core 5.0, AnyCPU release builds (include warm-up), Windows 10 Enterprise x64, local Microsoft SQL Server Express (64-bit) v13.0.4001.0, i7-6700 CPU @4.0GHz / 32GB RAM / SSD. 

If you download the project to run benchmark tests in your environment, edit the connection string in the [App.config](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/App.config) file and update the ORM library and target framework versions, if necessary. Please note that we used localy installed Universal build to add [DevExpress.Xpo](https://www.nuget.org/packages/DevExpress.Xpo/) and other libraries to project references.  

Feel free to make data model and test case modifications to cover additional usage scenarios. For instance,  measure memory consumption, include scenarios with BLOBs, reference and collection properties, etc. We'd love to hear your feedback about this project. Drop us a line in this [blog post](blog link here), thanks.

**See Also:**<br/>
[XPO ORM Library – Available Free-of-Charge in v18.1!](https://community.devexpress.com/blogs/xpo/archive/2018/05/21/xpo-free-of-charge-in-v18-1.aspx) (blog)<br/>
[How to: Connect to a Data Store](https://documentation.devexpress.com/CoreLibraries/2123/DevExpress-ORM-Tool/Concepts/How-to-Connect-to-a-Data-Store) (online documentation)<br/>
[Getting Started with \.NET Core](https://documentation.devexpress.com/CoreLibraries/119377/DevExpress-ORM-Tool/Getting-Started/Getting-Started-with-NET-Core) (online documentation)


## Load Contacts


<p float="left">
  <img src="https://raw.githubusercontent.com/DevExpress-Examples/XAF_Security_E4908/fc2a7bf077a96e63bfd10d29efac8591e2634adc/Benchmarks/images/getContacts_smallDataSet.svg" />
  <img src="https://raw.githubusercontent.com/DevExpress-Examples/XAF_Security_E4908/fc2a7bf077a96e63bfd10d29efac8591e2634adc/Benchmarks/images/getContacts_largeDataSet.svg" /> 
</p>

|Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|----------------------------|-------------------------|----------------------|-------------------|
|10          |1.370                       |12.734                   |2.781                 |7.149              |
|20          |1.415                       |17.683                   |3.760                 |7.304              |
|50          |1.708                       |26.966                   |5.051                 |12.335             |
|100         |2.017                       |44.743                   |7.246                 |20.567             |
|250         |3.065                       |101.844                  |15.311                |48.055             |
|500         |4.741                       |196.163                  |26.649                |94.178             |
|1000        |8.098                       |407.876                  |53.689                |188.849            |
|2500        |18.643                      |1,139.486                |154.995               |474.939            |
|5000        |19.457                      |2,847.249                |268.775               |967.130            |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)


## Load Tasks

<p float="left">
  <img src="https://raw.githubusercontent.com/DevExpress-Examples/XAF_Security_E4908/fc2a7bf077a96e63bfd10d29efac8591e2634adc/Benchmarks/images/getTasks_smallDataSet.svg" />
  <img src="https://raw.githubusercontent.com/DevExpress-Examples/XAF_Security_E4908/fc2a7bf077a96e63bfd10d29efac8591e2634adc/Benchmarks/images/getTasks_largeDataSet.svg" /> 
</p>

|Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|--------------------------|---------------------------|----------------------|-----------------------|
|10          |1.581                     |26.763                     |4.184                 |38.900                 |
|20          |1.865                     |37.863                     |6.792                 |88.789                 |
|50          |2.449                     |75.082                     |12.080                |202.839                |
|100         |3.284                     |134.820                    |21.896                |416.492                |
|250         |19.132                    |325.692                    |44.621                |1,069.608              |
|500         |22.137                    |634.178                    |79.334                |2,084.031              |
|1000        |28.301                    |1,253.319                  |153.552               |4,192.824              |
|2500        |47.406                    |3,105.556                  |304.600               |10,222.239             |
|5000        |78.365                    |6,198.760                  |558.145               |20,674.707             | 

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)


## Add new Contacts with 20 Tasks for each and other content

<!-- |                    Small Data Set                    |                    Large Data Set                    |
| ---------------------------------------------------- | ---------------------------------------------------- |
| ![](/Benchmarks/images/getTasks_smallDataSet.png) | ![](/Benchmarks/images/getTasks_largeDataSet.png) | -->

|Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|----------------------------|-------------------------|----------------------|-----------------------|
|10          |37.689                      |49.538                   |9.610                 |17.945                 |
|20          |48.919                      |63.285                   |16.315                |30.597                 |
|50          |111.202                     |130.787                  |40.767                |62.213                 |
|100         |191.713                     |219.141                  |76.946                |115.297                |
|250         |323.199                     |377.414                  |188.958               |283.511                |
|500         |519.324                     |612.042                  |375.124               |558.222                |
|1000        |934.478                     |1,101.866                |762.798               |1,143.385              |
|2500        |2,251.004                   |2,706.443                |2,015.533             |3,217.575              |
|5000        |4,434.771                   |5,261.035                |4,336.850             |6,529.839              |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertContact](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L75-L77)


## Add new Contacts without ref objects

<!-- |                    Small Data Set                    |                    Large Data Set                    |
| ---------------------------------------------------- | ---------------------------------------------------- |
| ![](/Benchmarks/images/getTasks_smallDataSet.png) | ![](/Benchmarks/images/getTasks_largeDataSet.png) | -->

|Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|----------------------------|-------------------------|----------------------|-----------------------|
|10          |16.224                      |21.824                   |6.256                 |8.557                  |
|20          |21.386                      |28.582                   |10.476                |12.832                 |
|50          |49.791                      |59.096                   |23.785                |28.845                 |
|100         |67.251                      |82.284                   |44.266                |50.461                 |
|250         |122.076                     |148.043                  |104.173               |123.448                |
|500         |191.099                     |235.686                  |207.360               |242.628                |
|1000        |333.375                     |413.528                  |410.068               |475.639                |
|2500        |776.824                     |949.982                  |1,027.423             |1,216.567              |
|5000        |1,514.629                   |1,864.973                |2,102.060             |2,503.027              |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.InsertEmptyContact](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L71-L73)


## Load Contacts, update and save

<!-- |                    Small Data Set                    |                    Large Data Set                    |
| ---------------------------------------------------- | ---------------------------------------------------- |
| ![](/Benchmarks/images/getTasks_smallDataSet.png) | ![](/Benchmarks/images/getTasks_largeDataSet.png) | -->

|Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|----------------------------|-------------------------|----------------------|-----------------------|
|10          |2.810                       |15.952                   |5.582                 |10.709                 |
|20          |3.857                       |23.704                   |8.366                 |16.588                 |
|50          |6.304                       |35.559                   |15.322                |35.404                 |
|100         |10.674                      |62.368                   |27.150                |60.569                 |
|250         |22.372                      |139.360                  |64.693                |140.151                |
|500         |41.286                      |273.831                  |131.991               |285.815                |
|1000        |83.497                      |559.966                  |246.728               |601.347                |
|2500        |211.406                     |1,539.082                |647.841               |1,603.247              |
|5000        |465.541                     |3,636.916                |1,267.308             |3,158.913              |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateContacts](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L79-L81)


## Load Tasks, update and save

<!-- |                    Small Data Set                    |                    Large Data Set                    |
| ---------------------------------------------------- | ---------------------------------------------------- |
| ![](/Benchmarks/images/getTasks_smallDataSet.png) | ![](/Benchmarks/images/getTasks_largeDataSet.png) | -->

|Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|----------------------------|-------------------------|----------------------|-----------------------|
|10          |1.475                       |22.537                   |5.841                 |90.147                 |
|20          |1.881                       |37.966                   |8.603                 |169.677                |
|50          |2.813                       |79.589                   |14.698                |407.658                |
|100         |3.662                       |136.937                  |26.857                |794.231                |
|250         |12.763                      |325.457                  |66.651                |1,966.607              |
|500         |18.088                      |647.989                  |113.578               |3,939.288              |
|1000        |25.225                      |1,281.299                |236.457               |7,834.771              |
|2500        |49.258                      |3,161.325                |542.876               |19,379.141             |
|5000        |90.776                      |6,267.947                |1,043.535             |38,638.566             |

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.UpdateTasks](/Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L83-L85)