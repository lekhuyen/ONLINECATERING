namespace USER.API.DTOs
{
    public class UserDTO
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; } = "User";

    }
}
