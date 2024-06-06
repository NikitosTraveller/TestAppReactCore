using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
            base.OnModelCreating(builder);
        }
    }
}
