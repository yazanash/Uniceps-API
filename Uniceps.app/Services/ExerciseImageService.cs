using ImageMagick;

namespace Uniceps.app.Services
{
    public class ExerciseImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExerciseImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsWebP(IFormFile file, string fileNameWithoutExtension, string folderName = "ExerciseImages",int maxWidth = 1080)
        {
            if (file == null || file.Length == 0) return string.Empty;

            var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(rootPath, folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{fileNameWithoutExtension}.webp";
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            using var inputStream = file.OpenReadStream();

            using var image = new MagickImage(inputStream);

            if (image.Width > maxWidth)
            {
                image.Resize((uint)maxWidth, 0);
            }

            image.Format = MagickFormat.WebP;
            image.Quality = 80;

            await image.WriteAsync(filePath);

            return $"/{folderName}/{fileName}";
        }
        public string GetImagePath(string imagePath)
        {
            var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            imagePath = imagePath.TrimStart('/', '\\');
            var filePath = Path.Combine(rootPath, imagePath);
            return filePath;
        }
        public void DeleteImage(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return;

            var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            imagePath = imagePath.TrimStart('/', '\\');
            var fullPath = Path.Combine(rootPath, imagePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}
