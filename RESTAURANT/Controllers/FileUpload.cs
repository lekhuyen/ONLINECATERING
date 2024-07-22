public static class FileUpload
{
    public static async Task<string> SaveImage(string folderName, IFormFile file)
    {
        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", folderName);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        // Ensure unique file name to prevent overwriting existing files
        var uniqueFileName = DateTime.Now.Ticks.ToString() + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        try
        {
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return the relative path of the saved file
            return Path.Combine(folderName, uniqueFileName).Replace("\\", "/");
        }
        catch (Exception ex)
        {
            // Handle exceptions (log or throw as needed)
            Console.WriteLine($"Error saving file: {ex.Message}");
            throw;
        }
    }

    public static void DeleteImage(string imagePath)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath);

        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
            }
            catch (Exception ex)
            {
                // Handle exceptions (log or throw as needed)
                Console.WriteLine($"Error deleting file: {ex.Message}");
                throw;
            }
        }
    }
}
