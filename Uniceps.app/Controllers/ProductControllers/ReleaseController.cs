using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.ReleaseDtos;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Services.ProductServices;

namespace Uniceps.app.Controllers.ProductControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class ReleaseController : ControllerBase
    {
        private readonly IReleaseDataService _releaseDataService;
        private readonly IProductDataService _productDataService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ReleaseController(IReleaseDataService releaseDataService, IProductDataService productDataService, IWebHostEnvironment webHostEnvironment)
        {
            _releaseDataService = releaseDataService;
            _productDataService = productDataService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost("upload")]
        public async Task<IActionResult> UploadRelease(ReleaseCreationDto dto)
        {
            var release = new Release
            {
                Version = dto.Version,
                TargetOS = dto.TargetOS,
                DownloadSource = dto.DownloadSource,
                DownloadUrl = dto.DownloadUrl??"",
                ChangeLog = dto.ChangeLog,
                ProductId = dto.ProductId,
                CreatedAt = DateTime.UtcNow
            };

            await _releaseDataService.AddReleaseAsync(release);
            ReleaseDto releaseDto = new ReleaseDto()
            {
                Id = release.Id,
                Version = release.Version,
                TargetOS = release.TargetOS,
                DownloadSource = release.DownloadSource,
                TargetOSText = release.TargetOS.ToString(),
                DownloadSourceText = release.DownloadSource.ToString(),
                DownloadUrl = release.DownloadUrl ?? "",
                ChangeLog = release.ChangeLog,
                ProductId = release.ProductId,
                CreatedAt = DateTime.UtcNow
            };
            return Ok(releaseDto);
        }
        [HttpPost("upload-chunk")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UploadChunk([FromForm] ChunkUploadDto dto)
        {
            var tempFolder = Path.Combine(_webHostEnvironment.WebRootPath, "temp", dto.UploadId);
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var chunkPath = Path.Combine(tempFolder, $"{dto.ChunkIndex}.part");

            // 2. حفظ الـ Chunk الحالي
            using (var stream = new FileStream(chunkPath, FileMode.Create))
            {
                await dto.Chunk.CopyToAsync(stream);
            }

            // 3. التحقق إذا كان هذا هو الـ Chunk الأخير لبدء التجميع
            if (dto.ChunkIndex == dto.TotalChunks - 1)
            {
                try
                {
                    var finalFolder = Path.Combine(_webHostEnvironment.WebRootPath, "downloads", "releases");
                    if (!Directory.Exists(finalFolder)) Directory.CreateDirectory(finalFolder);

                    string originalFileName = string.IsNullOrWhiteSpace(dto.FileName)
                                                ? dto.UploadId
                                                : dto.FileName;

                    string cleanFileName = string.Join("_", originalFileName.Split(Path.GetInvalidFileNameChars()));

                    string fileNameOnly = Path.GetFileNameWithoutExtension(cleanFileName);
                    string extension = Path.GetExtension(cleanFileName);

                    string finalPath = Path.Combine(finalFolder, cleanFileName);
                    int count = 1;

                    while (System.IO.File.Exists(finalPath))
                    {
                        string tempFileName = $"{fileNameOnly}({count}){extension}";
                        finalPath = Path.Combine(finalFolder, tempFileName);
                        count++;
                    }

                    using (var finalStream = new FileStream(finalPath, FileMode.Create))
                    {
                        for (int i = 0; i < dto.TotalChunks; i++)
                        {
                            var partPath = Path.Combine(tempFolder, $"{i}.part");
                            if (System.IO.File.Exists(partPath))
                            {
                                using (var partStream = new FileStream(partPath, FileMode.Open))
                                {
                                    await partStream.CopyToAsync(finalStream);
                                }
                            }
                        }
                    }

                    if (Directory.Exists(tempFolder))
                        Directory.Delete(tempFolder, true);

                    var downloadUrl = $"/downloads/releases/{Path.GetFileName(finalPath)}";
                    return Ok(new { DownloadUrl = downloadUrl });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new { Message = "Error during file merging", Detail = ex.Message });
                }
            }

            return Ok(new { Message = $"Chunk {dto.ChunkIndex} uploaded successfully" });
        }

        [HttpGet("product/{productId}/latest")]
        public async Task<IActionResult> GetLatestReleases(int productId)
        {
            var release = await _releaseDataService.GetLatestReleasesAsync(productId);
            if (release == null) return NotFound("No release found");
            
            return Ok(release.Select(release =>  new ReleaseDto()
            {
                Id = release.Id,
                Version = release.Version,
                TargetOS = release.TargetOS,
                DownloadSource = release.DownloadSource,
                TargetOSText = release.TargetOS.ToString(),
                DownloadSourceText = release.DownloadSource.ToString(),
                DownloadUrl = release.DownloadUrl ?? "",
                ChangeLog = release.ChangeLog,
                ProductId = release.ProductId,
                CreatedAt = DateTime.UtcNow
            }));
        }
        [HttpGet("GetRelease/{id}")]
        public async Task<IActionResult> GetRelease(int id)
        {
            var release = await _releaseDataService.GetReleaseByIdAsync(id);
            if (release == null) return NotFound("No release found");

            return Ok( new ReleaseDto()
            {
                Id = release.Id,
                Version = release.Version,
                TargetOS = release.TargetOS,
                DownloadSource = release.DownloadSource,
                TargetOSText = release.TargetOS.ToString(),
                DownloadSourceText = release.DownloadSource.ToString(),
                DownloadUrl = release.DownloadUrl ?? "",
                ChangeLog = release.ChangeLog,
                ProductId = release.ProductId,
                CreatedAt = DateTime.UtcNow
            });
        }
        [HttpGet("app/{appId}/latest/{os}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestReleaseByOs(int appId, TargetOS os)
        {
            var product = await _productDataService.GetByAppId(appId);
            var release = await _releaseDataService.GetLatestReleaseAsync(product.Id, os);

            if (release == null)
                return NotFound($"No release found for product {product.Id} on {os}");

            // تحويل الـ Model إلى Dto لإرجاعه للـ Frontend
            var releaseDto = new ReleaseDto
            {
                Id = release.Id,
                Version = release.Version,
                TargetOS = release.TargetOS,
                TargetOSText = release.TargetOS.ToString(),
                DownloadSource =release.DownloadSource,
                DownloadSourceText = release.DownloadSource.ToString(),
                DownloadUrl = release.DownloadUrl ?? "",
                ChangeLog = release.ChangeLog,
                ProductId = release.ProductId,
                CreatedAt = release.CreatedAt
            };

            return Ok(releaseDto);
        }
        [HttpGet]

        public async Task<IActionResult> GetReleases([FromQuery] int productId)
        {
            var release = await _releaseDataService.GetReleasesByProductAsync(productId);
            if (release == null) return NotFound("No release found");

            return Ok(release.Select(release => new ReleaseDto()
            {
                Id = release.Id,
                Version = release.Version,
                TargetOS = release.TargetOS,
                DownloadSource = release.DownloadSource,
                TargetOSText = release.TargetOS.ToString(),
                DownloadSourceText = release.DownloadSource.ToString(),
                DownloadUrl = release.DownloadUrl ?? "",
                ChangeLog = release.ChangeLog,
                ProductId = release.ProductId,
                CreatedAt = DateTime.UtcNow
            }));
        }
        [HttpGet("download/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DownloadRelease(int id)
        {
            var release = await _releaseDataService.GetReleaseByIdAsync(id);
            if (release == null) return NotFound("Release not found");

            await _releaseDataService.LogDownloadAsync(
                id,
                HttpContext.Connection.RemoteIpAddress?.ToString(),
                Request.Headers["User-Agent"].ToString()
            );

            return Redirect(release.DownloadUrl);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRelease(int id)
        {
            var entityToDelete = await _releaseDataService.GetReleaseByIdAsync(id);
            if (entityToDelete != null)
            {
                DeletePhysicalFile(entityToDelete.DownloadUrl);
                 await _releaseDataService.DeleteReleaseAsync(id);
                return Ok();
            }
            return NotFound("Release not found");
        }
        private void DeletePhysicalFile(string downloadUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(downloadUrl)) return;

                if (downloadUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                var relativePath = downloadUrl.TrimStart('/');
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch 
            {
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRelease(int id,ReleaseCreationDto dto)
        {
           var release = new Release
           {
               Version = dto.Version,
               TargetOS = dto.TargetOS,
               DownloadSource = dto.DownloadSource,
               DownloadUrl = dto.DownloadUrl ?? "",
               ChangeLog = dto.ChangeLog,
               ProductId = dto.ProductId,
               CreatedAt = DateTime.UtcNow,
               Id = id
           };

           
            await _releaseDataService.UpdateReleaseAsync(release);
            ReleaseDto releaseDto = new ReleaseDto()
            {
                Id = release.Id,
                Version = release.Version,
                TargetOS = release.TargetOS,
                DownloadSource = release.DownloadSource,
                TargetOSText = release.TargetOS.ToString(),
                DownloadSourceText = release.DownloadSource.ToString(),
                DownloadUrl = release.DownloadUrl ?? "",
                ChangeLog = release.ChangeLog,
                ProductId = release.ProductId,
                CreatedAt = DateTime.UtcNow
            };
            return Ok(releaseDto);
        }
    }
}
