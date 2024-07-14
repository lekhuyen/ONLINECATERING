using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public interface ICategory
    {
        Task<IEnumerable<Category>> GetCategoryAsync();
        Task<Category> AddCategoryAsync(Category category);
        Task<Category> UpdateCategoryAsync(Category category);
        Task DeleteRestaurantAsync(int id);
        Task<Category> GetCategoryByIdAsync(int id);
    }
}
