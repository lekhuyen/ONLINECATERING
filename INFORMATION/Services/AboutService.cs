using INFORMATION.API.Models;
using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                    return false; // Return false if the about document with the given id doesn't exist
                }

                if (about.Id != id)
                {
                    throw new Exception("Mismatched ID in about object and parameter");
                }

                // Update the AboutTypeName with the new value
                var existingImagePaths = existingAbout.ImagePaths ?? new List<string>();

                // Update title and content
                existingAbout.Title = about.Title;
                existingAbout.Content = about.Content;
                existingAbout.AboutTypeId = about.AboutTypeId;

                // Handle image update
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");

                    // Delete old images from the folder
                    foreach (var imagePath in existingAbout.ImagePaths)
                    {
                        var fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
                        if (File.Exists(fullPath))
                        {
                            File.Delete(fullPath);
                        }
                    }

                    // Clear existing image paths before adding new ones
                    existingAbout.ImagePaths.Clear();

                    // Add new images
                    foreach (var imageFile in imageFiles)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        existingAbout.ImagePaths.Add("/uploads/" + uniqueFileName);
                    }
                }

                // Replace the entire document in MongoDB
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.About.ReplaceOneAsync(a => a.Id == id, existingAbout, options);

                // Delete the old image files if the image paths were updated and previously existed
                foreach (var imagePath in existingImagePaths)
                {
                    if (!existingAbout.ImagePaths.Contains(imagePath))
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

        // AboutType
        public async Task<IEnumerable<AboutType>> GetAllAboutTypesAsync()
        {
            try
            {
                return await _dbContext.AboutType.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving all AboutTypes: {ex.Message}");
            }
        }

        public async Task<AboutType> GetAboutTypeByIdAsync(string id)
        {
            try
            {
                return await _dbContext.AboutType.Find(nt => nt.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving AboutType by Id: {ex.Message}");
            }
        }

        public async Task<AboutType> GetAboutTypeByNameAsync(string name)
        {
            try
            {
                return await _dbContext.AboutType.Find(nt => nt.AboutTypeName == name).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while retrieving AboutType by name: {ex.Message}");
            }
        }

        public async Task CreateAboutTypeAsync(AboutType aboutType)
        {
            try
            {
                await _dbContext.AboutType.InsertOneAsync(aboutType);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating AboutType: {ex.Message}");
            }
        }

        public async Task<bool> UpdateAboutTypeAsync(string id, AboutType aboutType)
        {
            try
            {
                var existingAboutType = await _dbContext.AboutType.Find(nt => nt.Id == id).FirstOrDefaultAsync();

                if (existingAboutType == null)
                {
                    return false; // Return false if the news type with the given ID is not found
                }

                // Ensure the Id in the existingAboutType matches the id parameter
                if (existingAboutType.Id != id)
                {
                    throw new Exception("Mismatched ID in existingAboutType and parameter");
                }

                // Update the AboutTypeName with the new value
                existingAboutType.AboutTypeName = aboutType.AboutTypeName;

                // Define options for the ReplaceOneAsync operation
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };

                // Perform the update operation using ReplaceOneAsync
                var result = await _dbContext.AboutType.ReplaceOneAsync(nt => nt.Id == id, existingAboutType, options);

                // Check if the update was successful
                if (result.ModifiedCount == 0 && result.MatchedCount == 0)
                {
                    return false; // Return false if no documents were modified
                }

                return true; // Return true indicating successful update
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while updating AboutType: {ex.Message}");
            }
        }


        public async Task<bool> DeleteAboutTypeAsync(string id)
        {
            try
            {
                var result = await _dbContext.AboutType.DeleteOneAsync(nt => nt.Id == id);
                return result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while deleting AboutType: {ex.Message}");
            }
        }

    }
}
