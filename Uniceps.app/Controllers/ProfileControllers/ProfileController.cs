using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.Controllers.RoutineControllers;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Controllers.ProfileControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IDataService<NormalProfile> _profileDataService;
        private readonly IDataService<BusinessProfile> _businseeDataService;
        private readonly IGetByUserId<NormalProfile> _getProfileByUserId;
        private readonly IGetByUserId<BusinessProfile> _getBusinessByUserId;
        IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto> _normalProfileMapperExtension;
        IMapperExtension<BusinessProfile, BusinessProfileDto, BusinessProfileCreationDto> _businessProfileMapperExtension;
        private ILogger<ProfileController> _logger;
        public ProfileController(IDataService<NormalProfile> profileDataService, IDataService<BusinessProfile> businseeDataService, ILogger<ProfileController> logger, IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto> normalProfileMapperExtension, IMapperExtension<BusinessProfile, BusinessProfileDto, BusinessProfileCreationDto> businessProfileMapperExtension, IGetByUserId<NormalProfile> getProfileByUserId, IGetByUserId<BusinessProfile> getBusinessByUserId)
        {
            _profileDataService = profileDataService;
            _businseeDataService = businseeDataService;
            _logger = logger;
            _normalProfileMapperExtension = normalProfileMapperExtension;
            _businessProfileMapperExtension = businessProfileMapperExtension;
            _getProfileByUserId = getProfileByUserId;
            _getBusinessByUserId = getBusinessByUserId;
        }

        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            NormalProfile normalProfile = await _getProfileByUserId.GetByUserId(userId);

            return Ok(_normalProfileMapperExtension.ToDto(normalProfile));
        }
        [HttpGet("business")]
        public async Task<IActionResult> GetBusinessProfile()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BusinessProfile businessProfile = await _getBusinessByUserId.GetByUserId(userId);

            return Ok(_businessProfileMapperExtension.ToDto(businessProfile));
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
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
        [HttpPost("business")]
        public async Task<IActionResult> CreateBusinessProfile(BusinessProfileCreationDto businessProfileCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (businessProfileCreationDto == null)
                return BadRequest("Exercise data is missing.");

            BusinessProfile profile = _businessProfileMapperExtension.FromCreationDto(businessProfileCreationDto);
            profile.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _businseeDataService.Create(profile);
            _logger.LogInformation("Created Successfully");
            return Ok(_businessProfileMapperExtension.ToDto(profile));
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
            NormalProfile normalProfile = await _getProfileByUserId.GetByUserId(userId);
            NormalProfile newProfile = _normalProfileMapperExtension.FromCreationDto(profileCreationDto);
            newProfile.Id = normalProfile.Id;
            newProfile.UserId = userId;
            await _profileDataService.Update(newProfile);
            return Ok("Updated successfully");
        }
        [HttpPut("business")]
        public async Task<IActionResult> UpdateBusiness([FromBody] BusinessProfileCreationDto businessProfileCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (businessProfileCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BusinessProfile businessProfile = await _getBusinessByUserId.GetByUserId(userId);
            BusinessProfile newProfile = _businessProfileMapperExtension.FromCreationDto(businessProfileCreationDto);
            newProfile.Id = businessProfile.Id;
            newProfile.UserId = userId;
            await _businseeDataService.Update(newProfile);
            return Ok("Updated successfully");
        }
    }
}
