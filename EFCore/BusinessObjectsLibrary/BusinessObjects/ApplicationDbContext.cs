using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjectsLibrary.EFCore.BusinessObjects {
    public class ApplicationDbContext : DbContext {
        public const string DatabaseConnectionFailedMessage = "Make sure the database has been created.\r\nTo create the database, run the 'DatabaseUpdater' project.";
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
        public DbSet<PermissionPolicyUser> PermissionPolicyUsers { get; set; }
    }
}
