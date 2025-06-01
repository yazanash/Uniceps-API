using MediatR;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.DayCommands;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Queries.RoutineQueries;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineDayController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogger<RoutineDayController> _logger;
        IMapperExtension<Day, DayDto, DayCreationDto> _mapper;
        public RoutineDayController(IMediator mediator, ILogger<RoutineDayController> logger, IMapperExtension<Day, DayDto, DayCreationDto> mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("routineId")]
        public async Task<IActionResult> GetAll(int routineId)
        {
            IEnumerable<Day> days = await _mediator.Send(new GetRoutineDaysQuery(routineId));
            return Ok(days.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(DayCreationDto dayCreationDto)
        {
            if (dayCreationDto == null)
                return BadRequest("day data is missing.");

            Day day = _mapper.FromCreationDto(dayCreationDto);
            var result = await _mediator.Send(new CreateRoutineDayCommand(day));
            _logger.LogInformation("Created Successfully");
            return Ok(_mapper.ToDto(day));
        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromBody] DayCreationDto dayCreationDto)
        {
            Day day = _mapper.FromCreationDto(dayCreationDto);
            day.Id = id;
            var result = await _mediator.Send(new UpdateRoutineDayCommand(day));
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRoutineDayCommand(id));
            return Ok("Deleted successfully");
        }
    }
}
