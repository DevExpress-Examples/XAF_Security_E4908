using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using XAFSecurityBenchmark.PerformanceTests;

namespace XAFSecurityBenchmark.Models.EFCore {
    public class EFCoreContext : DbContext {
        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DemoTask> Tasks { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<PhoneNumber> PhoneNumbers { get; set; }

        public DbSet<PermissionPolicyRole> Roles { get; set; }
        public DbSet<CustomPermissionPolicyUser> Users { get; set; }

        public EFCoreContext() { }
        public EFCoreContext(DbContextOptions<EFCoreContext> options)
            : base(options) {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
            if(!optionsBuilder.IsConfigured) {
                optionsBuilder.UseSqlServer(TestSetConfig.EFCoreConnectionStrings);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Party>().HasMany(c => c.PhoneNumbers).WithOne(p => p.Party).IsRequired();


            modelBuilder.Entity<Contact>()
                .HasOne(r => r.Location)
                .WithOne(p => p.Contact)
                .HasForeignKey<Location>(fk => fk.ContactRef);

            modelBuilder.Entity<Department>()
                .HasMany(p => p.Contacts)
                .WithOne(r => r.Department);
            modelBuilder.Entity<Department>()
                .HasMany(p => p.Users)
                .WithOne(r => r.Department);

            modelBuilder.Entity<Department>()
                .HasOne(r => r.DepartmentHead);

            modelBuilder.Entity<DemoTask>().HasMany(d => d.Contacts).WithMany(c => c.Tasks);
            modelBuilder.Entity<Department>().HasMany(d => d.Positions);
            modelBuilder.Entity<Position>().HasMany(p => p.Departments);
        }
    }
}
