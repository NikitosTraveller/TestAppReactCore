namespace ReactApp1.Server.Models
{
    public class UserResponse
    {
        public string Id { get; set; }

        public int LoginCount { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public DateTime? LastLoginDate { get; set; }
    }
}
