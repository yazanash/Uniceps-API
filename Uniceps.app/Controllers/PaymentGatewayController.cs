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
        private readonly IWebHostEnvironment _env;
        public PaymentGatewayController(IIntDataService<PaymentGateway> paymentGatewayService, IMapperExtension<PaymentGateway, PaymentGatewayDto, PaymentGatewayCreationDto> mapperExtension, IWebHostEnvironment env)
        {
            _paymentGatewayService = paymentGatewayService;
            _mapperExtension = mapperExtension;
            _env = env;
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
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> CreatePaymentGateway([FromForm] PaymentGatewayCreationDto paymentGatewayCreationDto)
        {
            try
            {
                if (paymentGatewayCreationDto == null) return BadRequest("Data is missing.");

                PaymentGateway paymentGateway = _mapperExtension.FromCreationDto(paymentGatewayCreationDto);

                if (paymentGatewayCreationDto.QrCodeFile != null && paymentGatewayCreationDto.QrCodeFile.Length > 0)
                {
                    var cleanName = paymentGatewayCreationDto.Name.Replace(" ", "_").ToLower();
                    var fileName = $"{cleanName}_{DateTime.Now.Ticks}{Path.GetExtension(paymentGatewayCreationDto.QrCodeFile.FileName)}";

                    var folderPath = Path.Combine(_env.WebRootPath, "uploads", "qrcodes");

                    if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await paymentGatewayCreationDto.QrCodeFile.CopyToAsync(stream);
                    }

                    paymentGateway.QrCodeUrl = "/uploads/qrcodes/" + fileName;
                }
                var result = await _paymentGatewayService.Create(paymentGateway);
                return Ok(_mapperExtension.ToDto(paymentGateway));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private void DeleteOldImage(string? imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl)) return;

            var fullPath = Path.Combine(_env.WebRootPath, "wwwroot", imageUrl.TrimStart('/'));

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdatePaymentGateway(int id, [FromForm] PaymentGatewayCreationDto paymentGatewayCreationDto)
        {
            var existingGateway = await _paymentGatewayService.Get(id);
            if (existingGateway == null) return NotFound("Gateway not found.");

            if (paymentGatewayCreationDto.QrCodeFile != null && paymentGatewayCreationDto.QrCodeFile.Length > 0)
            {
                DeleteOldImage(existingGateway.QrCodeUrl);

                var cleanName = paymentGatewayCreationDto.Name.Replace(" ", "_").ToLower();
                var fileName = $"{cleanName}_{DateTime.Now.Ticks}{Path.GetExtension(paymentGatewayCreationDto.QrCodeFile.FileName)}";
                var folderPath = Path.Combine(_env.WebRootPath, "uploads", "qrcodes");

                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await paymentGatewayCreationDto.QrCodeFile.CopyToAsync(stream);
                }

                existingGateway.QrCodeUrl = "/uploads/qrcodes/" + fileName;
            }

            existingGateway.Name = paymentGatewayCreationDto.Name;
            existingGateway.TransferInfo = paymentGatewayCreationDto.TransferInfo;
            existingGateway.IsActive = paymentGatewayCreationDto.IsActive;

            await _paymentGatewayService.Update(existingGateway);
            return Ok(_mapperExtension.ToDto(existingGateway));
        }
    }
}
