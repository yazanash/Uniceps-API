using Google.Api.Gax.ResourceNames;
using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;
using Telegram.Bot.Types;

namespace Uniceps.app.Services
{
    public class ExerciseImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExerciseImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string> SaveImageAsWebP(IFormFile file, string fileNameWithoutExtension, string folderName = "ExerciseImages")
        {
            var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            var uploadsFolder = Path.Combine(rootPath, folderName);
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{fileNameWithoutExtension}.webp";
            var filePath = Path.Combine(uploadsFolder, fileName);

            if (File.Exists(filePath)) File.Delete(filePath);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                if (image.Width > 1080)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(1080, 0),
                        Mode = ResizeMode.Max
                    }));
                }

                await image.SaveAsWebpAsync(filePath, new WebpEncoder
                {
                    Quality = 80,
                    Method = WebpEncodingMethod.BestQuality
                });
            }
            return $"{folderName}/{fileName}";
        }
        public string GetImagePath(string imagePath)
        {
            var rootPath = _webHostEnvironment.WebRootPath ?? Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot");
            var filePath = Path.Combine(rootPath, imagePath);
            return filePath;
        }
    }
}
