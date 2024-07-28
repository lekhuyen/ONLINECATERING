using INFORMATION.API.Models;
using INFORMATIONAPI.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace INFORMATIONAPI.Repositories
{
    public interface IAboutRepositories
    {
        Task<IEnumerable<About>> GetAllAsync();
        Task<About> GetByIdAsync(string id);
        Task CreateAsync(About about, List<IFormFile>? imageFiles);
        Task<bool> UpdateAsync(string id, About about, List<IFormFile>? imageFiles, string subFolder); 
        Task<bool> DeleteAsync(string id);

        // AboutType CRUD operations
        Task<IEnumerable<AboutType>> GetAllAboutTypesAsync();
        Task<AboutType> GetAboutTypeByIdAsync(string id);
        Task CreateAboutTypeAsync(AboutType aboutType);
        Task<bool> UpdateAboutTypeAsync(string id, AboutType aboutType);
        Task<bool> DeleteAboutTypeAsync(string id);
        Task<AboutType> GetAboutTypeByNameAsync(string aboutTypeName);
    }
}
