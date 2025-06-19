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
        private readonly IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto> _mapperExtension;
        public UserDeviceController(IDataService<UserDevice> dataService, IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto> mapperExtension)
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
            await _dataService.Create(device);
            return Ok();
        }
    }
}
