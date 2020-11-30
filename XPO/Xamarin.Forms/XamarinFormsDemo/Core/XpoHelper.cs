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
        public static SecuredObjectSpaceProvider objectSpaceProvider;
        public static AuthenticationStandard authentication; 
        public static SecurityStrategyComplex security;
        
        public static void InitXpo(string connectionString, string login, string password) {
            RegisterEntities();
            InitSecurity();
            objectSpaceProvider = new SecuredObjectSpaceProvider(security, new WebApiDataStoreProvider(connectionString));
            LogIn(login, password);
            XpoDefault.Session = null;
        }
        public static UnitOfWork CreateUnitOfWork() {
            var space = objectSpaceProvider.CreateObjectSpace() as XPObjectSpace;
            return space.Session as UnitOfWork;
        }
        static void LogIn(string login, string password) {
            authentication.SetLogonParameters(new AuthenticationStandardLogonParameters(login, password));
            IObjectSpace loginObjectSpace = objectSpaceProvider.CreateObjectSpace();
            security.Logon(loginObjectSpace);
        }
        
        static void InitSecurity() {
            authentication = new AuthenticationStandard();
            security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();
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
                throw new NotImplementedException();
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
