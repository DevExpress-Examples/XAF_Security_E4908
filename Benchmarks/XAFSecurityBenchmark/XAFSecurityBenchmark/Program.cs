using System;
using System.IO;
using BenchmarkDotNet.Running;
using XAFSecurityBenchmark.PerformanceTests;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            for(int i = 0; i < args.Length; i++) {
                if(args[i].ToLower() == "-count") {
                    var counts = new List<int>();
                    for(i++; i < args.Length; i++) {
                        int count;
                        if(Int32.TryParse(args[i], out count)) {
                            counts.Add(count);
                        }
                        else {
                            i--;
                            break;
                        }
                    }
                    if(counts.Count > 0) {
                        TestSetConfig.ItemsForTestIteration = counts.ToArray();
                    }
                }
            }
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.Latest;
            //ExecuteBenchmarkManually<XPOTestProviderWithSecurity>(t => t.GetTasks(50));
            //ExecuteBenchmarkManually<EFCoreTestProviderWithSecurity>(t => t.GetTasks(50));
            //EFCoreTestProviderWithSecurity

            //Environment.SetEnvironmentVariable("R_HOME", @"c:\WorkTools\R\R-4.0.3\");
            //string test = Environment.GetEnvironmentVariable("R_HOME");

            var summary = BenchmarkRunner.Run<PerformanceTestSet>(new TestSetConfig());
            //string resultsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BenchmarkDotNet.Artifacts", "results", "XAFSecurityBenchmark.PerformanceTests.PerformanceTestSet-report.html");
            //var psi = new ProcessStartInfo(resultsPath);
            //psi.UseShellExecute = true;
            //Process.Start(psi);
        }


        /*private static TimeSpan ExecuteBenchmarkManually<T>(Action<T> target) where T : TestProviderBase, new() {
            T testProvider = new T();
            Console.WriteLine($"Run {testProvider.GetType().Name}");
            testProvider.GlobalTestDataSetup(true);

            testProvider.InitSession();
            Stopwatch swWarmup = new Stopwatch();
            swWarmup.Start();
            target.Invoke(testProvider);
            Console.WriteLine($"Warmup--Time:{swWarmup.Elapsed}");
            swWarmup.Stop();
            testProvider.TearDownSession();

            testProvider.InitSession();
            target.Invoke(testProvider);
            testProvider.TearDownSession();

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();

            testProvider.InitSession();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            target.Invoke(testProvider);
            sw.Stop();

            Console.WriteLine($"Result--Time:{sw.Elapsed}");
            Console.WriteLine();
            Console.WriteLine();
            return sw.Elapsed;
        }*/
    }
}
