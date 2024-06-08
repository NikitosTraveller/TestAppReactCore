using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using TestApp.Server.Models;

namespace TestApp.Server.Data
{
    public class AppDBContext : IdentityDbContext<User, Role, string,
        IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
           Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>(b =>
            {
                b.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

            builder.Entity<Role>(b =>
            {
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();
            });

            Seed(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            var superAdmin = new User()
            {
                UserName = SuperAdminData.UserName,
                Email = SuperAdminData.Email,
                PasswordHash = SuperAdminData.PasswordHash,
            };

            var superAdminRole = new Role()
            {
                Name = AppUserRole.SuperAdmin.ToString(),
                NormalizedName = AppUserRole.SuperAdmin.ToString().ToUpper()
            };

            var adminRole = new Role()
            {
                Name = AppUserRole.Admin.ToString(),
                NormalizedName = AppUserRole.Admin.ToString().ToUpper()
            };

            var regularUserRole = new Role()
            {
                Name = AppUserRole.Regular.ToString(),
                NormalizedName = AppUserRole.Regular.ToString().ToUpper()
            };

            builder.Entity<Role>().HasData(superAdminRole, adminRole, regularUserRole);
            builder.Entity<User>().HasData(superAdmin);

            var userRoleAdmin = new UserRole()
            {
                RoleId = superAdminRole.Id,
                UserId = superAdmin.Id
            };

            builder.Entity<UserRole>().HasData(userRoleAdmin);
        }
    }
}
