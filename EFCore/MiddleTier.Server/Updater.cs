using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace MiddleTier.Server;

// For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.Updating.ModuleUpdater
public class Updater : ModuleUpdater {
    public Updater(IObjectSpace objectSpace, Version currentDBVersion) :
        base(objectSpace, currentDBVersion) {
    }
    public override void UpdateDatabaseAfterUpdateSchema() {
        base.UpdateDatabaseAfterUpdateSchema();
        var sharedUpdater = new DatabaseUpdater.Updater(ObjectSpace);
        sharedUpdater.UpdateDatabase();
    }
}
