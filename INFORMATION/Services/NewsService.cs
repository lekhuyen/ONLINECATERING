using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using MongoDB.Driver;

namespace INFORMATIONAPI.Service
{
    public class NewsService : INewsRepositories
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public NewsService(DatabaseContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task CreateAsync(News news, IFormFile? imageFile)
        {
            try
            {
                await HandleImageUpload(news, imageFile);

                await _dbContext.News.InsertOneAsync(news);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating news content: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var newsToDelete = await _dbContext.News.Find(a => a.Id == id).FirstOrDefaultAsync();
                if (newsToDelete == null)
                {
                    return false;
                }

                // Store the image path to delete if it exists
                string imagePathToDelete = newsToDelete.ImagePath;

                var result = await _dbContext.News.DeleteOneAsync(a => a.Id == id);
                if (result.DeletedCount == 0)
                {
                    return false;
                }

                // Delete the image file if it exists
                if (!string.IsNullOrEmpty(imagePathToDelete))
                {
                    var filePath = Path.Combine(_env.WebRootPath, imagePathToDelete.TrimStart('/'));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting news content: {ex.Message}");
            }
        }

        public async Task<IEnumerable<News>> GetAllAsync()
        {
            try
            {
                return await _dbContext.News.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving all news content: {ex.Message}");
            }
        }

        public async Task<News> GetByIdAsync(string id)
        {
            try
            {
                return await _dbContext.News.Find(a => a.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving news content by Id: {ex.Message}");
            }
        }

        public async Task<bool> UpdateAsync(string id, News news, IFormFile? imageFile)
        {
            try
            {
                var existingNews = await _dbContext.News.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (existingNews == null)
                {
                    return false;
                }

                // Ensure the Id in the about object matches the id parameter
                if (news.Id != id)
                {
                    throw new Exception("Mismatched ID in about object and parameter");
                }

                // Store the existing image path to delete if necessary
                string existingImagePath = existingNews.ImagePath;

                existingNews.Title = news.Title;
                existingNews.Content = news.Content;

                await HandleImageUpload(existingNews, imageFile);

                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.News.ReplaceOneAsync(a => a.Id == id, existingNews, options);

                // Delete the old image file if the image path was updated and previously existed
                if (!string.IsNullOrEmpty(existingImagePath) && existingImagePath != existingNews.ImagePath)
                {
                    var oldFilePath = Path.Combine(_env.WebRootPath, existingImagePath.TrimStart('/'));
                    if (File.Exists(oldFilePath))
                    {
                        File.Delete(oldFilePath);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating about content: {ex.Message}");
            }
        }

        private async Task HandleImageUpload(News news, IFormFile? imageFile)
        {
            if (imageFile != null && imageFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                news.ImagePath = "/uploads/" + uniqueFileName;
            }

            // Ensure ImagePath is not null when inserting or updating into MongoDB
            if (news.ImagePath == null)
            {
                news.ImagePath = string.Empty; // or null if desired
            }
        }

        // NewTypes
        public async Task<IEnumerable<NewsType>> GetAllNewTypesAsync()
        {
            try
            {
                return await _dbContext.NewsType.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving all NewTypes: {ex.Message}");
            }
        }

        public async Task<NewsType> GetNewTypeByIdAsync(string id)
        {
            try
            {
                return await _dbContext.NewsType.Find(nt => nt.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving NewType by Id: {ex.Message}");
            }
        }

        public async Task<NewsType> GetNewsTypeByNameAsync(string name)
        {
            try
            {
                return await _dbContext.NewsType.Find(nt => nt.NewsTypeName == name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving NewType by name: {ex.Message}");
            }
        }

        public async Task CreateNewTypeAsync(NewsType newType)
        {
            try
            {
                await _dbContext.NewsType.InsertOneAsync(newType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating NewType: {ex.Message}");
            }
        }

        public async Task<bool> UpdateNewTypeAsync(string id, NewsType newType)
        {
            try
            {
                var existingNewType = await _dbContext.NewsType.Find(nt => nt.Id == id).FirstOrDefaultAsync();

                if (existingNewType == null)
                {
                    return false;
                }

                existingNewType.NewsTypeName = newType.NewsTypeName;

                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.NewsType.ReplaceOneAsync(nt => nt.Id == id, existingNewType, options);

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating NewType: {ex.Message}");
            }
        }

        public async Task<bool> DeleteNewTypeAsync(string id)
        {
            try
            {
                var result = await _dbContext.NewsType.DeleteOneAsync(nt => nt.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting NewType: {ex.Message}");
            }
        }
    }
}
