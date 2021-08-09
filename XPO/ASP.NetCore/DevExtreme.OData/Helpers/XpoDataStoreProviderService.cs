using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ASPNETCoreODataService {
	public class XpoDataStoreProviderService {
		private IXpoDataStoreProvider dataStoreProvider;
		private string connectionString;
		public XpoDataStoreProviderService(IConfiguration config) {
			connectionString = config.GetConnectionString("ConnectionString");
		}
		public IXpoDataStoreProvider GetDataStoreProvider() {
			if(dataStoreProvider == null) {
				dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
			}
			return dataStoreProvider;
		}
	}
}
