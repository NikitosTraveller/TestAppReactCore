namespace TestApp.Server.Models
{
    public class LoginRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public bool Remember { get; set; }
    }
}
