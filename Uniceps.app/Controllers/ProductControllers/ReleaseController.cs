using Microsoft.AspNetCore.Authorization;
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
        public ReleaseController(IReleaseDataService releaseDataService, IProductDataService productDataService)
        {
            _releaseDataService = releaseDataService;
            _productDataService = productDataService;
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
            var tempFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", dto.UploadId);
            if (!Directory.Exists(tempFolder))
                Directory.CreateDirectory(tempFolder);

            var chunkPath = Path.Combine(tempFolder, $"{dto.ChunkIndex}.part");

            using (var stream = new FileStream(chunkPath, FileMode.Create))
            {
                await dto.Chunk.CopyToAsync(stream);
            }

            if (dto.ChunkIndex == dto.TotalChunks - 1)
            {
                var finalFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "downloads", "releases");
                if (!Directory.Exists(finalFolder))
                    Directory.CreateDirectory(finalFolder);

                var finalPath = Path.Combine(finalFolder, $"{dto.UploadId}_{DateTime.UtcNow.Ticks}.exe");

                using (var finalStream = new FileStream(finalPath, FileMode.Create))
                {
                    for (int i = 0; i < dto.TotalChunks; i++)
                    {
                        var partPath = Path.Combine(tempFolder, $"{i}.part");
                        var bytes = await System.IO.File.ReadAllBytesAsync(partPath);
                        await finalStream.WriteAsync(bytes, 0, bytes.Length);
                    }
                }

                Directory.Delete(tempFolder, true);

                // توليد رابط التحميل
                var downloadUrl = $"/downloads/releases/{Path.GetFileName(finalPath)}";

                return Ok(new { DownloadUrl = downloadUrl });
            }

            return Ok(new { Message = $"Chunk {dto.ChunkIndex} uploaded" });
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
        [HttpGet("app/{appId}/latest/{os}")]
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
            var deleted = await _releaseDataService.DeleteReleaseAsync(id);
            if (!deleted) return NotFound("Release not found");
            return Ok();
        }
    }
}
