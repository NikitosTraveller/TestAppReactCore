using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Server.Models
{
    public class User : IdentityUser
    {
        public int LoginCount { get; set; } = 0;

        public string Name { get; set; }

        public Nullable<DateTime> LastLoginDate { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
