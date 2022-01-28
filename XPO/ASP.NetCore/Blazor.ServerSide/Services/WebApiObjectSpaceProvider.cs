using Blazor.ServerSide.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.WebApi.Services;

namespace Blazor.ServerSide.Services
{
    internal class WebApiObjectSpaceProvider : IWebApiObjectSpaceProvider
    {
        readonly SecurityProvider securityProvider;
        public WebApiObjectSpaceProvider(SecurityProvider securityProvider)
        {
            this.securityProvider = securityProvider;
        }

        public IObjectSpace CreateObjectSpace(Type objectType)
        {
            return securityProvider.ObjectSpaceProvider.CreateObjectSpace();
        }

        public IObjectSpaceProvider GetObjectSpaceProvider(Type objectType)
        {
            return securityProvider.ObjectSpaceProvider;
        }
    }
}
