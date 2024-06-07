
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactApp1.Server.Data;
using ReactApp1.Server.Models;
using TestApp.Server.Data;
using TestApp.Server.Models;

namespace ReactApp1.Server
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddAuthorization();
            builder.Services.AddDbContext<AppDBContext>(options => {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"));
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            //builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AppDBContext>();
            builder.Services.AddIdentityApiEndpoints<User>().AddEntityFrameworkStores<AppDBContext>();
            //builder.Services.AddIdentityApiEndpoints<IdentityRole>().AddEntityFrameworkStores<AppDBContext>();

            builder.Services.AddIdentityCore<User>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<AppDBContext>();

            var app = builder.Build();

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapIdentityApi<User>();
            //app.MapIdentityApi<IdentityRole>();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            using (var scope = app.Services.CreateScope())
            {
                await DbSeeder.SeedDatabase(scope.ServiceProvider);
            }

            app.Run();
        }
    }
}
