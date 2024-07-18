using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using MongoDB.Driver;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

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

        public async Task CreateAsync(News news, List<IFormFile>? imageFiles)
        {
            try
            {
                await HandleImageUpload(news, imageFiles);
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

                // Store the image paths to delete if they exist
                var imagePathsToDelete = newsToDelete.ImagePaths;

                var result = await _dbContext.News.DeleteOneAsync(a => a.Id == id);
                if (result.DeletedCount == 0)
                {
                    return false;
                }

                // Delete the image files if they exist
                if (imagePathsToDelete != null && imagePathsToDelete.Any())
                {
                    foreach (var imagePath in imagePathsToDelete)
                    {
                        var filePath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
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

        public async Task<bool> UpdateAsync(string id, News news, List<IFormFile>? imageFiles)
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

                // Store the existing image paths to delete if necessary
                var existingImagePaths = existingNews.ImagePaths;

                existingNews.Title = news.Title;
                existingNews.Content = news.Content;
                existingNews.NewsTypeId = news.NewsTypeId;

                await HandleImageUpload(existingNews, imageFiles);

                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.News.ReplaceOneAsync(a => a.Id == id, existingNews, options);

                // Delete the old image files if the image paths were updated and previously existed
                if (existingImagePaths != null && existingImagePaths.Any())
                {
                    foreach (var imagePath in existingImagePaths)
                    {
                        var oldFilePath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
                        if (File.Exists(oldFilePath))
                        {
                            File.Delete(oldFilePath);
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating about content: {ex.Message}");
            }
        }

        private async Task HandleImageUpload(News news, List<IFormFile>? imageFiles)
        {
            if (imageFiles != null && imageFiles.Count > 0)
            {
                if (imageFiles.Count > 5)
                {
                    throw new Exception("Cannot upload more than 5 images");
                }

                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var imageFile in imageFiles)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    news.ImagePaths.Add("/uploads/" + uniqueFileName);
                }
            }

            // Ensure ImagePaths is not null when inserting or updating into MongoDB
            if (news.ImagePaths == null)
            {
                news.ImagePaths = new List<string>();
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
