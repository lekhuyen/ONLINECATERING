using INFORMATION.API.Helper;
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
        private readonly FileUpload _fileUpload; // Add FileUpload dependency

        public AboutService(DatabaseContext dbContext, IWebHostEnvironment env)
        {
            _dbContext = dbContext;
            _fileUpload = new FileUpload(env); // Initialize FileUpload

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

        public async Task<bool> UpdateAsync(string id, About about, List<IFormFile>? imageFiles, string subFolder)
        {
            try
            {
                // Fetch the existing About document from the database
                var existingAbout = await _dbContext.About.Find(a => a.Id == id).FirstOrDefaultAsync();

                if (existingAbout == null)
                {
                    return false; // Return false if no document is found
                }

                if (about.Id != id)
                {
                    throw new Exception("Mismatched ID in about object and parameter");
                }

                // Store the existing image paths to delete if necessary
                var existingImagePaths = existingAbout.ImagePaths ?? new List<string>();

                // Update scalar properties
                existingAbout.Title = about.Title;
                existingAbout.Content = about.Content;
                existingAbout.AboutTypeId = about.AboutTypeId;

                // Handle image update
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    // Clear existing image paths
                    existingAbout.ImagePaths.Clear();

                    // Process each image file
                    foreach (var imageFile in imageFiles)
                    {
                        var imagePath = await _fileUpload.SaveImage(subFolder, imageFile); // Save image to specified subfolder
                        existingAbout.ImagePaths.Add(imagePath); // Add new image path to existingAbout
                    }
                }

                // Replace the entire document in MongoDB
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.About.ReplaceOneAsync(a => a.Id == id, existingAbout, options);

                // Delete old images if they are no longer referenced
                foreach (var imagePath in existingImagePaths)
                {
                    if (!existingAbout.ImagePaths.Contains(imagePath))
                    {
                        _fileUpload.DeleteImage(imagePath);
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

                // Delete the document from the database
                var result = await _dbContext.About.DeleteOneAsync(a => a.Id == id);
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
                throw new Exception($"Error while deleting about content: {ex.Message}");
            }
        }


        private async Task HandleImageUpload(About about, List<IFormFile>? imageFiles)
        {
            int maxImageCount = 5;

            if (about.ImagePaths == null)
            {
                about.ImagePaths = new List<string>();
            }

            int existingImageCount = about.ImagePaths.Count;
            int remainingImageSlots = maxImageCount - existingImageCount;

            if (imageFiles != null && imageFiles.Count > 0)
            {
                foreach (var imageFile in imageFiles)
                {
                    if (remainingImageSlots > 0)
                    {
                        var imagePath = await _fileUpload.SaveImage("images", imageFile); // Save to root Uploads folder
                        about.ImagePaths.Add(imagePath);
                        remainingImageSlots--;
                    }
                    else
                    {
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
