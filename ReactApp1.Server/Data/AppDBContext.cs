using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Runtime.CompilerServices;
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
            var superAdminRole = new IdentityRole(Role.SuperAdmin.ToString());
            var adminRole = new IdentityRole(Role.Admin.ToString());
            var regularUserRole = new IdentityRole(Role.Regular.ToString());

            builder.Entity<IdentityRole>().HasData(superAdminRole, adminRole, regularUserRole);

            base.OnModelCreating(builder);
        }
    }
}
