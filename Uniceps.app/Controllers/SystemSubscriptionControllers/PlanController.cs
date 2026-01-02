using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PlanController : ControllerBase
    {
        private readonly IPlanDataService _dataService;
        private readonly IProductDataService _productDataService;
        private readonly IMembershipDataService _membershipDataService;
        private readonly IIntDataService<PlanItem> _planItemDataService;
        private readonly IMapperExtension<PlanModel, PlanDto, PlanCreationDto> _mapperExtension;
        private readonly IMapperExtension<PlanItem, PlanItemDto, PlanItemCreationDto> _planItemMapperExtension;
        private ILogger<PlanController> _logger;
        public PlanController(IPlanDataService dataService, IMapperExtension<PlanModel, PlanDto, PlanCreationDto> mapperExtension, ILogger<PlanController> logger, IIntDataService<PlanItem> planItemDataService, IMapperExtension<PlanItem, PlanItemDto, PlanItemCreationDto> planItemMapperExtension, IProductDataService productDataService, IMembershipDataService membershipDataService)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
            _logger = logger;
            _planItemDataService = planItemDataService;
            _planItemMapperExtension = planItemMapperExtension;
            _productDataService = productDataService;
            _membershipDataService = membershipDataService;
        }
        [HttpGet("[action]/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPlans(int productId)
        {
            IEnumerable<PlanModel> plans = await _dataService.GetPlansForApp(productId);
            return Ok(plans.Select(x => _mapperExtension.ToDto(x)).ToList());
        }
        [HttpGet("{target}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int target)
        {

            var product = await _productDataService.GetByAppId(target);
            IEnumerable<PlanModel> plans = await _dataService.GetPlansForApp(product.Id);
            if (User.Identity?.IsAuthenticated == true)
            {
                string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

                // هل المستخدم اشترك مسبقاً بهذا المنتج؟ (لمنع تكرار الـ IsFree)
                bool hasHistory = await _membershipDataService.HasUsedTrialForProduct(userId, product.Id);

                if (hasHistory)
                {
                    foreach (var plan in plans)
                    {
                        plan.PlanItems = plan.PlanItems.Where(i => !i.IsFree).ToList();
                    }
                    plans = plans.Where(p => p.PlanItems.Any()).ToList();
                }
            }
            return Ok(plans.Select(x=>_mapperExtension.ToDto(x)).ToList());
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (Guid.TryParse(id, out var planId))
            {
                PlanModel plan = await _dataService.Get(planId);
                return Ok( _mapperExtension.ToDto(plan));
            }
            else
                return NotFound();
           
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
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]  PlanCreationDto planCreationDto)
        {
            PlanModel plan = _mapperExtension.FromCreationDto(planCreationDto);
            plan.NID = id;
            var result = await _dataService.Update(plan);
            return Ok("Updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
        [HttpPost("Item")]
        public async Task<IActionResult> CreatePlanItem(PlanItemCreationDto planItemCreationDto)
        {
            if (planItemCreationDto == null)
                return BadRequest("Plan item data is missing.");

            PlanItem planItem = _planItemMapperExtension.FromCreationDto(planItemCreationDto);
            PlanItem result = await _planItemDataService.Create(planItem);
            _logger.LogInformation("Created Successfully");
            return Ok(_planItemMapperExtension.ToDto(result));

        }
        [HttpPut("Items/{id}")]
        public async Task<IActionResult> UpdatePlanItem(int id, [FromBody] PlanItemCreationDto planItemCreationDto)
        {
            PlanItem planItem = _planItemMapperExtension.FromCreationDto(planItemCreationDto);
            planItem.Id = id;
            var result = await _planItemDataService.Update(planItem);
            return Ok("Updated successfully");
        }
        [HttpDelete("Items/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _planItemDataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
