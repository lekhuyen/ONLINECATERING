using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace INFORMATIONAPI.Service
{
    public class AboutService : IAboutRepositories
    {
        private readonly DatabaseContext _dbContext;
        private readonly IWebHostEnvironment _env;

        public AboutService(DatabaseContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _env = env;
        }

        public async Task<IEnumerable<About>> GetAllAsync()
        {
            try
            {
                return await _dbContext.About.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving all about content: {ex.Message}");
            }
        }

        public async Task<About> GetByIdAsync(string id)
        {
            try
            {
                return await _dbContext.About.Find(a => a.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving about content by Id: {ex.Message}");
            }
        }

        public async Task CreateAsync(About about, List<IFormFile>? imageFiles)
        {
            try
            {
                await HandleImageUpload(about, imageFiles);

                await _dbContext.About.InsertOneAsync(about);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating about content: {ex.Message}");
            }
        }

        public async Task<bool> UpdateAsync(string id, About about, List<IFormFile>? imageFiles)
        {
            try
            {
                var existingAbout = await _dbContext.About.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (existingAbout == null)
                {
                    return false;
                }

                if (about.Id != id)
                {
                    throw new Exception("Mismatched ID in about object and parameter");
                }

                var existingImagePaths = existingAbout.ImagePaths ?? new List<string>();

                existingAbout.Title = about.Title;
                existingAbout.Content = about.Content;

                // Handle image update
                await HandleImageUpdate(existingAbout, imageFiles);

                // Replace the entire document in MongoDB
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.About.ReplaceOneAsync(a => a.Id == id, existingAbout, options);

                // Clean up old image files (if needed)
                if (existingImagePaths != null)
                {
                    foreach (var path in existingImagePaths)
                    {
                        if (!existingAbout.ImagePaths.Contains(path))
                        {
                            var oldFilePath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));
                            if (File.Exists(oldFilePath))
                            {
                                File.Delete(oldFilePath);
                            }
                        }
                    }
                }

                // Log existing image paths for debugging
                Console.WriteLine($"Existing Image Paths after update: {string.Join(", ", existingAbout.ImagePaths)}");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating about content: {ex.Message}");
            }
        }


        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var aboutToDelete = await _dbContext.About.Find(a => a.Id == id).FirstOrDefaultAsync();
                if (aboutToDelete == null)
                {
                    return false;
                }

                var imagePathsToDelete = aboutToDelete.ImagePaths;

                var result = await _dbContext.About.DeleteOneAsync(a => a.Id == id);
                if (result.DeletedCount == 0)
                {
                    return false;
                }

                if (imagePathsToDelete != null)
                {
                    foreach (var path in imagePathsToDelete)
                    {
                        var filePath = Path.Combine(_env.WebRootPath, path.TrimStart('/'));
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
                throw new Exception($"Error while deleting about content: {ex.Message}");
            }
        }

        private async Task HandleImageUpload(About about, List<IFormFile>? imageFiles)
        {
            // Define the maximum number of images allowed
            int maxImageCount = 5;  // Example: Limiting to 5 images

            // Ensure ImagePaths is initialized
            if (about.ImagePaths == null)
            {
                about.ImagePaths = new List<string>();
            }

            // Count existing images
            int existingImageCount = about.ImagePaths.Count;

            // Calculate how many more images can be added
            int remainingImageSlots = maxImageCount - existingImageCount;

            if (imageFiles != null && imageFiles.Count > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                foreach (var imageFile in imageFiles)
                {
                    if (remainingImageSlots > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        about.ImagePaths.Add("/uploads/" + uniqueFileName);
                        remainingImageSlots--;
                    }
                    else
                    {
                        // Handle case where maximum image count is exceeded
                        throw new Exception($"Cannot add more than {maxImageCount} images.");
                    }
                }
            }
        }

        private async Task HandleImageUpdate(About about, List<IFormFile>? imageFiles)
        {
            if (imageFiles != null && imageFiles.Count > 0)
            {
                var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Clear existing image paths before adding new ones
                about.ImagePaths.Clear();

                foreach (var imageFile in imageFiles)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    about.ImagePaths.Add("/uploads/" + uniqueFileName);
                }
            }
        }

    }
}
