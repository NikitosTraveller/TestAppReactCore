using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using TestApp.Server.Models;

namespace TestApp.Server.Data
{
    public class AppDBContext : IdentityDbContext<User>
    {

        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
           Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var superAdmin = new User()
            {
                UserName = SuperAdminData.UserName,
                Email = SuperAdminData.Email,
                PasswordHash = SuperAdminData.Password,
                IsAdmin = SuperAdminData.IsAdmin,
            };

            builder.Entity<User>().HasData(superAdmin);

            base.OnModelCreating(builder);
        }
    }
}
