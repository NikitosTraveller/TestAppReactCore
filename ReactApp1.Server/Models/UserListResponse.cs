namespace ReactApp1.Server.Models
{
    public class UserListResponse
    {
        public string Id { get; set; }

        public int LoginCount { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public DateTime LastLoginDate { get; set; }

        public bool IsAdmin { get; set; }
    }
}
