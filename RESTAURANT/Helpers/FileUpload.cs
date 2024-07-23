using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RESTAURANT.API.Helpers
{
    public class FileUpload
    {
        static readonly string baseFolder = "Uploads";
        static readonly string subFolder = "images"; // Adjust subfolder name as needed
        static readonly string rootUrl = "http://localhost:5265/";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImage(IFormFile formFile)
        {
            string imagesName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, baseFolder, subFolder);

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var exactPath = Path.Combine(imagePath, imagesName);

            using (var fileStream = new FileStream(exactPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            return Path.Combine(rootUrl, baseFolder, subFolder, imagesName).Replace("\\", "/");
        }

        public void DeleteImage(string imageUrl)
        {
            var exactPath = imageUrl.Replace(rootUrl, "").Replace("/", "\\");
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, exactPath);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
