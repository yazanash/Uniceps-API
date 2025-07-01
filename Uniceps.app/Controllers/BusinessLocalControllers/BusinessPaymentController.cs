using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessPaymentDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Controllers.BusinessLocalControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessPaymentController : ControllerBase
    {
        private readonly IDataService<BusinessPaymentModel> _dataService;
        private readonly IMapperExtension<BusinessPaymentModel, BusinessPaymentDto, BusinessPaymentCreationDto> _mapperExtension;

        public BusinessPaymentController(IDataService<BusinessPaymentModel> dataService, IMapperExtension<BusinessPaymentModel, BusinessPaymentDto, BusinessPaymentCreationDto> mapperExtension)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusinessPaymentCreationDto businessPaymentCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (businessPaymentCreationDto == null)
                return BadRequest("Exercise data is missing.");

            BusinessPaymentModel businessPayment = _mapperExtension.FromCreationDto(businessPaymentCreationDto);
            businessPayment.BusinessId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(businessPayment);
            return Ok(_mapperExtension.ToDto(businessPayment));
        }
    }
}
