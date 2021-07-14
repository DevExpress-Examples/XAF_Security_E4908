using DevExpress.ExpressApp.Security;
using DevExpress.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo.DB;
using System;
using DevExpress.Xpo.DB.Helpers;
using System.Net.Http;
using DevExpress.ExpressApp.Security.ClientServer;
using BusinessObjectsLibrary;

namespace XamarinFormsDemo.Services {
    public static class XpoHelper {
        static readonly SecurityStrategyComplex fSecurity;
        public static SecurityStrategyComplex Security {
            get {
                if(fSecurity == null) {
                    throw new InvalidOperationException("The security system is not initialized. Call the InitSecuritySystem method first.");
                }
                return fSecurity;
            }
        }
        public static UnitOfWork CreateUnitOfWork() {
            var space = (XPObjectSpace)GetObjectSpaceProvider().CreateObjectSpace();
            return (UnitOfWork)space.Session;
        }

        static XpoHelper() {
            RegisterEntities();
            fSecurity = InitSecuritySystem();
            fSecurity.RegisterXPOAdapterProviders(); 
#if DEBUG
            ConfigureXpoForDevEnvironment();
#endif
        }
        static void RegisterEntities() {
            XpoTypesInfoHelper.GetXpoTypeInfoSource();
            XafTypesInfo.Instance.RegisterEntity(typeof(Employee));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyUser));
            XafTypesInfo.Instance.RegisterEntity(typeof(PermissionPolicyRole));
        }
        static SecurityStrategyComplex InitSecuritySystem() {
            AuthenticationStandard authentication = new AuthenticationStandard();
            return new SecurityStrategyComplex(
                typeof(PermissionPolicyUser),
                typeof(PermissionPolicyRole),
                authentication);
        }
        static void ConfigureXpoForDevEnvironment() {
            XpoDefault.RegisterBonusProviders();
            DataStoreBase.RegisterDataStoreProvider(WebApiDataStoreClient.XpoProviderTypeString, CreateWebApiDataStoreFromString);
        }
        static IDataStore CreateWebApiDataStoreFromString(string connectionString, AutoCreateOption autoCreateOption, out IDisposable[] objectsToDisposeOnDisconnect) {
            ConnectionStringParser parser = new ConnectionStringParser(connectionString);
            if(!parser.PartExists("uri"))
                throw new ArgumentException("The connection string does not contain the 'uri' part.");
            string uri = parser.GetPartByName("uri");
            HttpClient client = new HttpClient(GetInsecureHandler());
            client.BaseAddress = new Uri(uri);
            objectsToDisposeOnDisconnect = new IDisposable[] { client };
            return new WebApiDataStoreClient(client, autoCreateOption);
        }
        /// <summary>
        /// Disables an SSL sertificate validation to support self-signed developer certificates.
        /// </summary>
        /// <returns></returns>
        static HttpClientHandler GetInsecureHandler() {
            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            return handler;
        }
        const string ConnectionString = @"XpoProvider=WebApi;uri=https://10.0.2.2:5001/xpo/";
        static IObjectSpaceProvider ObjectSpaceProvider;
        static IObjectSpaceProvider GetObjectSpaceProvider() {
            if(ObjectSpaceProvider == null) {
                ObjectSpaceProvider = new SecuredObjectSpaceProvider(Security, ConnectionString, null);
            }
            return ObjectSpaceProvider;
        }
        public static void Logon(string userName, string password) {
            var logonParameters = new AuthenticationStandardLogonParameters(userName, password);
            Security.Authentication.SetLogonParameters(logonParameters);
            IObjectSpace logonObjectSpace = GetObjectSpaceProvider().CreateObjectSpace();
            Security.Logon(logonObjectSpace);
        }
        public static void Logoff() {
            Security.Logoff();
            ObjectSpaceProvider = null;
        }
    }
}