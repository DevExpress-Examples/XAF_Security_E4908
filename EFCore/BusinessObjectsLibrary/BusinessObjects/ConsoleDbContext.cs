using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects {
    public class ConsoleDbContext : DbContext {
        public ConsoleDbContext(DbContextOptions<ConsoleDbContext> options) : base(options) {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
        public DbSet<PermissionPolicyUser> PermissionPolicyUsers { get; set; }
    }
}
