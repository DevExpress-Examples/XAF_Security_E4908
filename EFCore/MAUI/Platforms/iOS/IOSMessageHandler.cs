namespace MAUI.Services {
	public static partial class HttpMessageHandler {
		static HttpMessageHandler() {
			NSUrlSessionHandler nSUrlSessionHandler = new();
			nSUrlSessionHandler.ServerCertificateCustomValidationCallback += (_, cert, _, errors) 
				=> cert is { Issuer: "CN=localhost" } || errors == System.Net.Security.SslPolicyErrors.None;
			nSUrlSessionHandler.TrustOverrideForUrl = (sender, url, trust) => {
				return true;
			};

			PlatformHttpMessageHandler = nSUrlSessionHandler;
			
		}
	}
} 
