using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using MongoDB.Driver;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using INFORMATION.API.Helper;

namespace INFORMATIONAPI.Service
{
    public class NewsService : INewsRepositories
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _env;
        private readonly FileUpload _fileUpload; // Add FileUpload dependency


        public NewsService(DatabaseContext dbContext, IWebHostEnvironment env, FileUpload fileUpload)
        {
            _dbContext = dbContext;
            _env = env;
            _fileUpload = fileUpload; // Initialize the fileUpload dependency
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

        public async Task<bool> UpdateAsync(string id, News news, List<IFormFile>? imageFiles, string subFolder)
        {
            try
            {
                var existingNews = await _dbContext.News.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (existingNews == null)
                {
                    return false;
                }

                if (news.Id != id)
                {
                    throw new Exception("Mismatched ID in news object and parameter");
                }

                var existingImagePaths = existingNews.ImagePaths ?? new List<string>();

                // Update fields
                existingNews.Title = news.Title;
                existingNews.Content = news.Content;
                existingNews.NewsTypeId = news.NewsTypeId;

                // Handle image update
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    // Clear existing image paths
                    existingNews.ImagePaths = new List<string>();

                    // Process new image files
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await _fileUpload.SaveImage(subFolder, imageFile);
                        existingNews.ImagePaths.Add(imagePath);
                    }

                }

                // Update the document
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.News.ReplaceOneAsync(a => a.Id == id, existingNews, options);

                // Delete old images if necessary
                foreach (var imagePath in existingImagePaths)
                {
                    if (!existingNews.ImagePaths.Contains(imagePath))
                    {
                        _fileUpload.DeleteImage(imagePath);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Handle exceptions
                throw new Exception($"Error while updating news content: {ex.Message}");
            }
        }

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var existingNews = await _dbContext.News.Find(a => a.Id == id).FirstOrDefaultAsync();
                if (existingNews == null)
                {
                    return false;
                }

                var imagePathsToDelete = existingNews.ImagePaths;

                var result = await _dbContext.News.DeleteOneAsync(a => a.Id == id);
                if (result.DeletedCount == 0)
                {
                    return false;
                }

                // Delete associated images
                if (imagePathsToDelete != null)
                {
                    foreach (var imagePath in imagePathsToDelete)
                    {
                        _fileUpload.DeleteImage(imagePath);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting news content: {ex.Message}");
            }
        }

        private async Task HandleImageUpload(News news, List<IFormFile>? imageFiles)
        {
            int maxImageCount = 5;  // Maximum number of images allowed

            // Ensure ImagePaths is initialized
            if (news.ImagePaths == null)
            {
                news.ImagePaths = new List<string>();
            }

            // Count existing images
            int existingImageCount = news.ImagePaths.Count;

            // Calculate how many more images can be added
            int remainingImageSlots = maxImageCount - existingImageCount;

            if (imageFiles != null && imageFiles.Count > 0)
            {
                if (imageFiles.Count > remainingImageSlots)
                {
                    throw new Exception($"Cannot add more than {maxImageCount} images.");
                }

                foreach (var imageFile in imageFiles)
                {
                    var imagePath = await _fileUpload.SaveImage("images", imageFile);
                    news.ImagePaths.Add(imagePath);
                }
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
                    return false; // Return false if the news type with the given ID is not found
                }

                // Ensure the Id in the existingNewType matches the id parameter
                if (existingNewType.Id != id)
                {
                    throw new Exception("Mismatched ID in existingNewType and parameter");
                }

                // Update the NewsTypeName with the new value
                existingNewType.NewsTypeName = newType.NewsTypeName;

                // Define options for the ReplaceOneAsync operation
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };

                // Perform the update operation using ReplaceOneAsync
                var result = await _dbContext.NewsType.ReplaceOneAsync(nt => nt.Id == id, existingNewType, options);

                // Check if the update was successful
                if (result.ModifiedCount == 0 && result.MatchedCount == 0)
                {
                    return false; // Return false if no documents were modified
                }

                return true; // Return true indicating successful update
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
