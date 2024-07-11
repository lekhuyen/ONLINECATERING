
using MongoDB.Driver;
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
            var filter = Builders<User>.Filter.Eq(u => u.UserEmail, email);
            //&Builders<User>.Filter.Eq(u => u.Password, password)
            var user = await _dbContext.Users.Find(filter).FirstOrDefaultAsync();
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
