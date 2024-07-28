using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace INFORMATION.API.Helper
{
    public class FileUpload
    {
        static readonly string baseFolder = "Uploads"; // Directory inside wwwroot or elsewhere
        static readonly string rootUrl = "http://localhost:5034/";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImage(string subFolder, IFormFile formFile)
        {
            // Ensure the base folder path is inside wwwroot
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, subFolder);

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            // Create a unique filename
            var fileName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var filePath = Path.Combine(imagePath, fileName);

            // Save the file
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            // Return the URL to access the file
            return Path.Combine(baseFolder, subFolder, fileName).Replace("\\", "/");
        }

        public void DeleteImage(string imagePath)
        {
            var exactPath = imagePath.Substring(rootUrl.Length);
            var filePath = Path.Combine(exactPath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
