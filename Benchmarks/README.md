# .NET XAF Security Benchmark
This project is a [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet)-based benchmark. We used it to test the performance of the following Object-Relational Mapping (ORM) libraries for .NET Core 5.0:<br/>
 - [Entity Framework Core 5.0](https://docs.microsoft.com/en-us/ef/core/) (EF Core);<br/>
 - [eXpress Persistent Objects™ 20.2.5](https://www.devexpress.com/Products/NET/ORM/) (XPO).<br/>
 
You can run these benchmarks or review our test results below. Needless to say, the lower the execution time the better.

All benchmarks were executed using .NET Core 5.0, AnyCPU release builds (include warm-up), Windows 10 Enterprise x64, local Microsoft SQL Server Express (64-bit) v13.0.4001.0, i7-6700 CPU @4.0GHz / 32GB RAM / SSD. 

If you download the project to run benchmark tests in your environment, edit the connection string in the [App.config](/Benchmarks/XAFSecurityBenchmark/App.config) file and update the ORM library and target framework versions, if necessary. Please note that we used localy installed Universal build to add [DevExpress.Xpo](https://www.nuget.org/packages/DevExpress.Xpo/) and other libraries to project references.  

Feel free to make data model and test case modifications to cover additional usage scenarios. For instance,  measure memory consumption, include scenarios with BLOBs, reference and collection properties, etc. We'd love to hear your feedback about this project. Drop us a line in this [blog post](blog link here), thanks.

**See Also:**<br/>
[XPO ORM Library – Available Free-of-Charge in v18.1!](https://community.devexpress.com/blogs/xpo/archive/2018/05/21/xpo-free-of-charge-in-v18-1.aspx) (blog)<br/>
[How to: Connect to a Data Store](https://documentation.devexpress.com/CoreLibraries/2123/DevExpress-ORM-Tool/Concepts/How-to-Connect-to-a-Data-Store) (online documentation)<br/>
[Getting Started with \.NET Core](https://documentation.devexpress.com/CoreLibraries/119377/DevExpress-ORM-Tool/Getting-Started/Getting-Started-with-NET-Core) (online documentation)


## Load Contacts

|                    Small Data Set                    |                    Large Data Set                    |
| ---------------------------------------------------- | ---------------------------------------------------- |
| ![](/Benchmarks/images/getContacts_smallDataSet.png) | ![](/Benchmarks/images/getContacts_largeDataSet.png) |

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

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetContacts](Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L87-L89)

## Load Tasks

|                    Small Data Set                    |                    Large Data Set                    |
| ---------------------------------------------------- | ---------------------------------------------------- |
| ![](/Benchmarks/images/getTasks_smallDataSet.png) | ![](/Benchmarks/images/getTasks_largeDataSet.png) |

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

**Source:** [XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet.GetTasks](Benchmarks/XAFSecurityBenchmark/XAFSecurityBenchmark/PerformanceTests/Base/PerformanceTestSet.cs#L91-L93)

<!-- |Items Count | EF Core 5 (No Security) ms | EF Core 5 (Security) ms | XPO (No Security) ms | XPO (Security) ms |
|------------|--------------------------|-----------------------|-----------------------|-----------------------|
|10          |                          |                       |                       |                       |
|20          |                          |                       |                       |                       |
|50          |                          |                       |                       |                       |
|100         |                          |                       |                       |                       |
|250         |                          |                       |                       |                       |
|500         |                          |                       |                       |                       |
|1000        |                          |                       |                       |                       |
|2500        |                          |                       |                       |                       |
|5000        |                          |                       |                       |                       | -->