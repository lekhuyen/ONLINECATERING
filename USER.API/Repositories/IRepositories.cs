using USER.API.DTOs;
using USER.API.Models;

namespace USER.API.Repositories
{
    public interface IRepositories
    {
        Task<IEnumerable<UserDTO>> GetAllUserAsync();
        Task<UserDTO> GetByIdAsync(int id);
        Task<User> GetOneByIdAsync(int id);
        Task<User> GetByIdRefeshToken(string id);
        Task<int> AddUserAsync(User user);
        Task<int> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<bool> EditUserForAdmin(int userId, bool newStatus);
    }
}
