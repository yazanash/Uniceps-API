using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.PayGatewayDtos;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.app.HostBuilder;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly IIntDataService<PaymentGateway> _paymentGatewayService;
        private readonly IMapperExtension<PaymentGateway, PaymentGatewayDto, PaymentGatewayCreationDto> _mapperExtension;
        public PaymentGatewayController(IIntDataService<PaymentGateway> paymentGatewayService, IMapperExtension<PaymentGateway, PaymentGatewayDto, PaymentGatewayCreationDto> mapperExtension)
        {
            _paymentGatewayService = paymentGatewayService;
            _mapperExtension = mapperExtension;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<PaymentGateway> gateways = await _paymentGatewayService.GetAll();
            return Ok(gateways.Select(x => _mapperExtension.ToDto(x)).ToList());
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            PaymentGateway paymentGateway = await _paymentGatewayService.Get(id);

            return Ok(_mapperExtension.ToDto(paymentGateway));
        }
        [HttpPost]
        public async Task<IActionResult> CreatePaymentGateway(PaymentGatewayCreationDto paymentGatewayCreationDto)
        {
            if (paymentGatewayCreationDto == null)
                return BadRequest("Exercise data is missing.");

            PaymentGateway paymentGateway = _mapperExtension.FromCreationDto(paymentGatewayCreationDto);
            var result = await _paymentGatewayService.Create(paymentGateway);
            return Ok(_mapperExtension.ToDto(paymentGateway));
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePaymentGateway(int id , [FromBody] PaymentGatewayCreationDto paymentGatewayCreationDto)
        {
            if (paymentGatewayCreationDto == null)
                return BadRequest("Exercise data is missing.");

            PaymentGateway paymentGateway = _mapperExtension.FromCreationDto(paymentGatewayCreationDto);
            paymentGateway.Id = id;
            var result = await _paymentGatewayService.Update(paymentGateway);
            return Ok(_mapperExtension.ToDto(paymentGateway));
        }
    }
}
