using MongoDB.Driver;
using USER.API.Models;

namespace USER.API.Repositories
{
    public class UserRepositories : IRepositories
    {
        private readonly DatabaseContext _dbContext;
        public UserRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddUserAsync(User user)
        {
            await _dbContext.Users.InsertOneAsync(user);
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
