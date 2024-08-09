namespace USER.API.Models
{
    public class ChangeLogin
    {
        public string? UserEmail { get; set; }
        public string? Password { get; set; }
        public string? OldPassword { get; set; }
    }
}
