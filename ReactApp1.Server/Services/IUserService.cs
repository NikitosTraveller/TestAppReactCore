using Microsoft.AspNetCore.Identity;
using TestApp.Server.Models;

namespace ReactApp1.Server.Services
{
    public interface IUserService
    {
        public Task<IdentityResult> CreateUserAsync(User user, string password);

        public Task<IdentityResult> LoginUserAsync(User user);

        public Task<IdentityResult> DeleteUserAsync(User user);

        public Task<IdentityResult> ChangeUserRoleAsync(User user, string roleName);

        public Task<string> GetUserRoleAsync(User user);

        public Task<User> GetUserByIdAsync(string userId);

        public Task<User> GetUserByEmailAsync(string email);

        public Task<IdentityResult> UpdateUserAsync(User user);

        public Task<List<User>> GetAllUsersAsync(string userId);
    }
}
