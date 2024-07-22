public static class FileUpload
{
    public static async Task<string> SaveImage(string folderName, IFormFile file)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
    }

    public static void DeleteImage(string imagePath)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }
}
