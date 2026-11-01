using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Telegram.Bot.Types;
using Uniceps.app.Services;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Services.ProfileServices;

namespace Uniceps.app.Controllers.ProfileControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePictureController : ControllerBase
    {
        private readonly IProfileDataService _normalProfileDataService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ExerciseImageService _imageService;
        public ProfilePictureController(IProfileDataService normalProfileDataService, UserManager<AppUser> userManager, ExerciseImageService imageService)
        {
            _normalProfileDataService = normalProfileDataService;
            _userManager = userManager;
            _imageService = imageService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound("User not found.");

            NormalProfile normalProfile = await _normalProfileDataService.GetByUserId(user.Id);

            if (!string.IsNullOrEmpty(normalProfile.PictureUrl))
            {
                _imageService.DeleteImage(normalProfile.PictureUrl);
            }

            string fileNameWithoutExt = $"profile-{userId.Substring(0, Math.Min(userId.Length, 8))}-{Guid.NewGuid().ToString().Substring(0, 6)}";

            string imageUrl = await _imageService.SaveImageAsWebP(
                file,
                fileNameWithoutExtension: fileNameWithoutExt,
                folderName: "profile-pictures",
                maxWidth: 500
            );

            normalProfile.PictureUrl = imageUrl;
            await _normalProfileDataService.Update(normalProfile);

            return Ok(new { imageUrl });

        }
        [HttpGet]
        public async Task<IActionResult> GetProfilePicture()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            NormalProfile normalProfile = await _normalProfileDataService.GetByUserId(userId);

            if (!string.IsNullOrEmpty(normalProfile.PictureUrl))
            {
                var filePath = _imageService.GetImagePath(normalProfile.PictureUrl);

                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                return File(imageBytes, "image/webp");
            }
            return NotFound();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            NormalProfile normalProfile = await _normalProfileDataService.GetByUserId(userId);
            string? picUrl = normalProfile.PictureUrl;

            normalProfile.PictureUrl = null;
            await _normalProfileDataService.Update(normalProfile);

            if (!string.IsNullOrEmpty(picUrl))
            {
                _imageService.DeleteImage(picUrl);
            }

            return Ok();
        }
    }
}
