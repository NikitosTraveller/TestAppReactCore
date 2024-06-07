using Microsoft.AspNetCore.Identity;
using ReactApp1.Server.Models;
using TestApp.Server.Models;

namespace ReactApp1.Server.Data
{
    public class DbSeeder
    {
        public static async Task SeedDatabase(IServiceProvider service)
        {
            var userManager = service.GetService<UserManager<User>>();
            var roleManager = service.GetService<RoleManager<IdentityRole>>();

            var superAdminRole = new IdentityRole(Role.SuperAdmin.ToString());
            var adminRole = new IdentityRole(Role.Admin.ToString());
            var regularUserRole = new IdentityRole(Role.Regular.ToString());

            await roleManager.CreateAsync(regularUserRole);
            await roleManager.CreateAsync(superAdminRole);
            await roleManager.CreateAsync(adminRole);

            var superAdmin = new User()
            {
                UserName = SuperAdminData.UserName,
                Email = SuperAdminData.Email,
                PasswordHash = SuperAdminData.PasswordHash,
            };

            var userInDb = await userManager.FindByEmailAsync(superAdmin.Email);
            if (userInDb == null)
            {
                await userManager.CreateAsync(superAdmin, SuperAdminData.Password);
                await userManager.AddToRoleAsync(superAdmin, Role.SuperAdmin.ToString());
            }
        }
    }
}
