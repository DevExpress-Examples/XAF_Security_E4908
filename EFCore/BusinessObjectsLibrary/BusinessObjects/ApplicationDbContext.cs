using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using Microsoft.EntityFrameworkCore;

namespace BusinessObjectsLibrary.EFCore.NetCore.BusinessObjects {
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<PermissionPolicyRole> PermissionPolicyRoles { get; set; }
        public DbSet<PermissionPolicyUser> PermissionPolicyUsers { get; set; }
    }
}
