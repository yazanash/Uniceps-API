using Microsoft.AspNetCore.Hosting;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace Uniceps.app.Services
{
    public class ExerciseImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ExerciseImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task SaveImageAsWebP(IFormFile file, string fileName)
        {
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "ExerciseImages");
            if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

            var filePath = Path.Combine(uploadsFolder, fileName);

            // إذا الملف موجود مسبقاً بنحذفه عشان نضمن التحديث
            if (System.IO.File.Exists(filePath)) System.IO.File.Delete(filePath);

            using (var image = await Image.LoadAsync(file.OpenReadStream()))
            {
                // تصغير العرض لـ 1080 بكسل مع الحفاظ على التناسب (إذا كانت الصورة ضخمة)
                if (image.Width > 1080)
                {
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(1080, 0),
                        Mode = ResizeMode.Max
                    }));
                }

                // تحويل وحفظ بصيغة WebP مع ضغط احترافي
                await image.SaveAsWebpAsync(filePath, new WebpEncoder
                {
                    Quality = 80, // جودة ممتازة وحجم صغير جداً
                    Method = WebpEncodingMethod.BestQuality
                });
            }
        }
    }
}
