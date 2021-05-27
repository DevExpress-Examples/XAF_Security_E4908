using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.XPO;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    class XpoSecuredObjectHelper : SecuredObjectHelperBase, ITransactionHelper {
        public XpoSecuredObjectHelper(IObjectSpace objectSpace) : base(objectSpace) { }

        public IContact CreateContact() => CreateObject<Contact>();
        public IDemoTask CreateTask() => CreateObject<DemoTask>();

        public void RemoveAllTestData() => new XpoObjectHelper((UnitOfWork)((XPObjectSpace)ObjectSpace).Session).RemoveAllTestData();
        public void UpdateQueryOptimizationStatistics() => new XpoObjectHelper((UnitOfWork)((XPObjectSpace)ObjectSpace).Session).UpdateQueryOptimizationStatistics();

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

        public ICustomPermissionPolicyUser GetSecurityUser(string userName) => FirstOrDefault<CustomPermissionPolicyUser>(user => user.UserName == userName);
    }
}
