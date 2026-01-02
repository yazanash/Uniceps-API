using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.Controllers.MeasurementControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkoutController : ControllerBase
    {
        private readonly IIntDataService<WorkoutSession> _dataService;
        private readonly IUserQueryDataService<WorkoutSession> _userQueryDataService;
        private readonly IMapperExtension<WorkoutSession, WorkoutSessionDto, WorkoutSessionCreationDto> _mapperExtension;

        public WorkoutController(IIntDataService<WorkoutSession> dataService, IUserQueryDataService<WorkoutSession> userQueryDataService, IMapperExtension<WorkoutSession, WorkoutSessionDto, WorkoutSessionCreationDto> mapperExtension)
        {
            _dataService = dataService;
            _userQueryDataService = userQueryDataService;
            _mapperExtension = mapperExtension;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
           
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<WorkoutSession> players = await _userQueryDataService.GetAllByUser(userId);
            return Ok(players.Select(x => _mapperExtension.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(WorkoutSessionCreationDto bodyMeasurementCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
         
            if (bodyMeasurementCreationDto == null)
                return BadRequest("Workout session data is missing.");

            WorkoutSession bodyMeasurement = _mapperExtension.FromCreationDto(bodyMeasurementCreationDto);

            bodyMeasurement.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(bodyMeasurement);
            return Ok(new WorkoutSessionIdDto() { Id= result.Id});
        }
        [HttpPut("Id")]
        public async Task<IActionResult> Update(int Id, [FromBody] WorkoutSessionCreationDto bodyMeasurementCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
          
            if (bodyMeasurementCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            WorkoutSession bodyMeasurement = await _dataService.Get(Id);
            WorkoutSession newBodyMeasurement = _mapperExtension.FromCreationDto(bodyMeasurementCreationDto);
            newBodyMeasurement.Id = bodyMeasurement.Id;
            //newPlayerModel.UserId = userId;
            await _dataService.Update(newBodyMeasurement);
            return Ok("Updated successfully");
        }
        [HttpDelete("Id")]
        public async Task<IActionResult> Delete(int id)
        {
            await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
