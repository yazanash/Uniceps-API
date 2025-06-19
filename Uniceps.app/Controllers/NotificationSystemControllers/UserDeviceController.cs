using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.UserDeviceDto;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.NotificationModels;

namespace Uniceps.app.Controllers.NotificationSystemControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDeviceController : ControllerBase
    {
        private readonly IDataService<UserDevice> _dataService;
        private readonly IUserQueryDataService<UserDevice> _userQueryDataService;
        private readonly IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto> _mapperExtension;
        public UserDeviceController(IDataService<UserDevice> dataService, IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto> mapperExtension, IUserQueryDataService<UserDevice> userQueryDataService)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _userQueryDataService = userQueryDataService;
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
            await _dataService.Create(device);
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

            IEnumerable<UserDevice> userDevices =  await _userQueryDataService.GetAllByUser(userId);
            return Ok(userDevices.Select(x=>_mapperExtension.ToDto(x)));
        }
    }
}
