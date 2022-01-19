using DevExpress.ExpressApp.Blazor.Services;

namespace Blazor.ServerSide.Services {
    internal class ValueManagerContextMiddleware {
        private readonly RequestDelegate next;
        public ValueManagerContextMiddleware(RequestDelegate next) {
            this.next = next;
        }
        public async Task Invoke(HttpContext context, IValueManagerStorageContainerInitializer storageContainerAccessor) {
            storageContainerAccessor.Initialize();
            await next(context);
        }
    }
}
