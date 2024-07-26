using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RESTAURANT.API.Helpers
{
    public class FileUpload
    {
        static readonly string baseFolder = "Uploads"; // Directory outside wwwroot
        static readonly string rootUrl = "http://localhost:5265/";
        //static readonly string rootUrl = "http://localhost:5246/";

        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUpload(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImage(string subFolder, IFormFile formFile)
        {
            string imagesName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, subFolder);

            if (!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }

            var exactPath = Path.Combine(imagePath, imagesName);

            using (var fileStream = new FileStream(exactPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }

            // If you need to generate a URL to access the file, you would need to handle routing differently
            return Path.Combine(rootUrl,baseFolder, subFolder, imagesName).Replace("\\", "/");
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
