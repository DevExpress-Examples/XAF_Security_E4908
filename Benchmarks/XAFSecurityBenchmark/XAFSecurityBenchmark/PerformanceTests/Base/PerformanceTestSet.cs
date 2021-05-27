using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters;

namespace XAFSecurityBenchmark.PerformanceTests {
    //[CsvMeasurementsExporter]
    //[RPlotExporter]
    public class PerformanceTestSet {
        [ParamsSource(nameof(GetItemsForTestIteration))]
        public int ItemsForTestIteration;

        [ParamsSource(nameof(TestProviders))]
        public TestProviderBase TestProvider;

        public IEnumerable<int> GetItemsForTestIteration() {
            return TestSetConfig.ItemsForTestIteration;
        }

        public IEnumerable<TestProviderBase> TestProviders() {
            yield return new EFCoreTestProviderWithSecurity();
            yield return new EFCoreTestProvider();


            yield return new XPOTestProviderWithSecurity();
            yield return new XPOPerfTestProvider();
        }

        [IterationSetup(Target = nameof(InsertEmptyContact)
            + "," + nameof(InsertContact)
        )]
        public void IterationSetupForInsert() {
            TestProvider.InitSession();
            TestProvider.CleanupTestDataSet();
            TestProvider.TearDownSession();
            TestProvider.InitSession();
        }

        [IterationSetup(Target = nameof(GetContacts)
            + "," + nameof(GetTasks)
            + "," + nameof(UpdateContacts)
            + "," + nameof(UpdateTasks)
        )]
        public void IterationSetupForUpdateAndSelect() {
            TestProvider.InitSession();
        }


        [GlobalSetup(Target = nameof(GetContacts)
            + "," + nameof(GetTasks)
            + "," + nameof(UpdateContacts)
            + "," + nameof(UpdateTasks)
        )]
        public void GlobalSetupForUpdateAndSelect() {
            TestProvider.GlobalTestDataSetup(true);
        }

        [GlobalSetup]
        public void GlobalTestDataSetup() {
            TestProvider.GlobalTestDataSetup(false);
        }

        [IterationCleanup]
        public void IterationCleanup() {
            TestProvider.TearDownSession();
        }

        [Benchmark]
        public void InsertEmptyContact() {
            TestProvider.InsertEmptyContact(ItemsForTestIteration);
        }
        [Benchmark]
        public void InsertContact() {
            TestProvider.InsertContact(ItemsForTestIteration);
        }
        [Benchmark]
        public void UpdateContacts() {
            TestProvider.UpdateContacts(ItemsForTestIteration);
        }
        [Benchmark]
        public void UpdateTasks() {
            TestProvider.UpdateTasks(ItemsForTestIteration);
        }
        [Benchmark]
        public void GetContacts() {
            TestProvider.GetContacts(ItemsForTestIteration);
        }
        [Benchmark]
        public void GetTasks() {
            TestProvider.GetTasks(ItemsForTestIteration);
        }
    }
}
