using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestApp.Server.Models
{
    public class User : IdentityUser
    {
        public int LoginCount { get; set; } = 0;

        public string Name { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime LastLoginDate { get; set; } = DateTime.Now;

        public bool IsAdmin { get; set; } = false;
    }
}
