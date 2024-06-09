using Microsoft.AspNetCore.Identity;
using TestApp.Server.Models;

namespace ReactApp1.Server.Models
{
    public enum AppUserRole
    {
        SuperAdmin,
        Admin,
        Regular
    }

    public class UserRole : IdentityUserRole<string>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }
    }

    public class Role : IdentityRole
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
