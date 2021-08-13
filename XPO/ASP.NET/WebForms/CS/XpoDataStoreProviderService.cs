using System.Configuration;
using System.Data;
using DevExpress.ExpressApp.Xpo;

namespace WebFormsApplication {
	public static class XpoDataStoreProviderService {
		private static IXpoDataStoreProvider dataStoreProvider;
		public static IXpoDataStoreProvider GetDataStoreProvider(IDbConnection connection, bool enablePoolingInConnectionString) {
			if(dataStoreProvider == null) {
				string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
				dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, enablePoolingInConnectionString);
			}
			return dataStoreProvider;
		}
	}
}