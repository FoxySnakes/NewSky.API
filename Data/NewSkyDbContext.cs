using Microsoft.EntityFrameworkCore;
using NewSky.API.Models;
using NewSky.API.Models.Db;
using System.ComponentModel;
using System.Reflection;

namespace NewSky.API.Data
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

            // Role Permission

            modelBuilder.Entity<RolePermission>()
                .HasIndex(x => new { x.RoleId, x.PermissionId })
                .IsUnique();

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Role)
                .WithMany(u => u.Permissions)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<RolePermission>()
                .HasOne(x => x.Permission)
                .WithMany()
                .HasForeignKey(x => x.PermissionId);

            // Role
            modelBuilder.Entity<Role>()
                .HasIndex(x => x.Name)
                .IsUnique();

            // User Role

            modelBuilder.Entity<UserRole>()
                .HasIndex(x => new { x.RoleId, x.UserId })
                .IsUnique();

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
                .HasKey(x => x.Position);

            // User Package

            modelBuilder.Entity<UserPackage>()
                .HasIndex(x => new { x.UserId, x.PackageId })
                .IsUnique();

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
                .HasPrecision(12, 2);

            modelBuilder.Entity<Package>()
                .Property(x => x.PriceTtc)
                .HasPrecision(12, 2);

            modelBuilder.Entity<Package>()
                .HasIndex(x => x.TebexId)
                .IsUnique();

            // AppSetting
            modelBuilder.Entity<AppSetting>()
                .HasIndex(x => x.Name)
                .IsUnique();

            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Permissions
            modelBuilder.Entity<Permission>()
                .HasData(new List<Permission>
                {
                    new Permission
                    {
                        Id = -1,
                        Name = PermissionName.AccessToAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -2,
                        Name = PermissionName.AccessToDashboardOnAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToDashboardOnAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -3,
                        Name = PermissionName.AccessToSalesOnAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToSalesOnAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -4,
                        Name = PermissionName.AccessToUsersOnAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToUsersOnAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -5,
                        Name = PermissionName.AccessToVotesOnAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToVotesOnAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -6,
                        Name = PermissionName.AccessToGeneralSettingsOnAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToGeneralSettingsOnAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -7,
                        Name = PermissionName.CreateRole,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.CreateRole)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -8,
                        Name = PermissionName.UpdateUserInformations,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.UpdateUserInformations)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -9,
                        Name = PermissionName.UpdateUserPunishment,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.UpdateUserPunishment)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -10,
                        Name = PermissionName.UpdateGeneralSettings,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.UpdateGeneralSettings)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -11,
                        Name = PermissionName.UpdateUserRole,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.UpdateUserRole)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -12,
                        Name = PermissionName.UpdateRole,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.UpdateRole)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -13,
                        Name = PermissionName.DeleteRole,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.DeleteRole)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                    new Permission
                    {
                        Id = -14,
                        Name = PermissionName.AccessToRolesOnAdminPanel,
                        Description = typeof(PermissionName).GetField(nameof(PermissionName.AccessToRolesOnAdminPanel)).GetCustomAttribute<DescriptionAttribute>().Description
                    },
                });



            // Player Role
            modelBuilder.Entity<Role>()
                .HasData(new Role
                {
                    Id = -1,
                    Name = DefaultRole.Player,
                    Description = typeof(DefaultRole).GetField(nameof(DefaultRole.Player)).GetCustomAttribute<DescriptionAttribute>().Description,
                    IsDefault = true,
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

            // Set Owner Role Permission
            modelBuilder.Entity<RolePermission>()
                .HasData(new List<RolePermission>
                {
                    new RolePermission
                    {
                        Id = -50,
                        RoleId = -2,
                        PermissionId = -1,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -51,
                        RoleId = -2,
                        PermissionId = -2,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -52,
                        RoleId = -2,
                        PermissionId = -3,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -53,
                        RoleId = -2,
                        PermissionId = -4,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -54,
                        RoleId = -2,
                        PermissionId = -5,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -55,
                        RoleId = -2,
                        PermissionId = -6,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -56,
                        RoleId = -2,
                        PermissionId = -7,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -57,
                        RoleId = -2,
                        PermissionId = -8,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -58,
                        RoleId = -2,
                        PermissionId = -9,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -59,
                        RoleId = -2,
                        PermissionId = -10,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -60,
                        RoleId = -2,
                        PermissionId = -11,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -61,
                        RoleId = -2,
                        PermissionId = -12,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -62,
                        RoleId = -2,
                        PermissionId = -13,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -63,
                        RoleId = -2,
                        PermissionId = -14,
                        IsEditable = false,
                        HasPermission = true
                    }
                });


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
                .HasData(new List<RolePermission>
                {
                    new RolePermission
                    {
                        Id = -100,
                        RoleId = -3,
                        PermissionId = -1,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -101,
                        RoleId = -3,
                        PermissionId = -2,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -102,
                        RoleId = -3,
                        PermissionId = -3,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -103,
                        RoleId = -3,
                        PermissionId = -4,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -104,
                        RoleId = -3,
                        PermissionId = -5,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -105,
                        RoleId = -3,
                        PermissionId = -6,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -106,
                        RoleId = -3,
                        PermissionId = -7,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -107,
                        RoleId = -3,
                        PermissionId = -8,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -108,
                        RoleId = -3,
                        PermissionId = -9,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -109,
                        RoleId = -3,
                        PermissionId = -10,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -110,
                        RoleId = -3,
                        PermissionId = -11,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -111,
                        RoleId = -3,
                        PermissionId = -12,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -112,
                        RoleId = -3,
                        PermissionId = -13,
                        IsEditable = false,
                        HasPermission = true
                    },
                    new RolePermission
                    {
                        Id = -113,
                        RoleId = -3,
                        PermissionId = -14,
                        IsEditable = false,
                        HasPermission = true
                    }
                });

            // AppSetting
            modelBuilder.Entity<AppSetting>()
                .HasData(new List<AppSetting>()
                {
                    new AppSetting
                    {
                        Id = 1,
                        Name = nameof(AppSettingDefault.DiscordUrl),
                        Value = AppSettingDefault.DiscordUrl,
                    },
                    new AppSetting
                    {
                        Id = 2,
                        Name = nameof(AppSettingDefault.InstagramUrl),
                        Value = AppSettingDefault.InstagramUrl,
                    },
                    new AppSetting
                    {
                        Id = 3,
                        Name = nameof(AppSettingDefault.YoutubeUrl),
                        Value = AppSettingDefault.YoutubeUrl,
                    },
                    new AppSetting
                    {
                        Id = 4,
                        Name = nameof(AppSettingDefault.TwitterUrl),
                        Value = AppSettingDefault.TwitterUrl,
                    },
                    new AppSetting
                    {
                        Id = 5,
                        Name = nameof(AppSettingDefault.TikTokUrl),
                        Value = AppSettingDefault.TikTokUrl,
                    },
                    new AppSetting
                    {
                        Id = 6,
                        Name = nameof(AppSettingDefault.WebStoreIdentifier),
                        Value = AppSettingDefault.WebStoreIdentifier,
                    },
                    new AppSetting
                    {
                        Id = 7,
                        Name = nameof(AppSettingDefault.XTebexSecret),
                        Value = AppSettingDefault.XTebexSecret,
                    },
                    new AppSetting
                    {
                        Id = 8,
                        Name = nameof(AppSettingDefault.ServerIp),
                        Value = AppSettingDefault.ServerIp,
                    }
                });
        }
    }
}
