
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.Controllers.MeasurementControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BodyMeasurementController : ControllerBase
    {
        private readonly IIntDataService<BodyMeasurement> _dataService;
        private readonly IUserQueryDataService<BodyMeasurement> _userQueryDataService;
        public BodyMeasurementController(IIntDataService<BodyMeasurement> dataService, IUserQueryDataService<BodyMeasurement> userQueryDataService)
        {
            _dataService = dataService;
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
            return Ok(players.Select(x =>new BodyMeasurementDto(x)).ToList());
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
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BodyMeasurement bodyMeasurement = bodyMeasurementCreationDto.ToModel();
            bodyMeasurement.UserId = userId;
            var result = await _dataService.Create(bodyMeasurement);
            return Ok(new BodyMeasurementDto(bodyMeasurement));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int Id, [FromBody] BodyMeasurementCreationDto bodyMeasurementCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
           
            if (bodyMeasurementCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BodyMeasurement bodyMeasurement = await _dataService.Get(Id);
            BodyMeasurement newBodyMeasurement  = bodyMeasurementCreationDto.ToModel();
            newBodyMeasurement.MId = bodyMeasurement.MId;
            newBodyMeasurement.UserId = userId;
            await _dataService.Update(newBodyMeasurement);
            return Ok("Updated successfully");
        }
       
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            bool result = await _dataService.Delete(id, userId);
            if (result)
                return Ok();
            return BadRequest();

        }
    }
}
