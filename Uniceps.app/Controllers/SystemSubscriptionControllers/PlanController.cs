using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanController : ControllerBase
    {
        private readonly IDataService<PlanModel> _dataService;
        private readonly IGetByTargetType<PlanModel> _getByTargetType;
        private readonly IMapperExtension<PlanModel, PlanDto, PlanCreationDto> _mapperExtension;
        private ILogger<PlanController> _logger;
        public PlanController(IDataService<PlanModel> dataService, IMapperExtension<PlanModel, PlanDto, PlanCreationDto> mapperExtension, ILogger<PlanController> logger, IGetByTargetType<PlanModel> getByTargetType)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _logger = logger;
            _getByTargetType = getByTargetType;
        }

        [HttpGet("target")]
        public async Task<IActionResult> GetAll(int target)
        {
            IEnumerable<PlanModel> plans = await _getByTargetType.GetAllByTarget(target);
            return Ok(plans.Select(x=>_mapperExtension.ToDto(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(PlanCreationDto planCreationDto)
        {
            if (planCreationDto == null)
                return BadRequest("Plan data is missing.");

            PlanModel plan = _mapperExtension.FromCreationDto(planCreationDto);
            PlanModel result = await _dataService.Create(plan);
            _logger.LogInformation("Created Successfully");
            return Ok(_mapperExtension.ToDto(result));
        }
        [HttpPut("id")]
        public async Task<IActionResult> Update(Guid id, [FromBody]  PlanCreationDto planCreationDto)
        {
            PlanModel plan = _mapperExtension.FromCreationDto(planCreationDto);
            plan.NID = id;
            var result = await _dataService.Update(plan);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
