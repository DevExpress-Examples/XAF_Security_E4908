using DevExpress.ExpressApp.Security;
using System.Security.Principal;

namespace Blazor.ServerSide.Services {
    public class HttpContextPrincipalProvider : IPrincipalProvider {
        readonly IHttpContextAccessor httpContextAccessor;
        public HttpContextPrincipalProvider(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }

        public IPrincipal User => httpContextAccessor.HttpContext?.User;
    }
}
