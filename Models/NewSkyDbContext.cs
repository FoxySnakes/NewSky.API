
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using System.Reflection;

namespace NewSky.API.Models
{
    public class NewSkyDbContext : DbContext
    {
        public NewSkyDbContext(DbContextOptions<NewSkyDbContext> dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var modelTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !type.IsAbstract && !type.IsGenericType && type.IsClass && typeof(EntityBase).IsAssignableFrom(type));

            foreach (var modelType in modelTypes)
            {
                modelBuilder.Entity(modelType);
            }

            // User
            modelBuilder.Entity<User>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(x => x.UUID)
                .IsUnique();

            // Role
            modelBuilder.Entity<Role>()
                .HasIndex(x => x.Level)
                .IsUnique();

            // User Permission 
            modelBuilder.Entity<UserPermission>()
                .HasKey(x => new { x.UserId, x.PermissionId });

            modelBuilder.Entity<UserPermission>()
                .HasOne(x => x.User)
                .WithMany(u => u.Permissions)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);

            // Role Permission

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Role)
                .WithMany(u => u.Permissions)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<UserPermission>()
                .HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);

            // User Role

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.Role)
                .WithMany()
                .HasForeignKey(x => x.RoleId);

            // Vote Reward
            modelBuilder.Entity<VoteReward>()
                .HasIndex(x => x.Position)
                .IsUnique();

            // UserPackage

            modelBuilder.Entity<UserPackage>()
                .HasOne(x => x.User)
                .WithMany(x => x.Packages)
                .HasForeignKey(x => x.UserId);

            modelBuilder.Entity<UserPackage>()
                .HasOne(x => x.Package)
                .WithMany()
                .HasForeignKey(x => x.PackageId);

            // Package
            modelBuilder.Entity<Package>()
                .Property(x => x.TotalPrice)
                .HasPrecision(12,2);

            modelBuilder.Entity<Package>()
                .HasIndex(x => x.TebexId)
                .IsUnique();

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {

        }
    }
}
