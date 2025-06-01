using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Controllers.RoutineControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MuscleGroupController : ControllerBase
    {
        private readonly ICommandDataService<MuscleGroup> _commandService;
        private readonly IQueryDataService<MuscleGroup> _queryService;
        IMapperExtension<MuscleGroup, MuscleGroupDto, MuscleGroupCreateDto> _mapper;
        public MuscleGroupController(IMapperExtension<MuscleGroup, MuscleGroupDto, MuscleGroupCreateDto> mapper, ICommandDataService<MuscleGroup> commandService, IQueryDataService<MuscleGroup> queryService)
        {
            _mapper = mapper;
            _commandService = commandService;
            _queryService = queryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<MuscleGroup> groups = await _queryService.GetAll();
            return Ok(groups.Select(x => _mapper.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(MuscleGroupCreateDto muscleGroupDto)
        {
            MuscleGroup muscleGroup = _mapper.FromCreationDto(muscleGroupDto);
            await _commandService.Create(muscleGroup);
            return Ok("Created successfully");
        }
        [HttpPut]
        public async Task<IActionResult> Update(MuscleGroup muscleGroup)
        {
            await _commandService.Update(muscleGroup);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(int id)
        {
            await _commandService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
