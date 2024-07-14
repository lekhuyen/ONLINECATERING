
using Microsoft.EntityFrameworkCore;
using USER.API.Helpers;
using USER.API.Models;

namespace USER.API.Repositories
{
    public class AuthUserRepositories : IAuthUser
    {
        private readonly DatabaseContext _dbContext;

        public AuthUserRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Login(string email, string password)
        {
            

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == email);
            
            if (user != null)
            {
                bool veriPass = PasswordBcrypt.VerifyPassword(password, user.Password);
                if(veriPass)
                {
                    return user;
                }
            }
            return null;
        }

        
    }
}
