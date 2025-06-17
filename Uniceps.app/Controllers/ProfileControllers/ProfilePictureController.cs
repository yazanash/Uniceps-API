using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.Controllers.ProfileControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilePictureController : ControllerBase
    {
        private readonly IDataService<NormalProfile> _normalProfileDataService;
        private readonly IDataService<BusinessProfile> _businessProfileDataService;
        private readonly IGetByUserId<NormalProfile> _getNormalProfileByUserId;
        private readonly IGetByUserId<BusinessProfile> _getBusinessProfileByUserId;
        private readonly UserManager<AppUser> _userManager;

        public ProfilePictureController(IDataService<NormalProfile> normalProfileDataService,
            IDataService<BusinessProfile> businessProfileDataService, IGetByUserId<NormalProfile> getNormalProfileByUserId,
            IGetByUserId<BusinessProfile> getBusinessProfileByUserId, UserManager<AppUser> userManager)
        {
            _normalProfileDataService = normalProfileDataService;
            _businessProfileDataService = businessProfileDataService;
            _getNormalProfileByUserId = getNormalProfileByUserId;
            _getBusinessProfileByUserId = getBusinessProfileByUserId;
            _userManager = userManager;
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

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile-pictures");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imageUrl = $"/profile-pictures/{fileName}";

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if(user!.UserType == UserType.Normal)
            {
                NormalProfile normalProfile = await _getNormalProfileByUserId.GetByUserId(user.Id);
                normalProfile.PictureUrl = imageUrl;
                await _normalProfileDataService.Update(normalProfile);
            }
            else
            {
                BusinessProfile businessProfile= await _getBusinessProfileByUserId.GetByUserId(user.Id);
                businessProfile.PictureUrl = imageUrl;
                await _businessProfileDataService.Update(businessProfile);

            }

            return Ok(new { imageUrl });
        }
        [HttpGet("profile-picture/{fileName}")]
        public IActionResult GetProfilePicture(string fileName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/profile-pictures", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var contentType = "image/" + Path.GetExtension(filePath).TrimStart('.');
            var imageBytes = System.IO.File.ReadAllBytes(filePath);
            return File(imageBytes, contentType);
        }
    }
}
