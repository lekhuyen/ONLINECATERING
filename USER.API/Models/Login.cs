namespace USER.API.Models
{
    public class Login
    {
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public string? Otp { get; set; }
        public string? LoginToken { get; set; }
    }
}
