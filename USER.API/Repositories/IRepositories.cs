using USER.API.Models;

namespace USER.API.Repositories
{
    public interface IRepositories
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<User> GetByIdAsync(int id);
        Task<User> GetByIdRefeshToken(string id);
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
