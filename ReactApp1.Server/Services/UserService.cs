using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ReactApp1.Server.Models;
using System.Security.Claims;
using TestApp.Server.Models;

namespace ReactApp1.Server.Services
{
    public class UserService(UserManager<User> um) : IUserService
    {
        private readonly UserManager<User> _userManager = um;

        public async Task<IdentityResult> ChangeUserRoleAsync(User user, string roleName)
        {
            await _userManager.RemoveFromRolesAsync(user, [AppUserRole.Admin.ToString(), AppUserRole.Regular.ToString()]);
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            var addedUser = await _userManager.FindByEmailAsync(user.Email);
            return await _userManager.AddToRoleAsync(addedUser, AppUserRole.Regular.ToString());
        }

        public async Task<IdentityResult> DeleteUserAsync(User user)
        {
            await _userManager.RemoveFromRolesAsync(user, [AppUserRole.Admin.ToString(), AppUserRole.Regular.ToString()]);
            return await _userManager.DeleteAsync(user);
        }

        public async Task<List<User>> GetAllUsersAsync(string userId)
        {
            return await _userManager.Users
                .Where(u => u.Id != userId)
                .Include(x => x.UserRoles)
                .ThenInclude(r => r.Role)
                .ToListAsync();
        }

        public async Task<User> GetCurrentUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await _userManager.GetUserAsync(claimsPrincipal);
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<string> GetUserRoleAsync(User user)
        {
            return (await _userManager.GetRolesAsync(user)).FirstOrDefault();
        }

        public async Task<IdentityResult> LoginUserAsync(User user)
        {
            user.LastLoginDate = DateTime.Now;
            user.LoginCount++;
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
