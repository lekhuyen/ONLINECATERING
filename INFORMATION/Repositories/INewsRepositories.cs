using INFORMATIONAPI.Models;

namespace INFORMATIONAPI.Repositories
{
    public interface INewsRepositories
    {
        Task<IEnumerable<News>> GetAllAsync();
        Task<News> GetByIdAsync(string id);
        Task CreateAsync(News news, IFormFile? imageFile);
        Task<bool> UpdateAsync(string id, News news, IFormFile? imageFile);
        Task<bool> DeleteAsync(string id);
    }
}
