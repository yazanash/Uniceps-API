using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.Controllers.RoutineControllers;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.Services;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Services.ProfileServices;

namespace Uniceps.app.Controllers.ProfileControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IProfileDataService _profileDataService;
        IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto> _normalProfileMapperExtension;
        private ILogger<ProfileController> _logger;
        private readonly DataMigrationService _dataMigrationService;
        public ProfileController(IProfileDataService profileDataService, ILogger<ProfileController> logger, IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto> normalProfileMapperExtension, DataMigrationService dataMigrationService)
        {
            _profileDataService = profileDataService;
            _logger = logger;
            _normalProfileMapperExtension = normalProfileMapperExtension;
            _dataMigrationService = dataMigrationService;
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

            return Ok(_normalProfileMapperExtension.ToDto(normalProfile));
        }
      
        [HttpGet("id")]
        public async Task<IActionResult> GetById(Guid id)
        {
            NormalProfile normalProfile = await _profileDataService.Get(id);

            return Ok(_normalProfileMapperExtension.ToDto(normalProfile));
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

            NormalProfile profile = _normalProfileMapperExtension.FromCreationDto(profileCreationDto);
            profile.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _profileDataService.Create(profile);
            _logger.LogInformation("Created Successfully");
            return Ok(_normalProfileMapperExtension.ToDto(profile));
        }
        [HttpPut()]
        public async Task<IActionResult> Update([FromBody] NormalProfileCreationDto profileCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (profileCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            NormalProfile normalProfile = await _profileDataService.GetByUserId(userId);
            NormalProfile newProfile = _normalProfileMapperExtension.FromCreationDto(profileCreationDto);
            newProfile.NID = normalProfile.NID;
            newProfile.UserId = userId;
            await _profileDataService.Update(newProfile);
            return Ok(_normalProfileMapperExtension.ToDto(newProfile));
        }
    }
}
