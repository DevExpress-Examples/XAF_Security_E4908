using DevExpress.ExpressApp.Xpo;

public class XpoDataStoreProviderService {
	//TODO Add thread save logic
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


