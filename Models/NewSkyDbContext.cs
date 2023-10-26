
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace NewSky.API.Models
{
    public class NewSkyDbContext : IdentityDbContext<User>
    {
        public NewSkyDbContext(DbContextOptions<NewSkyDbContext> dbContextOptions): base(dbContextOptions)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Utilise la réflexion pour obtenir tous les types de modèles
            var modelTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => !type.IsAbstract && !type.IsGenericType && type.IsClass && typeof(EntityBase).IsAssignableFrom(type));

            // Ajoute chaque type de modèle au contexte
            foreach (var modelType in modelTypes)
            {
                modelBuilder.Entity(modelType);
            }

            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.PhoneNumber).IsUnique();
            modelBuilder.Entity<VoteReward>().HasIndex(x => x.Position).IsUnique();



            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(new List<IdentityRole>()
            {
                new IdentityRole()
                {
                    Id = Role.Player,
                    Name = Role.Player,
                    NormalizedName = Role.Player,
                },
                new IdentityRole()
                {
                    Id = Role.Helper,
                    Name = Role.Helper,
                    NormalizedName = Role.Helper,
                },
                new IdentityRole()
                {
                    Id = Role.Moderator,
                    Name = Role.Moderator,
                    NormalizedName = Role.Moderator
                },
                new IdentityRole()
                {
                    Id = Role.Admin,
                    Name = Role.Admin,
                    NormalizedName = Role.Admin
                },
                new IdentityRole()
                {
                    Id = Role.SuperAdmin,
                    Name = Role.SuperAdmin,
                    NormalizedName = Role.SuperAdmin
                },
                new IdentityRole()
                {
                    Id = Role.Developer,
                    Name = Role.Developer,
                    NormalizedName = Role.Developer
                }
            });
        }
    }
}
