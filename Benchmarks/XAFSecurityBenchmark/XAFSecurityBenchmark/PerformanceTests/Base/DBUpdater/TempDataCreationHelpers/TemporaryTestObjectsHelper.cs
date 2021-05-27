using System;
using System.Collections.Generic;
using System.Diagnostics;
using DevExpress.Persistent.Base.General;
using XAFSecurityBenchmark.Models.Base;
using XAFSecurityBenchmark.Models.Base.Enums;

namespace XAFSecurityBenchmark.PerformanceTests.Base.DBUpdater {
    static class TemporaryTestObjectsHelper {
        public const string Contact_FirstNamePrefix = "Contact_FirstName";
        public const string Contact_LastNamePrefix = "Contact_LastName";
        public const string Contact_EmailPrefix = "Test.EmailAddress";

        public static void CreateTestDataSet(ITransactionHelper objectHelper) {
            objectHelper.BeginTransaction();
            foreach(string userName in TestSetConfig.Users) {
                var department = objectHelper.GetSecurityUser(userName).Department;
                for(int i = 0; i < TestSetConfig.ContactCountPerUserToCreate; i++) {
                    var contact = objectHelper.CreateContact();
                    objectHelper.FillContact(contact, department, i);
                    //Tasks are assigned to contact
                    contact.AddTasks(CreateTasks(objectHelper, null, TestSetConfig.TasksAssigedToContact));
                    //Tasks are only linked to contact
                    _ = CreateTasks(objectHelper, contact, TestSetConfig.TasksLinkedToContact);
                }
                objectHelper.SaveChanges();
            }
            objectHelper.SaveChanges();
            objectHelper.UpdateQueryOptimizationStatistics();
            objectHelper.EndTransaction();
        }
        public static void FillContact(this IObjectHelper objectHelper, IContact contact, int index) {
            objectHelper.FillContact(contact, null, index);
        }
        public static void FillContact(this IObjectHelper objectHelper, IContact contact, IDepartment department, int index) {
            contact.Email = $"{Contact_EmailPrefix}{index}.com";
            contact.FirstName = $"{Contact_FirstNamePrefix}{index}";
            contact.LastName = $"{Contact_LastNamePrefix}{index}";
            contact.Birthday = RandomBirthday(new Random());

            contact.SetDepartment(department);
            //contact.Photo = a picture from somewhere could be here;

            contact.TitleOfCourtesy = TitleOfCourtesy.Mr;

            _ = GetPhoneNumber(contact, objectHelper);
            _ = GetAddress(contact, objectHelper);
            _ = GetPosition(contact, objectHelper);
        }
        static IPhoneNumber GetPhoneNumber(IContact contact, IObjectHelper objectHelper) {
            IPhoneNumber phoneNumber = objectHelper.CreatePhoneNumber(contact);
            phoneNumber.Number = "+0 000 111 222 333";
            phoneNumber.PhoneType = "Work";
            return phoneNumber;
        }
        static IAddress GetAddress(IContact contact, IObjectHelper objectHelper) {
            IAddress address = objectHelper.CreateAddress();
            address.ZipPostal = "000111222333";
            address.Street = "Maksima Gorkogo";
            address.City = "Tula";
            address.StateProvince = "SomeProvince";
            contact.SetAddress1(address);
            string countryName = "YourCountryNameHere";
            ICountry country = objectHelper.CreateCountry(address);
            country.Name = countryName;
            return address;
        }
        static IPosition GetPosition(IContact contact, IObjectHelper objectHelper) {
            string positionTitle = "a pretty good job for a really good guy";
            IPosition position = objectHelper.CreatePosition();
            position.Title = positionTitle;
            if(contact.Department != null) {
                position.AddDepartment(contact.Department);
                contact.Department.AddPositions(position);
            }
            contact.SetPosition(position);
            return position;
        }
        static IList<ITask> CreateTasks(IObjectHelper testDataSetHelper, IContact assignedTo, int recordsCount) {
            Random rndGenerator = new Random();
            IList<ITask> taskList = new List<ITask>();
            for(int i = 0; i < recordsCount; i++) {
                var task = testDataSetHelper.CreateTask();
                task.Subject = $"Task №{i}";
                task.Description = $"Task for the {assignedTo?.FirstName} № {i}";
                int rndStatus = rndGenerator.Next(0, 5);
                task.Status = (TaskStatus)rndStatus;
                task.DueDate = DateTime.Now.AddHours((90 - rndStatus * 9) + 24).Date;
                task.EstimatedWorkHours = rndGenerator.Next(10, 20);
                if(task.Status == TaskStatus.WaitingForSomeoneElse ||
                   task.Status == TaskStatus.Completed ||
                   task.Status == TaskStatus.InProgress) {
                    task.StartDate = DateTime.Now.AddHours(-rndGenerator.Next(720)).Date;
                    task.ActualWorkHours = rndGenerator.Next(task.EstimatedWorkHours - 10, task.EstimatedWorkHours + 10);
                }
                task.DueDate = DateTime.Now.AddHours((90 - rndStatus * 9) + 24).Date;
                task.SetAssignedTo(assignedTo);
                task.Priority = (Priority)rndGenerator.Next(3);
                taskList.Add(task);
            }
            return taskList;
        }

        static DateTime RandomBirthday(Random random) {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(random.Next(range));
        }
    }
}
