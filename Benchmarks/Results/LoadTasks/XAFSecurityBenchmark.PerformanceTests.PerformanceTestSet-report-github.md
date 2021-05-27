``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1379 (1909/November2018Update/19H2)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.103
  [Host]     : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  Job-HRHPXY : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Runtime=.NET Core 5.0  InvocationCount=1  RunStrategy=Throughput  
UnrollFactor=1  

```
|         Method | ItemsForTestIteration |         TestProvider |          Mean |       Error |      StdDev |        Median |
|--------------- |---------------------- |--------------------- |--------------:|------------:|------------:|--------------:|
| UpdateContacts |                    10 |         EFCore 5.0.0 |      1.475 ms |   0.0295 ms |   0.0561 ms |      1.453 ms |
| UpdateContacts |                    20 |         EFCore 5.0.0 |      1.881 ms |   0.0375 ms |   0.0447 ms |      1.886 ms |
| UpdateContacts |                    50 |         EFCore 5.0.0 |      2.813 ms |   0.0560 ms |   0.0550 ms |      2.800 ms |
| UpdateContacts |                   100 |         EFCore 5.0.0 |      3.662 ms |   0.1047 ms |   0.3087 ms |      3.551 ms |
| UpdateContacts |                   250 |         EFCore 5.0.0 |     12.763 ms |   0.3059 ms |   0.8973 ms |     12.241 ms |
| UpdateContacts |                   500 |         EFCore 5.0.0 |     18.088 ms |   0.2499 ms |   0.2337 ms |     18.037 ms |
| UpdateContacts |                  1000 |         EFCore 5.0.0 |     25.225 ms |   0.7244 ms |   2.1361 ms |     25.444 ms |
| UpdateContacts |                  2500 |         EFCore 5.0.0 |     49.258 ms |   0.7812 ms |   0.6925 ms |     49.024 ms |
| UpdateContacts |                  5000 |         EFCore 5.0.0 |     90.776 ms |   0.4608 ms |   0.3598 ms |     90.769 ms |
| UpdateContacts |                    10 | EFCore+Security5.0.0 |     22.537 ms |   0.5998 ms |   1.6919 ms |     21.669 ms |
| UpdateContacts |                    20 | EFCore+Security5.0.0 |     37.966 ms |   0.4604 ms |   0.9301 ms |     37.690 ms |
| UpdateContacts |                    50 | EFCore+Security5.0.0 |     79.589 ms |   1.5890 ms |   3.6509 ms |     77.660 ms |
| UpdateContacts |                   100 | EFCore+Security5.0.0 |    136.937 ms |   1.1667 ms |   1.0343 ms |    136.883 ms |
| UpdateContacts |                   250 | EFCore+Security5.0.0 |    325.457 ms |   0.6207 ms |   0.5502 ms |    325.637 ms |
| UpdateContacts |                   500 | EFCore+Security5.0.0 |    647.989 ms |   3.5915 ms |   3.3595 ms |    648.419 ms |
| UpdateContacts |                  1000 | EFCore+Security5.0.0 |  1,281.299 ms |   8.7710 ms |   8.2044 ms |  1,280.678 ms |
| UpdateContacts |                  2500 | EFCore+Security5.0.0 |  3,161.325 ms |  10.4627 ms |   9.7868 ms |  3,160.149 ms |
| UpdateContacts |                  5000 | EFCore+Security5.0.0 |  6,267.947 ms |  16.6823 ms |  13.9305 ms |  6,265.435 ms |
| UpdateContacts |                    10 |           XPO 20.2.5 |      5.841 ms |   0.1170 ms |   0.3449 ms |      5.767 ms |
| UpdateContacts |                    20 |           XPO 20.2.5 |      8.603 ms |   0.2543 ms |   0.7417 ms |      8.450 ms |
| UpdateContacts |                    50 |           XPO 20.2.5 |     14.698 ms |   0.4119 ms |   1.2079 ms |     14.168 ms |
| UpdateContacts |                   100 |           XPO 20.2.5 |     26.857 ms |   0.3409 ms |   0.3022 ms |     26.863 ms |
| UpdateContacts |                   250 |           XPO 20.2.5 |     66.651 ms |   1.2830 ms |   1.3728 ms |     67.034 ms |
| UpdateContacts |                   500 |           XPO 20.2.5 |    113.578 ms |   1.8060 ms |   1.6009 ms |    113.766 ms |
| UpdateContacts |                  1000 |           XPO 20.2.5 |    236.457 ms |   2.7877 ms |   2.4712 ms |    236.292 ms |
| UpdateContacts |                  2500 |           XPO 20.2.5 |    542.876 ms |   6.3809 ms |   5.3283 ms |    544.327 ms |
| UpdateContacts |                  5000 |           XPO 20.2.5 |  1,043.535 ms |  12.9782 ms |  10.1325 ms |  1,041.471 ms |
| UpdateContacts |                    10 |  XPO+Security 20.2.5 |     90.147 ms |   1.4245 ms |   1.3325 ms |     90.236 ms |
| UpdateContacts |                    20 |  XPO+Security 20.2.5 |    169.677 ms |   2.2782 ms |   2.0195 ms |    170.361 ms |
| UpdateContacts |                    50 |  XPO+Security 20.2.5 |    407.658 ms |   3.1210 ms |   2.9194 ms |    407.413 ms |
| UpdateContacts |                   100 |  XPO+Security 20.2.5 |    794.231 ms |   5.4557 ms |   5.1033 ms |    794.971 ms |
| UpdateContacts |                   250 |  XPO+Security 20.2.5 |  1,966.607 ms |   7.5431 ms |   6.6867 ms |  1,965.127 ms |
| UpdateContacts |                   500 |  XPO+Security 20.2.5 |  3,939.288 ms |   7.0062 ms |   6.5536 ms |  3,940.703 ms |
| UpdateContacts |                  1000 |  XPO+Security 20.2.5 |  7,834.771 ms |  29.6945 ms |  26.3234 ms |  7,839.602 ms |
| UpdateContacts |                  2500 |  XPO+Security 20.2.5 | 19,379.141 ms | 144.1270 ms | 120.3526 ms | 19,362.193 ms |
| UpdateContacts |                  5000 |  XPO+Security 20.2.5 | 38,638.566 ms | 165.0972 ms | 154.4320 ms | 38,628.896 ms |
