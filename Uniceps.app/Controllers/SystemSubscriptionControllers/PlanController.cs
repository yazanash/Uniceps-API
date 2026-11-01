using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Core.Services;
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
        private ILogger<PlanController> _logger;
        public PlanController(IPlanDataService dataService, ILogger<PlanController> logger, IProductDataService productDataService, IMembershipDataService membershipDataService)
        {
            _dataService = dataService;
            _logger = logger;
            _productDataService = productDataService;
            _membershipDataService = membershipDataService;
        }
        [HttpGet("[action]/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPlans(int productId)
        {
            IEnumerable<PlanItem> plans = await _dataService.GetPlansForApp(productId);
            return Ok(plans.Select(x =>new PlanItemDto(x)).ToList());
        }
        [HttpGet("{target}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll(int target)
        {

            var product = await _productDataService.GetByAppId(target);
            IEnumerable<PlanItem> plans = await _dataService.GetPlansForApp(product.Id);
            if (User.Identity?.IsAuthenticated == true)
            {
                string userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

                bool hasHistory = await _membershipDataService.HasUsedTrialForProduct(userId, product.Id);

                if (hasHistory)
                {
                    plans = plans.Where(p => !p.IsFree).ToList();
                }
            }
            return Ok(plans.Select(x => new PlanItemDto(x)).ToList());
        }
        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            PlanItem plan = await _dataService.Get(id);
            return Ok(new PlanItemDto(plan));
        }
        [HttpPost]
        public async Task<IActionResult> Create(PlanItemCreationDto planCreationDto)
        {
            if (planCreationDto == null)
                return BadRequest("Plan data is missing.");

            PlanItem plan = planCreationDto.ToModel();
            PlanItem result = await _dataService.Create(plan);
            _logger.LogInformation("Created Successfully");
            return Ok(new PlanItemDto(result));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PlanItemCreationDto planCreationDto)
        {
            PlanItem plan = planCreationDto.ToModel();
            plan.Id = id;
            var result = await _dataService.Update(plan);
            return Ok("Updated successfully");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
