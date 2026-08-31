using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Telegram.Bot.Types;
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
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProfilePictureController(IProfileDataService normalProfileDataService, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _normalProfileDataService = normalProfileDataService;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
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

            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile-pictures");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imageUrl = fileName;

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                NormalProfile normalProfile = await _normalProfileDataService.GetByUserId(user.Id);
                normalProfile.PictureUrl = imageUrl;
                await _normalProfileDataService.Update(normalProfile);
            }

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
            if (normalProfile.PictureUrl != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile-pictures");
                var filePath = Path.Combine(uploadsFolder, normalProfile.PictureUrl);
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                var contentType = "image/" + Path.GetExtension(filePath).TrimStart('.');
                var imageBytes = System.IO.File.ReadAllBytes(filePath);
                return File(imageBytes, contentType);
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
            if (picUrl != null)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "profile-pictures");
                var filePath = Path.Combine(uploadsFolder, picUrl);
                if (System.IO.File.Exists(filePath))
                    System.IO.File.Delete(filePath);
            }
            return Ok();
        }
    }
}
