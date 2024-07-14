using Microsoft.EntityFrameworkCore;
using RESTAURANT.API.Models;

namespace RESTAURANT.API.Repositories
{
    public class CategoryRepositories:ICategory
    {
        private readonly DatabaseContext _dbContext;
        public CategoryRepositories(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Category> AddCategoryAsync(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task DeleteRestaurantAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category != null)
            {
                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetCategoryAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }
    }
}
