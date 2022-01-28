using Blazor.ServerSide.Helpers;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.WebApi.Services;

namespace Blazor.ServerSide.Services
{
    internal class TypesInfoProvider : ITypesInfoProvider2
    {
        readonly SecurityProvider securityProvider;
        public TypesInfoProvider(SecurityProvider securityProvider)
        {
            this.securityProvider = securityProvider;
        }
        public ITypesInfo GetTypesInfo()
        {
            return securityProvider.ObjectSpaceProvider.TypesInfo;
        }
    }
}
