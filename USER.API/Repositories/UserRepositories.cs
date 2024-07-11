using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using USER.API.Helpers;
using USER.API.Models;

namespace USER.API.Repositories
{
    public class UserRepositories : IRepositories
    {
        private readonly DatabaseContext _dbContext;
        private readonly IDistributedCache _cache;
        public UserRepositories(DatabaseContext dbContext, IDistributedCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        public async Task<int> AddUserAsync(User user)
        {
            
            var userExistEmail = await _dbContext.Users.Find(u => u.UserEmail == user.UserEmail).FirstOrDefaultAsync();

            if (userExistEmail != null)
            {
                return 1;
            }

            var userExistPhone = await _dbContext.Users.Find(u => u.Phone == user.Phone).FirstOrDefaultAsync();
                        
            if (userExistPhone != null)
            {
                 return 2;
            }
            
            user.Password = PasswordBcrypt.HashPassword(user.Password);
            await _dbContext.Users.InsertOneAsync(user);
            return 0;

        }

        public async Task DeleteUserAsync(string id)
        {
            await _dbContext.Users.DeleteOneAsync(u => u.Id == id);
        }

        public async Task<IEnumerable<User>> GetAllUserAsync()
        {
            var users = await _dbContext.Users.Find(u => true).ToListAsync();
            return users;
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var user = await _dbContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();
            return user;
        }

        public async Task<User> GetByIdRefeshToken(string refeshToken)
        {
            var user = await _dbContext.Users.Find(u => u.RefeshToken == refeshToken).FirstOrDefaultAsync();
            return user;
        }

        public async Task UpdateUserAsync(string id, User user)
        {
            await _dbContext.Users.ReplaceOneAsync(u => u.Id == id, user);
        }
    }
}
