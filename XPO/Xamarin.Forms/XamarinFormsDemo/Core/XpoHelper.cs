using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Linq;
using System.Net.Http;
using XafSolution.Module.BusinessObjects;

namespace XamarinFormsDemo {
    public static class XpoHelper {
        public static SecuredObjectSpaceProvider objectSpaceProvider;
        public static AuthenticationStandard authentication; 
        public static SecurityStrategyComplex security;
        static IDataStore dataStore;
        
        public static void InitXpo(string connectionString, string login, string password) {
            GetDataStore(connectionString);
            RegisterEntities();
            GetSecurity();
            objectSpaceProvider = new SecuredObjectSpaceProvider(security, new WebApiDataStoreProvider());
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
        static HttpClientHandler GetInsecureHandler() {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return handler;
        }
        static void GetSecurity() {
            authentication = new AuthenticationStandard();
            security = new SecurityStrategyComplex(typeof(PermissionPolicyUser), typeof(PermissionPolicyRole), authentication);
            security.RegisterXPOAdapterProviders();
        }
        static void GetDataStore(string connectionString) {
            HttpClient httpClient = new HttpClient(GetInsecureHandler());
            Uri uri = new Uri(connectionString);
            httpClient.BaseAddress = uri;
            dataStore =  new WebApiDataStoreClient(httpClient, AutoCreateOption.SchemaAlreadyExists);   
        }
        private static void RegisterEntities() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }

        private class WebApiDataStoreProvider : IXpoDataStoreProvider {
            public string ConnectionString {
                get {
                    throw new NotImplementedException();
                }
            }

            public IDataStore CreateSchemaCheckingStore(out IDisposable[] disposableObjects) {
                disposableObjects =null; 
                return dataStore;
            }

            public IDataStore CreateUpdatingStore(bool allowUpdateSchema, out IDisposable[] disposableObjects) {
                disposableObjects = null;
                return dataStore;
            }

            public IDataStore CreateWorkingStore(out IDisposable[] disposableObjects) {
                disposableObjects = null;
                return dataStore;
            }
        }
    }
}
