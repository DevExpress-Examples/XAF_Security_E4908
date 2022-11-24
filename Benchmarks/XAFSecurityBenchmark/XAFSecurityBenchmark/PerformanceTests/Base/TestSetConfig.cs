using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Configuration;
using System.Linq;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters.Json;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;

namespace XAFSecurityBenchmark.PerformanceTests {
    public class TestSetConfig : ManualConfig {
        //We will take user[0] to perform test operations. Another users increase amount of data in db
        public static readonly string[] Users = new string[] { "John", "Bob", "Crimp", "Yuri", "Pavel" };
        public static readonly string TestUser = Users[0];
        public static int[] ItemsForTestIteration = new int[] { 10, 20, 50, 100, 250, 500, 1000, 2500, 5000 };
        public static int ContactCountPerUserToCreate = ItemsForTestIteration.Max();
        public static readonly int TasksAssigedToContact = 10;
        public static readonly int TasksLinkedToContact = 10;

        public static string XPOConnectionStrings => "XpoProvider=MSSqlServer;" + ConfigurationManager.ConnectionStrings["ConnectionString_XPO"].ConnectionString;
        public static string EFCoreConnectionStrings => ConfigurationManager.ConnectionStrings["ConnectionString_EFCore"].ConnectionString;

        public TestSetConfig() {
            var job = Job.Default.WithRuntime(CoreRuntime.Core60);
                //.WithMaxIterationCount(85);
            job.Run.RunStrategy = BenchmarkDotNet.Engines.RunStrategy.Throughput;
            AddJob(job);
            Orderer = new TestSetOrderProvider();
            AddExporter(new JsonExporter("", true, true));
            AddValidator(JitOptimizationsValidator.DontFailOnError);
            AddLogger(DefaultConfig.Instance.GetLoggers().ToArray());
            AddExporter(DefaultConfig.Instance.GetExporters().ToArray());
            AddColumnProvider(DefaultConfig.Instance.GetColumnProviders().ToArray());
        }

        private class TestSetOrderProvider : IOrderer {

            public bool SeparateLogicalGroups {
                get {
                    return true;
                }
            }

            public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase) {
                return benchmarksCase;
            }

            public IEnumerable<BenchmarkCase> GetSummaryOrder(ImmutableArray<BenchmarkCase> benchmarksCase, Summary summary) {
                return benchmarksCase
                    .OrderBy(t => t.Parameters["Count"])
                    .ThenBy(t => t.Descriptor.WorkloadMethodDisplayInfo.ToString())
                    .ThenBy(t => t.Parameters["TestProvider"].ToString());
            }

            public string GetHighlightGroupKey(BenchmarkCase benchmarkCase) {
                return null;
            }

            public string GetLogicalGroupKey(ImmutableArray<BenchmarkCase> allBenchmarksCases, BenchmarkCase benchmarkCase) {
                return null;
            }

            public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups) {
                return logicalGroups;
            }

            public IEnumerable<BenchmarkCase> GetExecutionOrder(ImmutableArray<BenchmarkCase> benchmarksCase, IEnumerable<BenchmarkLogicalGroupRule> order = null) {
                return benchmarksCase;
            }

            public IEnumerable<IGrouping<string, BenchmarkCase>> GetLogicalGroupOrder(IEnumerable<IGrouping<string, BenchmarkCase>> logicalGroups, IEnumerable<BenchmarkLogicalGroupRule> order = null) {
                return logicalGroups;
            }
        }
    }
}
