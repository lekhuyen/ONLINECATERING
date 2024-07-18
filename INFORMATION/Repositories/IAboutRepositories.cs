using INFORMATIONAPI.Models;

namespace INFORMATIONAPI.Repositories
{
    public interface IAboutRepositories
    {
        Task<IEnumerable<About>> GetAllAsync();
        Task<About> GetByIdAsync(string id);
        Task CreateAsync(About about, List<IFormFile>? imageFiles);
        Task<bool> UpdateAsync(string id, About about, List<IFormFile>? imageFiles);
        Task<bool> DeleteAsync(string id);
    }
}
