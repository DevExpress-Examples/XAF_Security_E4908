using System;
using System.Linq.Expressions;
using DevExpress.ExpressApp;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    abstract class SecuredObjectHelperBase {
        IObjectSpace objectSpace;
        public SecuredObjectHelperBase(IObjectSpace objectSpace) {
            this.objectSpace = objectSpace;
        }

        public void BeginTransaction() { }
        public virtual void EndTransaction() { }

        public void SaveChanges() {
            objectSpace.CommitChanges();
        }
        
        public IObjectSpace ObjectSpace => objectSpace;

        public T CreateObject<T>() => objectSpace.CreateObject<T>();
        public ObjectType FirstOrDefault<ObjectType>(Expression<Func<ObjectType, bool>> criteriaExpression) where ObjectType : class {
            return objectSpace.FirstOrDefault<ObjectType>(criteriaExpression);
        }
    }
}
