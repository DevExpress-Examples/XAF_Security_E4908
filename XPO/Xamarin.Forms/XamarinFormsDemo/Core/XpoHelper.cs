using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.Net.Http;
using XafSolution.Module.BusinessObjects;

namespace XamarinFormsDemo {
    public static class XpoHelper {
        static SecuredObjectSpaceProvider ObjectSpaceProvider;
        static AuthenticationStandard Authentication; 
        public static SecurityStrategyComplex Security;
        
        public static void InitXpo(string connectionString, string login, string password) {
            RegisterEntities();
            InitSecurity();
            ObjectSpaceProvider = new SecuredObjectSpaceProvider(Security, new WebApiDataStoreProvider(connectionString));
            UpdateDataBase();
            LogIn(login, password);
            XpoDefault.Session = null;
        }

        static void UpdateDataBase() {
            var space = ObjectSpaceProvider.CreateUpdatingObjectSpace(true);
            Updater updater = new Updater(space);
            updater.UpdateDatabase();
        }

        public static UnitOfWork CreateUnitOfWork() {
            var space = (XPObjectSpace)ObjectSpaceProvider.CreateObjectSpace();
            return (UnitOfWork)space.Session;
        }
        static void LogIn(string login, string password) {
            Authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(login, password));
            IObjectSpace loginObjectSpace = ObjectSpaceProvider.CreateObjectSpace();
            Security.Logon(loginObjectSpace);
        }
        
        static void InitSecurity() {
            Authentication = new AuthenticationStandard();
            Security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), Authentication);
            Security.RegisterXPOAdapterProviders();
        }
        private static void RegisterEntities() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }

        private class WebApiDataStoreProvider : IXpoDataStoreProvider {
            string fConnectionString;
            public string ConnectionString {
                get => fConnectionString;
            }
            HttpClientHandler GetInsecureHandler() {
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                return handler;
            }
            public WebApiDataStoreProvider(string connectionString) {
                fConnectionString = connectionString;
            }

            public IDataStore CreateSchemaCheckingStore(out IDisposable[] disposableObjects) {
                throw new NotImplementedException();
            }

            public IDataStore CreateUpdatingStore(bool allowUpdateSchema, out IDisposable[] disposableObjects) {
                HttpClient httpClient = new HttpClient(GetInsecureHandler());
                Uri uri = new Uri(ConnectionString);
                httpClient.BaseAddress = uri;
                disposableObjects = new[] { httpClient };
                return new WebApiDataStoreClient(httpClient, AutoCreateOption.DatabaseAndSchema);
            }

            public IDataStore CreateWorkingStore(out IDisposable[] disposableObjects) {
                HttpClient httpClient = new HttpClient(GetInsecureHandler());
                Uri uri = new Uri(ConnectionString);
                httpClient.BaseAddress = uri;
                disposableObjects = new[] { httpClient };
                return new WebApiDataStoreClient(httpClient, AutoCreateOption.SchemaAlreadyExists);
            }
        }
    }
}
