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

                // Update title and content
                existingAbout.Title = about.Title;
                existingAbout.Content = about.Content;

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
                else if (existingAbout.ImagePaths != null && existingAbout.ImagePaths.Count > 0)
                {
                    // Maintain existing images if no new images are provided
                    about.ImagePaths = existingAbout.ImagePaths;
                }

                // Replace the entire document in MongoDB
                ReplaceOptions options = new ReplaceOptions { IsUpsert = true };
                await _dbContext.About.ReplaceOneAsync(a => a.Id == id, existingAbout, options);

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



        /*[HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] Product? product
    , List<IFormFile>? formFiles) // List<int>? idsDelete de xoa nhieu hinh anh ben UI
        {
            try
            {
                var existedProduct = await _dbContext.Products
                    .Include(p => p.ProductImages)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if (existedProduct == null)
                {
                    return NotFound(new
                    {
                        status = 400,
                        message = "Product is not found",
                        data = existedProduct,
                    });
                }
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (formFiles?.Count() > 0)
                {
                    // xoa hinh anh trong folder
                    foreach (var item in existedProduct.ProductImages)
                    {
                        if (!string.IsNullOrEmpty(item.ImageUrl))
                        {
                            FileUpload.DeleteImage(item.ImageUrl);
                            //_dbContext.Products.Remove(product);
                        }
                    }
                    // product.ProductImages.Clear();
                    // them hinh anh trong folder & DB
                    //productExisted.ProductImages.Clear();
                    foreach (var item in formFiles)
                    {
                        var imagePath = await FileUpload.SaveImage("productImages", item);
                        var productImage = new ProductImages
                        {
                            ImageUrl = imagePath,
                            ProductId = id
                        };
                        await _dbContext.ProductImages.AddAsync(productImage);
                    }
                }
                // k can user nhap but set ID de biet update tai ID nao
                product.Id = id;
                _dbContext.Entry(existedProduct).CurrentValues.SetValues(product);
                await _dbContext.SaveChangesAsync();
                return Ok(new
                {
                    status = 200,
                    message = "Update product successfully",
                    data = existedProduct
                });
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }*/

    }
}
