using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;

namespace WebAPI.BusinessObjects;

[VisibleInReports]
public class Post : BaseObject {
    public virtual string Title { get; set; }
    public virtual string Content { get; set; }
    public virtual ApplicationUser Author { get; set; }
    public override void OnCreated() {
        base.OnCreated();
        var user = ObjectSpace.GetObjectByKey<ApplicationUser>(
            ObjectSpace.ServiceProvider.GetRequiredService<ISecurityStrategyBase>().UserId);
        SetPropertyValueWithSecurityBypass(nameof(Author), user);
    }
}