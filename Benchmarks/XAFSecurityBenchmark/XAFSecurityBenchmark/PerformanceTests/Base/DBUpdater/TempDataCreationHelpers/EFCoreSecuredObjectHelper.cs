using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.EFCore;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    class EFCoreSecuredObjectHelper : SecuredObjectHelperBase, ITransactionHelper {
        public EFCoreSecuredObjectHelper(IObjectSpace objectSpace) : base(objectSpace) { }

        private DbContext DbContext => ((EFCoreObjectSpace)ObjectSpace).DbContext;

        public IContact CreateContact() => CreateObject<Contact>();
        public IDemoTask CreateTask() => CreateObject<DemoTask>();
        public IPhoneNumber CreatePhoneNumber(IContact forContact) {
            var phoneNumber = CreateObject<PhoneNumber>();
            phoneNumber.Party = (Party)forContact;
            return phoneNumber;
        }
        public IAddress CreateAddress() => CreateObject<Address>();
        public ICountry CreateCountry(IAddress forAddress) {
            var country = CreateObject<Country>();
            forAddress.Country = country;
            return country;
        }
        public IPosition CreatePosition() => CreateObject<Position>();

        public void RemoveAllTestData() => new EFCoreObjectHelper().RemoveAllTestData(DbContext);
        public void UpdateQueryOptimizationStatistics() => new EFCoreObjectHelper().UpdateQueryOptimizationStatistics(DbContext);

        public ICustomPermissionPolicyUser GetSecurityUser(string userName) => FirstOrDefault<CustomPermissionPolicyUser>(user => user.UserName == userName);
    }
}
