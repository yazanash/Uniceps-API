using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Commands.DayCommands;
using Uniceps.Entityframework.Commands.RoutineItemCommands;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Queries.RoutineQueries;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoutineItemController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<RoutineItemController> _logger;
        IMapperExtension<RoutineItem, RoutineItemDto, RoutineItemCreationDto> _mapper;

        public RoutineItemController(IMediator mediator, ILogger<RoutineItemController> logger, IMapperExtension<RoutineItem, RoutineItemDto, RoutineItemCreationDto> mapper)
        {
            _mediator = mediator;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("dayId")]
        public async Task<IActionResult> GetAll(int dayId)
        {
            IEnumerable<RoutineItem> routineItems = await _mediator.Send(new GetRoutineItemsQuery(dayId));
            return Ok(routineItems.Select(x => _mapper.ToDto(x)));
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoutineItemCreationDto routineItemDto)
        {
            RoutineItem routineItem = _mapper.FromCreationDto(routineItemDto);
            RoutineItem createdItem = await _mediator.Send(new CreateRoutineItemCommand(routineItem));
            return Ok(_mapper.ToDto(createdItem));

        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(int id, [FromBody] RoutineItemCreationDto creationDto)
        {
            RoutineItem item = _mapper.FromCreationDto(creationDto);
            item.Id = id;
            var result = await _mediator.Send(new UpdateRoutineItemCommand(item));
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteRoutineItemCommand(id));
            return Ok("Deleted successfully");
        }
    }
}
