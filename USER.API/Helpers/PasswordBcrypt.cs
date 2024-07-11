using BCrypt.Net;

namespace USER.API.Helpers
{
    public class PasswordBcrypt
    {
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }

        public static bool VerifyPassword(string enterPassword, string password)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(enterPassword, password);   
        }
    }
}
