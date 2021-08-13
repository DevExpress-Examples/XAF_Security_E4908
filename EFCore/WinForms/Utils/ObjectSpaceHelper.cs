using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;

namespace WindowsFormsApplication.Utils {
    public static class ObjectSpaceHelper {
        public static object GetBindingList<TEntity>(this IObjectSpace objectSpace) where TEntity : class {
            BindingList<TEntity> bindingSource = null;
            if (objectSpace is EFCoreObjectSpace efCoreObjectSpace) {
                efCoreObjectSpace.DbContext.Set<TEntity>().Load();
                bindingSource = efCoreObjectSpace.DbContext.Set<TEntity>().Local.ToBindingList();
            }
            return bindingSource;
        }
    }
}
