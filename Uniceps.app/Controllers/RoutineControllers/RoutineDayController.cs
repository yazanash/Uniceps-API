using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineDayController : ControllerBase
    {
        private readonly IDataService<Day> _dataService;
        private readonly IEntityQueryDataService<Day> _entityQueryDataService;
        private ILogger<RoutineDayController> _logger;
        IMapperExtension<Day, DayDto, DayCreationDto> _mapper;
        public RoutineDayController(ILogger<RoutineDayController> logger, IMapperExtension<Day, DayDto, DayCreationDto> mapper, IDataService<Day> dataService, IEntityQueryDataService<Day> entityQueryDataService)
        {
            _logger = logger;
            _mapper = mapper;
            _dataService = dataService;
            _entityQueryDataService = entityQueryDataService;
        }
        [HttpGet("routineId")]
        public async Task<IActionResult> GetAll(int routineId)
        {
            IEnumerable<Day> days = await _entityQueryDataService.GetAllById(routineId);
            return Ok(days.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(DayCreationDto dayCreationDto)
        {
            if (dayCreationDto == null)
                return BadRequest("day data is missing.");

            Day day = _mapper.FromCreationDto(dayCreationDto);
            var result = await _dataService.Create(day);
            _logger.LogInformation("Created Successfully");
            return Ok(_mapper.ToDto(day));
        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromBody] DayCreationDto dayCreationDto)
        {
            Day day = _mapper.FromCreationDto(dayCreationDto);
            day.Id = id;
            var result = await _dataService.Update(day);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
