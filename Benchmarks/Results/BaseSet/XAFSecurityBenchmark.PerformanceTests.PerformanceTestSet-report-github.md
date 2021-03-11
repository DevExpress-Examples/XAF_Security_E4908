``` ini

BenchmarkDotNet=v0.12.1, OS=Windows 10.0.18363.1379 (1909/November2018Update/19H2)
Intel Core i7-6700K CPU 4.00GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET Core SDK=5.0.103
  [Host]     : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT
  Job-QYLVQV : .NET Core 5.0.3 (CoreCLR 5.0.321.7212, CoreFX 5.0.321.7212), X64 RyuJIT

Runtime=.NET Core 5.0  InvocationCount=1  RunStrategy=Throughput  
UnrollFactor=1  

```
|             Method | ItemsForTestIteration |         TestProvider |          Mean |       Error |      StdDev |        Median |
|------------------- |---------------------- |--------------------- |--------------:|------------:|------------:|--------------:|
|        GetContacts |                    10 |         EFCore 5.0.0 |      1.370 ms |   0.0272 ms |   0.0498 ms |      1.367 ms |
|        GetContacts |                    20 |         EFCore 5.0.0 |      1.415 ms |   0.0283 ms |   0.0524 ms |      1.393 ms |
|        GetContacts |                    50 |         EFCore 5.0.0 |      1.708 ms |   0.0315 ms |   0.0295 ms |      1.710 ms |
|        GetContacts |                   100 |         EFCore 5.0.0 |      2.017 ms |   0.0345 ms |   0.0424 ms |      2.015 ms |
|        GetContacts |                   250 |         EFCore 5.0.0 |      3.065 ms |   0.0381 ms |   0.0338 ms |      3.054 ms |
|        GetContacts |                   500 |         EFCore 5.0.0 |      4.741 ms |   0.0935 ms |   0.1973 ms |      4.717 ms |
|        GetContacts |                  1000 |         EFCore 5.0.0 |      8.098 ms |   0.1454 ms |   0.2307 ms |      8.053 ms |
|        GetContacts |                  2500 |         EFCore 5.0.0 |     18.643 ms |   0.2498 ms |   0.2337 ms |     18.673 ms |
|        GetContacts |                  5000 |         EFCore 5.0.0 |     19.457 ms |   0.3092 ms |   0.6915 ms |     19.254 ms |
|        GetContacts |                    10 | EFCore+Security5.0.0 |     12.734 ms |   0.1040 ms |   0.0973 ms |     12.761 ms |
|        GetContacts |                    20 | EFCore+Security5.0.0 |     17.683 ms |   0.1346 ms |   0.1259 ms |     17.678 ms |
|        GetContacts |                    50 | EFCore+Security5.0.0 |     26.966 ms |   0.4772 ms |   0.9640 ms |     26.794 ms |
|        GetContacts |                   100 | EFCore+Security5.0.0 |     44.743 ms |   0.8656 ms |   0.8097 ms |     44.615 ms |
|        GetContacts |                   250 | EFCore+Security5.0.0 |    101.844 ms |   0.4467 ms |   0.4179 ms |    101.797 ms |
|        GetContacts |                   500 | EFCore+Security5.0.0 |    196.163 ms |   0.3518 ms |   0.3291 ms |    196.120 ms |
|        GetContacts |                  1000 | EFCore+Security5.0.0 |    407.876 ms |   3.3360 ms |   3.1205 ms |    406.742 ms |
|        GetContacts |                  2500 | EFCore+Security5.0.0 |  1,139.486 ms |   3.8188 ms |   3.5721 ms |  1,140.696 ms |
|        GetContacts |                  5000 | EFCore+Security5.0.0 |  2,847.249 ms |   6.1887 ms |   5.4862 ms |  2,847.469 ms |
|        GetContacts |                    10 |           XPO 20.2.5 |      2.781 ms |   0.0553 ms |   0.1437 ms |      2.759 ms |
|        GetContacts |                    20 |           XPO 20.2.5 |      3.760 ms |   0.0932 ms |   0.2749 ms |      3.750 ms |
|        GetContacts |                    50 |           XPO 20.2.5 |      5.051 ms |   0.1220 ms |   0.3597 ms |      4.904 ms |
|        GetContacts |                   100 |           XPO 20.2.5 |      7.246 ms |   0.1395 ms |   0.3934 ms |      7.117 ms |
|        GetContacts |                   250 |           XPO 20.2.5 |     15.311 ms |   0.1345 ms |   0.1050 ms |     15.317 ms |
|        GetContacts |                   500 |           XPO 20.2.5 |     26.649 ms |   0.5342 ms |   1.5751 ms |     25.981 ms |
|        GetContacts |                  1000 |           XPO 20.2.5 |     53.689 ms |   0.6981 ms |   0.6530 ms |     53.650 ms |
|        GetContacts |                  2500 |           XPO 20.2.5 |    154.995 ms |   1.9742 ms |   1.8467 ms |    155.227 ms |
|        GetContacts |                  5000 |           XPO 20.2.5 |    268.775 ms |   2.6054 ms |   2.4371 ms |    268.799 ms |
|        GetContacts |                    10 |  XPO+Security 20.2.5 |      7.149 ms |   0.1164 ms |   0.1032 ms |      7.183 ms |
|        GetContacts |                    20 |  XPO+Security 20.2.5 |      7.304 ms |   0.1451 ms |   0.3559 ms |      7.148 ms |
|        GetContacts |                    50 |  XPO+Security 20.2.5 |     12.335 ms |   0.1100 ms |   0.1578 ms |     12.317 ms |
|        GetContacts |                   100 |  XPO+Security 20.2.5 |     20.567 ms |   0.2113 ms |   0.1650 ms |     20.562 ms |
|        GetContacts |                   250 |  XPO+Security 20.2.5 |     48.055 ms |   0.5174 ms |   0.4840 ms |     48.007 ms |
|        GetContacts |                   500 |  XPO+Security 20.2.5 |     94.178 ms |   0.3827 ms |   0.3196 ms |     94.115 ms |
|        GetContacts |                  1000 |  XPO+Security 20.2.5 |    188.849 ms |   0.5866 ms |   0.5200 ms |    188.686 ms |
|        GetContacts |                  2500 |  XPO+Security 20.2.5 |    474.939 ms |   4.5721 ms |   4.2767 ms |    475.590 ms |
|        GetContacts |                  5000 |  XPO+Security 20.2.5 |    967.130 ms |   5.2821 ms |   4.9409 ms |    968.365 ms |
|           GetTasks |                    10 |         EFCore 5.0.0 |      1.581 ms |   0.0317 ms |   0.0934 ms |      1.561 ms |
|           GetTasks |                    20 |         EFCore 5.0.0 |      1.865 ms |   0.0283 ms |   0.0265 ms |      1.865 ms |
|           GetTasks |                    50 |         EFCore 5.0.0 |      2.449 ms |   0.0462 ms |   0.0386 ms |      2.436 ms |
|           GetTasks |                   100 |         EFCore 5.0.0 |      3.284 ms |   0.0644 ms |   0.0716 ms |      3.268 ms |
|           GetTasks |                   250 |         EFCore 5.0.0 |     19.132 ms |   0.1968 ms |   0.1841 ms |     19.137 ms |
|           GetTasks |                   500 |         EFCore 5.0.0 |     22.137 ms |   0.2116 ms |   0.1979 ms |     22.131 ms |
|           GetTasks |                  1000 |         EFCore 5.0.0 |     28.301 ms |   0.2106 ms |   0.1867 ms |     28.265 ms |
|           GetTasks |                  2500 |         EFCore 5.0.0 |     47.406 ms |   0.4392 ms |   0.4109 ms |     47.295 ms |
|           GetTasks |                  5000 |         EFCore 5.0.0 |     78.365 ms |   0.8048 ms |   0.7528 ms |     78.234 ms |
|           GetTasks |                    10 | EFCore+Security5.0.0 |     26.763 ms |   0.5098 ms |   0.4769 ms |     26.697 ms |
|           GetTasks |                    20 | EFCore+Security5.0.0 |     37.863 ms |   0.7464 ms |   1.5246 ms |     37.429 ms |
|           GetTasks |                    50 | EFCore+Security5.0.0 |     75.082 ms |   0.4428 ms |   0.3698 ms |     75.003 ms |
|           GetTasks |                   100 | EFCore+Security5.0.0 |    134.820 ms |   0.6038 ms |   0.5352 ms |    134.943 ms |
|           GetTasks |                   250 | EFCore+Security5.0.0 |    325.692 ms |   1.0844 ms |   0.8466 ms |    325.931 ms |
|           GetTasks |                   500 | EFCore+Security5.0.0 |    634.178 ms |   1.8961 ms |   1.6809 ms |    634.388 ms |
|           GetTasks |                  1000 | EFCore+Security5.0.0 |  1,253.319 ms |   3.8308 ms |   3.5833 ms |  1,251.638 ms |
|           GetTasks |                  2500 | EFCore+Security5.0.0 |  3,105.556 ms |   6.7522 ms |   5.9856 ms |  3,103.479 ms |
|           GetTasks |                  5000 | EFCore+Security5.0.0 |  6,198.760 ms |   5.2770 ms |   4.9361 ms |  6,198.716 ms |
|           GetTasks |                    10 |           XPO 20.2.5 |      4.184 ms |   0.0938 ms |   0.2735 ms |      4.160 ms |
|           GetTasks |                    20 |           XPO 20.2.5 |      6.792 ms |   0.1335 ms |   0.1311 ms |      6.790 ms |
|           GetTasks |                    50 |           XPO 20.2.5 |     12.080 ms |   0.1934 ms |   0.1615 ms |     12.104 ms |
|           GetTasks |                   100 |           XPO 20.2.5 |     21.896 ms |   0.4320 ms |   0.7098 ms |     21.962 ms |
|           GetTasks |                   250 |           XPO 20.2.5 |     44.621 ms |   0.8726 ms |   0.8162 ms |     44.471 ms |
|           GetTasks |                   500 |           XPO 20.2.5 |     79.334 ms |   1.1109 ms |   1.0391 ms |     79.165 ms |
|           GetTasks |                  1000 |           XPO 20.2.5 |    153.552 ms |   2.9924 ms |   3.8909 ms |    151.268 ms |
|           GetTasks |                  2500 |           XPO 20.2.5 |    304.600 ms |   1.2659 ms |   1.1221 ms |    304.544 ms |
|           GetTasks |                  5000 |           XPO 20.2.5 |    558.145 ms |   1.1374 ms |   1.0083 ms |    558.086 ms |
|           GetTasks |                    10 |  XPO+Security 20.2.5 |     38.900 ms |   0.3062 ms |   0.2715 ms |     38.827 ms |
|           GetTasks |                    20 |  XPO+Security 20.2.5 |     88.789 ms |   0.7735 ms |   0.7235 ms |     88.897 ms |
|           GetTasks |                    50 |  XPO+Security 20.2.5 |    202.839 ms |   2.1036 ms |   1.9677 ms |    203.459 ms |
|           GetTasks |                   100 |  XPO+Security 20.2.5 |    416.492 ms |   1.7631 ms |   1.5629 ms |    416.204 ms |
|           GetTasks |                   250 |  XPO+Security 20.2.5 |  1,069.608 ms |   7.0982 ms |   6.6397 ms |  1,068.892 ms |
|           GetTasks |                   500 |  XPO+Security 20.2.5 |  2,084.031 ms |   6.5958 ms |   6.1697 ms |  2,084.276 ms |
|           GetTasks |                  1000 |  XPO+Security 20.2.5 |  4,192.824 ms |  11.9122 ms |  10.5599 ms |  4,193.358 ms |
|           GetTasks |                  2500 |  XPO+Security 20.2.5 | 10,222.239 ms |  52.3252 ms |  46.3849 ms | 10,205.081 ms |
|           GetTasks |                  5000 |  XPO+Security 20.2.5 | 20,674.707 ms | 175.0382 ms | 163.7309 ms | 20,588.573 ms |
|      InsertContact |                    10 |         EFCore 5.0.0 |     37.689 ms |   0.2679 ms |   0.2237 ms |     37.746 ms |
|      InsertContact |                    20 |         EFCore 5.0.0 |     48.919 ms |   0.3682 ms |   0.3074 ms |     48.839 ms |
|      InsertContact |                    50 |         EFCore 5.0.0 |    111.202 ms |   1.8318 ms |   1.7134 ms |    110.659 ms |
|      InsertContact |                   100 |         EFCore 5.0.0 |    191.713 ms |   2.2066 ms |   2.0640 ms |    192.052 ms |
|      InsertContact |                   250 |         EFCore 5.0.0 |    323.199 ms |   2.5707 ms |   2.4046 ms |    322.728 ms |
|      InsertContact |                   500 |         EFCore 5.0.0 |    519.324 ms |   4.0656 ms |   3.8030 ms |    520.393 ms |
|      InsertContact |                  1000 |         EFCore 5.0.0 |    934.478 ms |  10.2259 ms |   9.5653 ms |    940.046 ms |
|      InsertContact |                  2500 |         EFCore 5.0.0 |  2,251.004 ms |   9.7588 ms |   9.1284 ms |  2,251.772 ms |
|      InsertContact |                  5000 |         EFCore 5.0.0 |  4,434.771 ms |  25.8231 ms |  21.5634 ms |  4,434.932 ms |
|      InsertContact |                    10 | EFCore+Security5.0.0 |     49.538 ms |   0.7447 ms |   0.6966 ms |     49.625 ms |
|      InsertContact |                    20 | EFCore+Security5.0.0 |     63.285 ms |   1.2374 ms |   1.5650 ms |     62.654 ms |
|      InsertContact |                    50 | EFCore+Security5.0.0 |    130.787 ms |   2.4338 ms |   2.2766 ms |    131.173 ms |
|      InsertContact |                   100 | EFCore+Security5.0.0 |    219.141 ms |   1.8946 ms |   1.6795 ms |    218.968 ms |
|      InsertContact |                   250 | EFCore+Security5.0.0 |    377.414 ms |   1.3308 ms |   1.2448 ms |    377.435 ms |
|      InsertContact |                   500 | EFCore+Security5.0.0 |    612.042 ms |   3.1113 ms |   2.9103 ms |    611.100 ms |
|      InsertContact |                  1000 | EFCore+Security5.0.0 |  1,101.866 ms |   6.5962 ms |   6.1701 ms |  1,100.915 ms |
|      InsertContact |                  2500 | EFCore+Security5.0.0 |  2,706.443 ms |   9.3191 ms |   8.2611 ms |  2,706.446 ms |
|      InsertContact |                  5000 | EFCore+Security5.0.0 |  5,261.035 ms |  22.6462 ms |  18.9106 ms |  5,258.341 ms |
|      InsertContact |                    10 |           XPO 20.2.5 |      9.610 ms |   0.1902 ms |   0.5142 ms |      9.407 ms |
|      InsertContact |                    20 |           XPO 20.2.5 |     16.315 ms |   0.3101 ms |   0.6740 ms |     16.146 ms |
|      InsertContact |                    50 |           XPO 20.2.5 |     40.767 ms |   1.0831 ms |   3.1936 ms |     39.998 ms |
|      InsertContact |                   100 |           XPO 20.2.5 |     76.946 ms |   1.5213 ms |   1.4230 ms |     76.544 ms |
|      InsertContact |                   250 |           XPO 20.2.5 |    188.958 ms |   2.0227 ms |   1.8920 ms |    188.106 ms |
|      InsertContact |                   500 |           XPO 20.2.5 |    375.124 ms |   7.2184 ms |   7.4128 ms |    372.255 ms |
|      InsertContact |                  1000 |           XPO 20.2.5 |    762.798 ms |   3.7821 ms |   3.5377 ms |    761.090 ms |
|      InsertContact |                  2500 |           XPO 20.2.5 |  2,015.533 ms |  12.0223 ms |  10.6574 ms |  2,013.454 ms |
|      InsertContact |                  5000 |           XPO 20.2.5 |  4,336.850 ms |  33.7860 ms |  31.6035 ms |  4,338.368 ms |
|      InsertContact |                    10 |  XPO+Security 20.2.5 |     17.945 ms |   0.7682 ms |   2.2650 ms |     18.197 ms |
|      InsertContact |                    20 |  XPO+Security 20.2.5 |     30.597 ms |   1.0538 ms |   3.1071 ms |     30.735 ms |
|      InsertContact |                    50 |  XPO+Security 20.2.5 |     62.213 ms |   1.2401 ms |   2.3293 ms |     61.918 ms |
|      InsertContact |                   100 |  XPO+Security 20.2.5 |    115.297 ms |   2.1233 ms |   2.5276 ms |    115.071 ms |
|      InsertContact |                   250 |  XPO+Security 20.2.5 |    283.511 ms |   1.8612 ms |   1.5542 ms |    283.665 ms |
|      InsertContact |                   500 |  XPO+Security 20.2.5 |    558.222 ms |   5.6017 ms |   5.2398 ms |    555.190 ms |
|      InsertContact |                  1000 |  XPO+Security 20.2.5 |  1,143.385 ms |   8.4557 ms |   7.9095 ms |  1,143.589 ms |
|      InsertContact |                  2500 |  XPO+Security 20.2.5 |  3,217.575 ms |  38.9534 ms |  36.4371 ms |  3,211.357 ms |
|      InsertContact |                  5000 |  XPO+Security 20.2.5 |  6,529.839 ms |  58.6152 ms |  51.9608 ms |  6,530.911 ms |
| InsertEmptyContact |                    10 |         EFCore 5.0.0 |     16.224 ms |   0.3190 ms |   0.3133 ms |     16.316 ms |
| InsertEmptyContact |                    20 |         EFCore 5.0.0 |     21.386 ms |   0.3499 ms |   0.3273 ms |     21.245 ms |
| InsertEmptyContact |                    50 |         EFCore 5.0.0 |     49.791 ms |   0.4863 ms |   0.3797 ms |     49.866 ms |
| InsertEmptyContact |                   100 |         EFCore 5.0.0 |     67.251 ms |   1.3347 ms |   1.8269 ms |     66.439 ms |
| InsertEmptyContact |                   250 |         EFCore 5.0.0 |    122.076 ms |   0.9954 ms |   0.9311 ms |    122.145 ms |
| InsertEmptyContact |                   500 |         EFCore 5.0.0 |    191.099 ms |   2.3393 ms |   2.1882 ms |    191.660 ms |
| InsertEmptyContact |                  1000 |         EFCore 5.0.0 |    333.375 ms |   3.4382 ms |   3.2161 ms |    333.570 ms |
| InsertEmptyContact |                  2500 |         EFCore 5.0.0 |    776.824 ms |  10.8598 ms |  10.1583 ms |    775.723 ms |
| InsertEmptyContact |                  5000 |         EFCore 5.0.0 |  1,514.629 ms |   4.9029 ms |   4.5862 ms |  1,514.100 ms |
| InsertEmptyContact |                    10 | EFCore+Security5.0.0 |     21.824 ms |   0.4334 ms |   0.7120 ms |     22.194 ms |
| InsertEmptyContact |                    20 | EFCore+Security5.0.0 |     28.582 ms |   0.5497 ms |   0.5645 ms |     28.774 ms |
| InsertEmptyContact |                    50 | EFCore+Security5.0.0 |     59.096 ms |   0.4521 ms |   0.3530 ms |     59.135 ms |
| InsertEmptyContact |                   100 | EFCore+Security5.0.0 |     82.284 ms |   1.6037 ms |   2.2482 ms |     81.504 ms |
| InsertEmptyContact |                   250 | EFCore+Security5.0.0 |    148.043 ms |   0.7452 ms |   0.6606 ms |    148.113 ms |
| InsertEmptyContact |                   500 | EFCore+Security5.0.0 |    235.686 ms |   1.9234 ms |   1.7051 ms |    235.096 ms |
| InsertEmptyContact |                  1000 | EFCore+Security5.0.0 |    413.528 ms |   1.8943 ms |   1.7719 ms |    414.249 ms |
| InsertEmptyContact |                  2500 | EFCore+Security5.0.0 |    949.982 ms |   7.2859 ms |   6.8152 ms |    949.438 ms |
| InsertEmptyContact |                  5000 | EFCore+Security5.0.0 |  1,864.973 ms |   7.5602 ms |   6.7019 ms |  1,862.080 ms |
| InsertEmptyContact |                    10 |           XPO 20.2.5 |      6.256 ms |   0.1236 ms |   0.2890 ms |      6.215 ms |
| InsertEmptyContact |                    20 |           XPO 20.2.5 |     10.476 ms |   0.3202 ms |   0.9136 ms |     10.199 ms |
| InsertEmptyContact |                    50 |           XPO 20.2.5 |     23.785 ms |   0.4737 ms |   1.2227 ms |     23.794 ms |
| InsertEmptyContact |                   100 |           XPO 20.2.5 |     44.266 ms |   0.8816 ms |   1.9719 ms |     44.063 ms |
| InsertEmptyContact |                   250 |           XPO 20.2.5 |    104.173 ms |   1.5000 ms |   1.3297 ms |    104.166 ms |
| InsertEmptyContact |                   500 |           XPO 20.2.5 |    207.360 ms |   2.4859 ms |   2.0758 ms |    206.602 ms |
| InsertEmptyContact |                  1000 |           XPO 20.2.5 |    410.068 ms |   7.6594 ms |   7.1646 ms |    406.967 ms |
| InsertEmptyContact |                  2500 |           XPO 20.2.5 |  1,027.423 ms |   8.3691 ms |   7.4190 ms |  1,026.597 ms |
| InsertEmptyContact |                  5000 |           XPO 20.2.5 |  2,102.060 ms |   8.1247 ms |   7.2023 ms |  2,101.403 ms |
| InsertEmptyContact |                    10 |  XPO+Security 20.2.5 |      8.557 ms |   0.1946 ms |   0.5519 ms |      8.377 ms |
| InsertEmptyContact |                    20 |  XPO+Security 20.2.5 |     12.832 ms |   0.2428 ms |   0.6480 ms |     12.628 ms |
| InsertEmptyContact |                    50 |  XPO+Security 20.2.5 |     28.845 ms |   0.5762 ms |   1.3806 ms |     28.451 ms |
| InsertEmptyContact |                   100 |  XPO+Security 20.2.5 |     50.461 ms |   0.9805 ms |   1.0491 ms |     50.215 ms |
| InsertEmptyContact |                   250 |  XPO+Security 20.2.5 |    123.448 ms |   2.2850 ms |   2.0256 ms |    123.987 ms |
| InsertEmptyContact |                   500 |  XPO+Security 20.2.5 |    242.628 ms |   2.0732 ms |   1.9393 ms |    242.280 ms |
| InsertEmptyContact |                  1000 |  XPO+Security 20.2.5 |    475.639 ms |   3.0295 ms |   2.8338 ms |    475.563 ms |
| InsertEmptyContact |                  2500 |  XPO+Security 20.2.5 |  1,216.567 ms |  10.5440 ms |   9.8629 ms |  1,214.292 ms |
| InsertEmptyContact |                  5000 |  XPO+Security 20.2.5 |  2,503.027 ms |  28.3400 ms |  25.1227 ms |  2,501.250 ms |
|     UpdateContacts |                    10 |         EFCore 5.0.0 |      2.810 ms |   0.0842 ms |   0.2468 ms |      2.801 ms |
|     UpdateContacts |                    20 |         EFCore 5.0.0 |      3.857 ms |   0.1149 ms |   0.3389 ms |      3.702 ms |
|     UpdateContacts |                    50 |         EFCore 5.0.0 |      6.304 ms |   0.1915 ms |   0.5241 ms |      6.471 ms |
|     UpdateContacts |                   100 |         EFCore 5.0.0 |     10.674 ms |   0.5507 ms |   1.6150 ms |     10.505 ms |
|     UpdateContacts |                   250 |         EFCore 5.0.0 |     22.372 ms |   0.6771 ms |   1.9858 ms |     22.271 ms |
|     UpdateContacts |                   500 |         EFCore 5.0.0 |     41.286 ms |   0.3394 ms |   0.2834 ms |     41.297 ms |
|     UpdateContacts |                  1000 |         EFCore 5.0.0 |     83.497 ms |   1.4918 ms |   1.3954 ms |     82.867 ms |
|     UpdateContacts |                  2500 |         EFCore 5.0.0 |    211.406 ms |   2.4095 ms |   2.0120 ms |    211.277 ms |
|     UpdateContacts |                  5000 |         EFCore 5.0.0 |    465.541 ms |   7.9612 ms |   7.4469 ms |    466.408 ms |
|     UpdateContacts |                    10 | EFCore+Security5.0.0 |     15.952 ms |   0.2740 ms |   0.2563 ms |     15.939 ms |
|     UpdateContacts |                    20 | EFCore+Security5.0.0 |     23.704 ms |   0.3459 ms |   0.3235 ms |     23.687 ms |
|     UpdateContacts |                    50 | EFCore+Security5.0.0 |     35.559 ms |   0.3953 ms |   0.6818 ms |     35.413 ms |
|     UpdateContacts |                   100 | EFCore+Security5.0.0 |     62.368 ms |   0.4284 ms |   0.3798 ms |     62.354 ms |
|     UpdateContacts |                   250 | EFCore+Security5.0.0 |    139.360 ms |   0.5316 ms |   0.4973 ms |    139.451 ms |
|     UpdateContacts |                   500 | EFCore+Security5.0.0 |    273.831 ms |   2.9285 ms |   2.7393 ms |    273.433 ms |
|     UpdateContacts |                  1000 | EFCore+Security5.0.0 |    559.966 ms |   4.5557 ms |   4.2614 ms |    559.522 ms |
|     UpdateContacts |                  2500 | EFCore+Security5.0.0 |  1,539.082 ms |   3.2382 ms |   3.0290 ms |  1,538.761 ms |
|     UpdateContacts |                  5000 | EFCore+Security5.0.0 |  3,636.916 ms |   9.1964 ms |   7.6794 ms |  3,637.213 ms |
|     UpdateContacts |                    10 |           XPO 20.2.5 |      5.582 ms |   0.1501 ms |   0.4305 ms |      5.596 ms |
|     UpdateContacts |                    20 |           XPO 20.2.5 |      8.366 ms |   0.2452 ms |   0.7114 ms |      8.145 ms |
|     UpdateContacts |                    50 |           XPO 20.2.5 |     15.322 ms |   0.2815 ms |   0.6411 ms |     15.099 ms |
|     UpdateContacts |                   100 |           XPO 20.2.5 |     27.150 ms |   0.5383 ms |   1.4825 ms |     26.639 ms |
|     UpdateContacts |                   250 |           XPO 20.2.5 |     64.693 ms |   1.2876 ms |   2.3545 ms |     63.611 ms |
|     UpdateContacts |                   500 |           XPO 20.2.5 |    131.991 ms |   2.3638 ms |   3.3138 ms |    132.461 ms |
|     UpdateContacts |                  1000 |           XPO 20.2.5 |    246.728 ms |   2.8060 ms |   2.1907 ms |    246.711 ms |
|     UpdateContacts |                  2500 |           XPO 20.2.5 |    647.841 ms |  12.7980 ms |  23.4019 ms |    647.425 ms |
|     UpdateContacts |                  5000 |           XPO 20.2.5 |  1,267.308 ms |   7.1526 ms |   5.9727 ms |  1,268.909 ms |
|     UpdateContacts |                    10 |  XPO+Security 20.2.5 |     10.709 ms |   0.1556 ms |   0.3698 ms |     10.612 ms |
|     UpdateContacts |                    20 |  XPO+Security 20.2.5 |     16.588 ms |   0.3277 ms |   0.7851 ms |     16.336 ms |
|     UpdateContacts |                    50 |  XPO+Security 20.2.5 |     35.404 ms |   0.6334 ms |   1.4167 ms |     35.085 ms |
|     UpdateContacts |                   100 |  XPO+Security 20.2.5 |     60.569 ms |   1.1959 ms |   1.5125 ms |     61.223 ms |
|     UpdateContacts |                   250 |  XPO+Security 20.2.5 |    140.151 ms |   1.6140 ms |   1.4307 ms |    139.929 ms |
|     UpdateContacts |                   500 |  XPO+Security 20.2.5 |    285.815 ms |   3.4659 ms |   2.8942 ms |    285.891 ms |
|     UpdateContacts |                  1000 |  XPO+Security 20.2.5 |    601.347 ms |   3.6213 ms |   2.8273 ms |    601.644 ms |
|     UpdateContacts |                  2500 |  XPO+Security 20.2.5 |  1,603.247 ms |  51.9852 ms | 153.2795 ms |  1,610.661 ms |
|     UpdateContacts |                  5000 |  XPO+Security 20.2.5 |  3,158.913 ms |  12.1084 ms |  11.3262 ms |  3,158.280 ms |
