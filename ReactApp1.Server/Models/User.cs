using Microsoft.AspNetCore.Identity;
using ReactApp1.Server.Models;

namespace TestApp.Server.Models
{
    public class User : IdentityUser
    {
        public int LoginCount { get; set; } = 0;

        public DateTime? LastLoginDate { get; set; }

        public string Avatar {  get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
