
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;

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

            // Role
            modelBuilder.Entity<Role>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // User Role

            modelBuilder.Entity<UserRole>()
                .HasOne(x => x.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(x => x.UserId);

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
                .Property(x => x.PriceHt)
                .HasPrecision(12,2);

            modelBuilder.Entity<Package>()
                .Property(x => x.PriceTtc)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Package>()
                .HasIndex(x => x.TebexId)
                .IsUnique();

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Permissions
            var permissionIdByName = new Dictionary<string, int>();
            var i = 1;
            foreach (var property in typeof(PermissionName).GetFields())
            {
                modelBuilder.Entity<Permission>()
                    .HasData(new Permission
                    {
                        Id = i,
                        Name = (string)property.GetValue(null),
                        Description = property.GetCustomAttribute<DescriptionAttribute>().Description
                    });
                permissionIdByName.Add((string)property.GetValue(null), i);
                i++;
            }

            // Player Role
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = -1,
                    Name = DefaultRole.Player,
                    Description = typeof(DefaultRole).GetField(nameof(DefaultRole.Player)).GetCustomAttribute<DescriptionAttribute>().Description,
                    IsDefault = true,
                });

            modelBuilder.Entity<RolePermission>()
                .HasData(new RolePermission
                {
                    Id = -1,
                    RoleId = -1,
                    PermissionId = permissionIdByName[PermissionName.ManageUserCart],
                    IsEditable = false,
                    HasPermission = true
                });

            // Owner Role
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = -2,
                    Name = DefaultRole.Owner,
                    Description = typeof(DefaultRole).GetField(nameof(DefaultRole.Owner)).GetCustomAttribute<DescriptionAttribute>().Description,
                    IsDefault = true,
                });

            modelBuilder.Entity<RolePermission>()
                .HasData(Enumerable.Range(1, permissionIdByName.Count)
                    .Select(id => new RolePermission
                    {
                        Id = -(id+1),
                        RoleId = -2,
                        PermissionId = id,
                        IsEditable = false,
                        HasPermission = true
                    })
                    .ToArray());


            // WebSite Developer Role
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = -3,
                    Name = DefaultRole.WebSiteDeveloper,
                    Description = typeof(DefaultRole).GetField(nameof(DefaultRole.WebSiteDeveloper)).GetCustomAttribute<DescriptionAttribute>().Description,
            IsDefault = true,
                });

            modelBuilder.Entity<RolePermission>()
                .HasData(Enumerable.Range(1, permissionIdByName.Count)
                    .Select(id => new RolePermission
                    {
                        Id = -(id + 1 + permissionIdByName.Count),
                        RoleId = -3,
                        PermissionId = id,
                        IsEditable = false,
                        HasPermission = true
                    })
                    .ToArray());
        }
    }
}
