using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public async Task CreateAsync(About about, IFormFile? imageFile)
        {
            try
            {
                await HandleImageUpload(about, imageFile);

                await _dbContext.About.InsertOneAsync(about);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error while creating about content: {ex.Message}");
            }
        }

        public async Task<bool> UpdateAsync(string id, About about, IFormFile? imageFile)
        {
            try
            {
                var existingAbout = await _dbContext.About.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (existingAbout == null)
                {
                    return false;
                }

                // Ensure the Id in the about object matches the id parameter
                if (about.Id != id)
                {
                    throw new Exception("Mismatched ID in about object and parameter");
                }

                // Store the existing image path to delete if necessary
                string existingImagePath = existingAbout.ImagePath;

                existingAbout.Title = about.Title;
                existingAbout.Content = about.Content;

                await HandleImageUpload(existingAbout, imageFile);

                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.About.ReplaceOneAsync(a => a.Id == id, existingAbout, options);

                // Delete the old image file if the image path was updated and previously existed
                if (!string.IsNullOrEmpty(existingImagePath) && existingImagePath != existingAbout.ImagePath)
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

        public async Task<bool> DeleteAsync(string id)
        {
            try
            {
                var aboutToDelete = await _dbContext.About.Find(a => a.Id == id).FirstOrDefaultAsync();
                if (aboutToDelete == null)
                {
                    return false;
                }

                // Store the image path to delete if it exists
                string imagePathToDelete = aboutToDelete.ImagePath;

                var result = await _dbContext.About.DeleteOneAsync(a => a.Id == id);
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
                throw new Exception($"Error while deleting about content: {ex.Message}");
            }
        }


        private async Task HandleImageUpload(About about, IFormFile? imageFile)
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

                about.ImagePath = "/uploads/" + uniqueFileName;
            }

            // Ensure ImagePath is not null when inserting or updating into MongoDB
            if (about.ImagePath == null)
            {
                about.ImagePath = string.Empty; // or null if desired
            }
        }

    }
}
