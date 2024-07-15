using USER.API.DTOs;
using USER.API.Models;

namespace USER.API.Repositories
{
    public interface IRepositories
    {
        Task<IEnumerable<User>> GetAllUserAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<User> GetOneByIdAsync(int id);
        Task<User> GetByIdRefeshToken(string id);
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
