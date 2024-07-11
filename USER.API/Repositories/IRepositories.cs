using USER.API.Models;

namespace USER.API.Repositories
{
    public interface IRepositories
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetByIdAsync(string id);
        Task<User> GetByIdRefeshToken(string id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(string id,User user);
        Task DeleteUserAsync(string id);
    }
}
