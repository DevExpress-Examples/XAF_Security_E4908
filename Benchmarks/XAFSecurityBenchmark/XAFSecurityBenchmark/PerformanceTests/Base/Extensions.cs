using DevExpress.ExpressApp;
using DevExpress.Xpo;

namespace XAFSecurityBenchmark.PerformanceTests {
    public static class Extensions {
        public static void DeleteAllObjects<T>(this UnitOfWork uow) {
            foreach(var item in uow.Query<T>()) {
                uow.Delete(item);
            }
            uow.CommitChanges();
        }
        public static void RemoveAllObjects<T>(this IObjectSpace updatingObjectSpace) => updatingObjectSpace.Delete(updatingObjectSpace.GetObjects<T>());
    }
}
