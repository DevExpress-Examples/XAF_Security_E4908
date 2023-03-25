using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;

namespace WebAPI.BusinessObjects;

[VisibleInReports][DomainComponent][DefaultClassOptions]
public class Post:IXafEntityObject,IObjectSpaceLink {
	[DevExpress.ExpressApp.Data.Key]
	public virtual int PostId { get; set; }
	public virtual string Title { get; set; }
	public virtual string Content { get; set; }
	
	
	public virtual ApplicationUser Author { get; set; }

	IObjectSpace IObjectSpaceLink.ObjectSpace { get; set; }

	void IXafEntityObject.OnCreated() {
		var objectSpace = ((IObjectSpaceLink)this).ObjectSpace;
		if (objectSpace.IsNewObject(this)) {
			Author = objectSpace.FindObject<ApplicationUser>(CriteriaOperator.Parse("ID=CurrentUserId()"));
		}
	}

	void IXafEntityObject.OnSaving() { }

	void IXafEntityObject.OnLoaded() { }
}