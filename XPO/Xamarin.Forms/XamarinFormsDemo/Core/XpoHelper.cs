using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using XafSolution.Module.BusinessObjects;

namespace XamarinFormsDemo {
    public static class XpoHelper {
        static SecuredObjectSpaceProvider ObjectSpaceProvider;
        static AuthenticationStandard Authentication; 
        public static SecurityStrategyComplex Security;
        
        public static void InitXpo(string connectionString, string login, string password) {
            RegisterEntities();
            InitSecurity();
            XpoDefault.RegisterBonusProviders();
            DataStoreBase.RegisterDataStoreProvider(WebApiDataStoreClient.XpoProviderTypeString, CreateWebApiDataStoreFromString);
            ObjectSpaceProvider = new SecuredObjectSpaceProvider(Security, connectionString, null);
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




        static IDataStore CreateWebApiDataStoreFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            ConnectionStringParser parser = new ConnectionStringParser(connectionString);
            if(!parser.PartExists("uri"))
                throw new ArgumentException("Connection string does not contain the 'uri' part.");
            string uri = parser.GetPartByName("uri");
#if DEBUG
            HttpClient client = new HttpClient(GetInsecureHandler());
#else
            HttpClient client = new HttpClient();
#endif
            client.BaseAddress = new Uri(uri);
            objectsToDisposeOnDisconnect = new IDisposable[] { client };
            return new WebApiDataStoreClient(client, autoCreateOption);
        }
        static HttpClientHandler GetInsecureHandler() {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return handler;
        }


    }
}
