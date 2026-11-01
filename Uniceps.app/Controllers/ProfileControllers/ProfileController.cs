using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Services.ProfileServices;

namespace Uniceps.app.Controllers.ProfileControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileDataService _profileDataService;
        private ILogger<ProfileController> _logger;
        public ProfileController(IProfileDataService profileDataService, ILogger<ProfileController> logger)
        {
            _profileDataService = profileDataService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            NormalProfile normalProfile = await _profileDataService.GetByUserId(userId);

            return Ok(new NormalProfileDto(normalProfile));
        }
      
        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            NormalProfile normalProfile = await _profileDataService.Get(id);

            return Ok(new NormalProfileDto(normalProfile));
        }
        [HttpPost]
        public async Task<IActionResult> CreateNormalProfile(NormalProfileCreationDto profileCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (profileCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            NormalProfile existedProfile = await _profileDataService.GetByUserId(userId);
            NormalProfile newProfile =profileCreationDto.ToModel();
            newProfile.UserId = userId;
            if (existedProfile != null)
            {
                newProfile.NID = existedProfile.NID;
                await _profileDataService.Update(newProfile);
                _logger.LogInformation("Updated Successfully");
                return Ok(new NormalProfileDto(newProfile));
            }
            else
            {
                var result = await _profileDataService.Create(newProfile);
                _logger.LogInformation("Created Successfully");
                return Ok(new NormalProfileDto(newProfile));
            }
         
        }
    }
}
