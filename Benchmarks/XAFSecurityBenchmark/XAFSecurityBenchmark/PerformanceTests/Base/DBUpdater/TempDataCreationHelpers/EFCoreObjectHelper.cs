using System;
using System.Linq;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.EFCore;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    class EFCoreObjectHelper : ITransactionHelper {
        Func<EFCoreContext> dataContextCreator;
        EFCoreContext dataContext;
        public EFCoreObjectHelper() { }
        public EFCoreObjectHelper(Func<EFCoreContext> dataContextCreator) {
            this.dataContextCreator = dataContextCreator;
        }
        public void Dispose() {        }
        public void BeginTransaction() {
            dataContext = dataContextCreator();
        }
        public void SaveChanges() => dataContext.SaveChanges();
        public void EndTransaction() {
            dataContext.Dispose();
            dataContext = null;
        }

        public IContact CreateContact() {
            var contact = new Contact();
            dataContext.Contacts.Add(contact);
            return contact;
        }
        public IDemoTask CreateTask() {
            var task = new DemoTask();
            dataContext.Tasks.Add(task);
            return task;
        }
        public IPhoneNumber CreatePhoneNumber(IContact forContact) {
            var phoneNumber = new PhoneNumber();
            phoneNumber.Party = (Party)forContact;
            dataContext.PhoneNumbers.Add(phoneNumber);
            return phoneNumber;
        }
        public IAddress CreateAddress() {
            var address = new Address();
            dataContext.Addresses.Add(address);
            return address;
        }
        public ICountry CreateCountry(IAddress forAddress) {
            var country = new Country();
            forAddress.Country = country;
            dataContext.Countries.Add(country);
            return country;
        }
        public IPosition CreatePosition() {
            var position = new Position();
            dataContext.Positions.Add(position);
            return position;
        }

        public void RemoveAllTestData() => RemoveAllTestData(dataContext);
        public void RemoveAllTestData(DbContext dataContext) {
            //dataContext.Database.ExecuteSqlRaw($"DELETE FROM [{GetTableName<PhoneNumber>()}]");
            //dataContext.Database.ExecuteSqlRaw($"DELETE FROM [{GetTableName<DemoTask>()}]");
            //dataContext.Database.ExecuteSqlRaw($"DELETE FROM [{GetTableName<Contact>()}]");
            //dataContext.Database.ExecuteSqlRaw($"DELETE FROM [{GetTableName<Position>()}]");
            //dataContext.Database.ExecuteSqlRaw($"DELETE FROM [{GetTableName<Address>()}]");
            //dataContext.Database.ExecuteSqlRaw($"DELETE FROM [{GetTableName<Country>()}]");


            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<PhoneNumber>()}] DROP CONSTRAINT FK_PhoneNumbers_Party_PartyID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<DemoTask>()}] DROP CONSTRAINT FK_Task_Party_AssignedToID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] DROP CONSTRAINT FK_Party_Addresses_Address1ID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] DROP CONSTRAINT FK_Party_Addresses_Address2ID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] DROP CONSTRAINT FK_Party_Departments_DepartmentID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] DROP CONSTRAINT FK_Party_Party_ManagerID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] DROP CONSTRAINT FK_Party_Positions_PositionID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Address>()}] DROP CONSTRAINT FK_Addresses_Countries_CountryID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Resume>()}] DROP CONSTRAINT FK_Resumes_FileData_FileID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Resume>()}] DROP CONSTRAINT FK_Resumes_Party_ContactID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Department>()}] DROP CONSTRAINT FK_Departments_Party_DepartmentHeadID");
            ExecuteSqlRaw($"ALTER TABLE [DepartmentPosition] DROP CONSTRAINT FK_DepartmentPosition_Positions_PositionsID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<PortfolioFileData>()}] DROP CONSTRAINT FK_PortfolioFileData_FileData_FileID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<PortfolioFileData>()}] DROP CONSTRAINT FK_PortfolioFileData_Resumes_ResumeForeignKey");
            ExecuteSqlRaw($"ALTER TABLE [ContactDemoTask] DROP CONSTRAINT FK_ContactDemoTask_Party_ContactsID");
            ExecuteSqlRaw($"ALTER TABLE [ContactDemoTask] DROP CONSTRAINT FK_ContactDemoTask_Task_TasksID");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Location>()}] DROP CONSTRAINT FK_Location_Party_ContactRef");

            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<Contact>()}]");
            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<PhoneNumber>()}]");
            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<Resume>()}]");
            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<DemoTask>()}]");
            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<Position>()}]");
            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<Address>()}]");
            ExecuteSqlRaw($"TRUNCATE TABLE [{GetTableName<Country>()}]");

            ExecuteSqlRaw($"TRUNCATE TABLE [DepartmentPosition]");
            ExecuteSqlRaw($"TRUNCATE TABLE [ContactDemoTask]");

            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<PhoneNumber>()}] ADD CONSTRAINT FK_PhoneNumbers_Party_PartyID FOREIGN KEY(PartyID) REFERENCES {GetTableName<Contact>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<DemoTask>()}] ADD CONSTRAINT FK_Task_Party_AssignedToID FOREIGN KEY(AssignedToID) REFERENCES {GetTableName<Contact>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] ADD CONSTRAINT FK_Party_Addresses_Address1ID FOREIGN KEY(Address1ID) REFERENCES {GetTableName<Address>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] ADD CONSTRAINT FK_Party_Addresses_Address2ID FOREIGN KEY(Address2ID) REFERENCES {GetTableName<Address>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] ADD CONSTRAINT FK_Party_Departments_DepartmentID FOREIGN KEY(DepartmentID) REFERENCES {GetTableName<Department>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] ADD CONSTRAINT FK_Party_Party_ManagerID FOREIGN KEY(ManagerID) REFERENCES {GetTableName<Contact>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Contact>()}] ADD CONSTRAINT FK_Party_Positions_PositionID FOREIGN KEY(PositionID) REFERENCES {GetTableName<Position>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Address>()}] ADD CONSTRAINT FK_Addresses_Countries_CountryID FOREIGN KEY(CountryID) REFERENCES {GetTableName<Country>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Resume>()}] ADD CONSTRAINT FK_Resumes_FileData_FileID FOREIGN KEY(FileID) REFERENCES {GetTableName<DevExpress.Persistent.BaseImpl.EF.FileData>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Resume>()}] ADD CONSTRAINT FK_Resumes_Party_ContactID FOREIGN KEY(ContactID) REFERENCES {GetTableName<Contact>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Department>()}] ADD CONSTRAINT FK_Departments_Party_DepartmentHeadID FOREIGN KEY(DepartmentHeadID) REFERENCES {GetTableName<Department>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [DepartmentPosition] ADD CONSTRAINT FK_DepartmentPosition_Positions_PositionsID FOREIGN KEY(PositionsID) REFERENCES Positions(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<PortfolioFileData>()}] ADD CONSTRAINT FK_PortfolioFileData_FileData_FileID FOREIGN KEY(FileID) REFERENCES {GetTableName<PortfolioFileData>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<PortfolioFileData>()}] ADD CONSTRAINT FK_PortfolioFileData_Resumes_ResumeForeignKey FOREIGN KEY(ResumeForeignKey) REFERENCES {GetTableName<Resume>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [ContactDemoTask] ADD CONSTRAINT FK_ContactDemoTask_Party_ContactsID FOREIGN KEY(ContactsID) REFERENCES {GetTableName<Contact>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [ContactDemoTask] ADD CONSTRAINT FK_ContactDemoTask_Task_TasksID FOREIGN KEY(TasksID) REFERENCES {GetTableName<DemoTask>()}(ID)");
            ExecuteSqlRaw($"ALTER TABLE [{GetTableName<Location>()}] ADD CONSTRAINT FK_Location_Party_ContactRef FOREIGN KEY(ContactRef) REFERENCES {GetTableName<Contact>()}(ID)");

            string GetTableName<T>() => dataContext.Model.FindEntityType(typeof(T)).GetTableName();
            void ExecuteSqlRaw(string sql) {
                //try {
                    dataContext.Database.ExecuteSqlRaw(sql);
                //}
                //catch { }
            }
        }

        public void UpdateQueryOptimizationStatistics() => UpdateQueryOptimizationStatistics(dataContext);
        public void UpdateQueryOptimizationStatistics(DbContext dataContext) {
            dataContext.Database.ExecuteSqlRaw("UPDATE STATISTICS[ContactDemoTask]");
            dataContext.Database.ExecuteSqlRaw($"UPDATE STATISTICS[{GetTableName<Party>()}]");
            dataContext.Database.ExecuteSqlRaw($"UPDATE STATISTICS[{GetTableName<Task>()}]");
            dataContext.Database.ExecuteSqlRaw($"UPDATE STATISTICS[{GetTableName<Department>()}]");
            dataContext.Database.ExecuteSqlRaw($"UPDATE STATISTICS[{GetTableName<Position>()}]");
            dataContext.Database.ExecuteSqlRaw($"UPDATE STATISTICS[{GetTableName<PermissionPolicyUser>()}]");
            string GetTableName<T>() => dataContext.Model.FindEntityType(typeof(T)).GetTableName();
        }
        public ICustomPermissionPolicyUser GetSecurityUser(string userName) => dataContext.Users.Where(user => user.UserName == userName).Include(user => user.Department).FirstOrDefault();
    }
}
