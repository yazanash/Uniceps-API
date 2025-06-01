using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.RoutineCommands;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Queries.RoutineQueries;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineController : ControllerBase
    {
        private readonly IMediator _mediator;
        private ILogger<RoutineController> _logger;
        IMapperExtension<Routine, RoutineDto, RoutineCreationDto> _mapper;
        public RoutineController(IMediator mediator, ILogger<RoutineController> logger, IMapperExtension<Routine, RoutineDto, RoutineCreationDto> mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Routine> routines = await _mediator.Send(new GetAllRoutineQuery());
            return Ok(routines.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            Routine routine = await _mediator.Send(new GetRoutineByIdQuery(id));
            return Ok(_mapper.ToDto(routine));
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoutineCreationDto routineCreationDto)
        {
            if (routineCreationDto == null)
                return BadRequest("Exercise data is missing.");

            Routine routine = _mapper.FromCreationDto(routineCreationDto);
            var result = await _mediator.Send(new CreateRoutineCommand(routine));
            _logger.LogInformation("Created Successfully");
            return Ok(_mapper.ToDto(routine));
        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromBody] RoutineCreationDto routineCreationDto)
        {
            Routine routine = _mapper.FromCreationDto(routineCreationDto);
            routine.Id = id;
            var result = await _mediator.Send(new UpdateRoutineCommand(routine));
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRoutineCommand(id));
            return Ok("Deleted successfully");
        }
    }
}
