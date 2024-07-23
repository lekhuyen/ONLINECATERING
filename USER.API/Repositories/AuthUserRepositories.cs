
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

        public async Task<User> Login(Login login)
        {
            

            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserEmail == login.UserEmail);
            
            if (user != null)
            {
                bool veriPass = PasswordBcrypt.VerifyPassword(login.Password, user.Password);
                if(veriPass)
                {
                    return user;
                }
                return null;
            }
            return null;
        }

        
    }
}
