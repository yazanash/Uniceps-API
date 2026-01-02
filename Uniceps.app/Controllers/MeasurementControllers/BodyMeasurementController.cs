using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.app.Helpers;
using Uniceps.app.HostBuilder;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.Controllers.MeasurementControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodyMeasurementController : ControllerBase
    {
        private readonly IDataService<BodyMeasurement> _dataService;
        private readonly IUserQueryDataService<BodyMeasurement> _userQueryDataService;
        private readonly IMapperExtension<BodyMeasurement, BodyMeasurementDto, BodyMeasurementCreationDto> _mapperExtension;
        public BodyMeasurementController(IDataService<BodyMeasurement> dataService, IMapperExtension<BodyMeasurement, BodyMeasurementDto, BodyMeasurementCreationDto> mapperExtension, IUserQueryDataService<BodyMeasurement> userQueryDataService)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _userQueryDataService = userQueryDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<BodyMeasurement> players = await _userQueryDataService.GetAllByUser(userId);
            return Ok(players.Select(x => _mapperExtension.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(BodyMeasurementCreationDto bodyMeasurementCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
           
            if (bodyMeasurementCreationDto == null)
                return BadRequest("Body Measurment data is missing.");

            BodyMeasurement bodyMeasurement = _mapperExtension.FromCreationDto(bodyMeasurementCreationDto);

            bodyMeasurement.BusinessId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(bodyMeasurement);
            return Ok(_mapperExtension.ToDto(bodyMeasurement));
        }
        [HttpPut("Id")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] BodyMeasurementCreationDto bodyMeasurementCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
           
            if (bodyMeasurementCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BodyMeasurement bodyMeasurement = await _dataService.Get(Id);
            BodyMeasurement newBodyMeasurement  = _mapperExtension.FromCreationDto(bodyMeasurementCreationDto);
            newBodyMeasurement.Id = bodyMeasurement.Id;
            //newPlayerModel.UserId = userId;
            await _dataService.Update(newBodyMeasurement);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
