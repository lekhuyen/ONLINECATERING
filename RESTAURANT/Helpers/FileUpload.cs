namespace RESTAURANT.API.Helpers
{
    public class FileUpload
    {
        static readonly string baseFolder = "Uploads";
        static readonly string rootUrl = "http://localhost:5265/";

        public static async Task<string> SaveImage(string subFolder, IFormFile formFile)
        {
            string imagesName = Guid.NewGuid().ToString() + "_" + formFile.FileName;
            var imagePath = Path.Combine(Directory.GetCurrentDirectory(), baseFolder, subFolder);
            if(!Directory.Exists(imagePath))
            {
                Directory.CreateDirectory(imagePath);
            }
            var exacPath = Path.Combine(imagePath, imagesName);
            using(var fileStream = new FileStream(exacPath, FileMode.Create))
            {
                await formFile.CopyToAsync(fileStream);
            }
            return Path.Combine(rootUrl, baseFolder,subFolder, imagesName).Replace("\\", "/");
        }
        public static void DeleteImage(string imagePath)
        {
            var exacPath = imagePath.Substring(rootUrl.Length);
            var filePath = Path.Combine(exacPath);
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
