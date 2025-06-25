using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessSubscriptionsDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Controllers.BusinessLocalControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessSubscriptionController : ControllerBase
    {
        private readonly IDataService<BusinessSubscriptionModel> _dataService;
        private readonly IMapperExtension<BusinessSubscriptionModel, BusinessSubscriptionDto, BusinessSubscriptionCreationDto> _mapperExtension;
        public BusinessSubscriptionController(IDataService<BusinessSubscriptionModel> dataService, IMapperExtension<BusinessSubscriptionModel, BusinessSubscriptionDto, BusinessSubscriptionCreationDto> mapperExtension)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusinessSubscriptionCreationDto businessSubscriptionCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (businessSubscriptionCreationDto == null)
                return BadRequest("Exercise data is missing.");

            BusinessSubscriptionModel subscriptionModel = _mapperExtension.FromCreationDto(businessSubscriptionCreationDto);

            subscriptionModel.BusinessId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(subscriptionModel);
            return Ok(_mapperExtension.ToDto(result));
        }
        [HttpPut("subscriptionId")]
        public async Task<IActionResult> Update(int subscriptionId, [FromBody] BusinessSubscriptionCreationDto businessSubscriptionCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (businessSubscriptionCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BusinessSubscriptionModel subscriptionModel = await _dataService.Get(subscriptionId);
            BusinessSubscriptionModel newSubscriptionModel = _mapperExtension.FromCreationDto(businessSubscriptionCreationDto);
            newSubscriptionModel.Id = subscriptionModel.Id;
            //newPlayerModel.UserId = userId;
            await _dataService.Update(newSubscriptionModel);
            return Ok("Updated successfully");
        }
    }
}
