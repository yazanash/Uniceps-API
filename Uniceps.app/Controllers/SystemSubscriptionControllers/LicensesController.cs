using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.DTOs.LicensesDto;
using Uniceps.app.Services;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Services.BillingLicenseService;

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LicensesController : ControllerBase
    {
        private readonly IBellingLicenseService _billingLicenseService;
        private readonly ILicenseGenerationService<LicenseActivation> _licenseGenerationService;

        public LicensesController(IBellingLicenseService billingLicenseService, ILicenseGenerationService<LicenseActivation> licenseGenerationService)
        {
            _billingLicenseService = billingLicenseService;
            _licenseGenerationService = licenseGenerationService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateLicense([FromBody] CreateLicenseRequest createLicenseRequest)
        {
            if (createLicenseRequest == null) return BadRequest("بيانات الطلب غير صالحة");

            BillingLicense billingLicense = createLicenseRequest.ToModel();

            var rawData = $"{billingLicense.Id}|{billingLicense.CustomerName}|{billingLicense.MaxDevices}";

            billingLicense.ServerSignature = _licenseGenerationService.GenerateRSASignature(rawData);

            var createdLicense = await _billingLicenseService.CreateAsync(billingLicense);

            return Ok(createdLicense);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLicense(string id, [FromBody] CreateLicenseRequest createLicenseRequest)
        {
            if (createLicenseRequest == null) return BadRequest("بيانات الطلب غير صالحة");

            BillingLicense billingLicense = await _billingLicenseService.Get(Guid.Parse(id));
            billingLicense.MaxDevices = createLicenseRequest.MaxDevices;
            billingLicense.ExpireDate = createLicenseRequest.ExpireDate;
            billingLicense.CustomerName = createLicenseRequest.CustomerName;
            var rawData = $"{billingLicense.Id}|{billingLicense.CustomerName}|{billingLicense.MaxDevices}";

            billingLicense.ServerSignature = _licenseGenerationService.GenerateRSASignature(rawData);

            var createdLicense = await _billingLicenseService.UpdateAsync(billingLicense);
            return Ok(createdLicense);
        }
        [HttpGet("download-license/{id}")]
        public async Task<IActionResult> DownloadLicense(Guid id)
        {
            var (fileBytes, fileName) = await _licenseGenerationService.DownloadLicenseFileAsync(id);
            return File(fileBytes, "application/octet-stream", fileName);
        }
        [HttpGet]
        public async Task<IActionResult> GetLicense()
        {
            IEnumerable< BillingLicense> billingLicenses = await _billingLicenseService.GetAll();
            var response = billingLicenses.Select(x => new LicenseResponse(x));
            return Ok(response);
        }

        [HttpPost("activate")]
        [AllowAnonymous]
        public async Task<IActionResult> Activate([FromBody] ActivateLicenseRequest request)
        {
            try
            {
                var activation = await _licenseGenerationService.Activate(request.LicenseId, request.MachineId);

                return Ok(new
                {
                    Success = true,
                    Token = activation.ActivationToken,
                    activation.ActivatedAt,
                    activation.ExpiredAt
                    
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Success = false, ex.Message });
            }
        }
    }
}
