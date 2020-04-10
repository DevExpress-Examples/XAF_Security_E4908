using DevExpress.ExpressApp;
using DevExpress.ExpressApp.EFCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WindowsFormsApplication {
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
