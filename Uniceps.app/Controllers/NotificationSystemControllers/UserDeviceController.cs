using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.UserDeviceDto;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Services.NotificationSystemServices;

namespace Uniceps.app.Controllers.NotificationSystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserDeviceController : ControllerBase
    {
        private readonly IUserDeviceDataService _dataService;
        private readonly IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto> _mapperExtension;
        public UserDeviceController(IUserDeviceDataService dataService, IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto> mapperExtension)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterDevice([FromBody] UserDeviceCreationDto dto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            UserDevice device = _mapperExtension.FromCreationDto(dto);
            device.UserId = userId;
            await _dataService.UpsertUserDeviceAsync(device);
            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            IEnumerable<UserDevice> userDevices =  await _dataService.GetAllByUser(userId);
            return Ok(userDevices.Select(x=>_mapperExtension.ToDto(x)));
        }
    }
}
