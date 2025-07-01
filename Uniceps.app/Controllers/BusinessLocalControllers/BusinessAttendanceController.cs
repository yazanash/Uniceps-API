using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessAttendanceDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessPaymentDtos;
using Uniceps.app.Helpers;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Controllers.BusinessLocalControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessAttendanceController : ControllerBase
    {
        private readonly IDataService<BusinessAttendanceRecord> _dataService;
        private readonly IMapperExtension<BusinessAttendanceRecord, BusinessAttendanceRecordDto, BusinessAttendanceRecordCreationDto> _mapperExtension;

        public BusinessAttendanceController(IDataService<BusinessAttendanceRecord> dataService, IMapperExtension<BusinessAttendanceRecord, BusinessAttendanceRecordDto, BusinessAttendanceRecordCreationDto> mapperExtension)
        {
            _dataService = dataService;
            _mapperExtension = mapperExtension;
        }
        [HttpPost]
        public async Task<IActionResult> Create(BusinessAttendanceRecordCreationDto businessAttendanceRecordCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (businessAttendanceRecordCreationDto == null)
                return BadRequest("Exercise data is missing.");

            BusinessAttendanceRecord businessAttendance = _mapperExtension.FromCreationDto(businessAttendanceRecordCreationDto);
            businessAttendance.BusinessId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _dataService.Create(businessAttendance);
            return Ok(_mapperExtension.ToDto(businessAttendance));
        }
        [HttpPut("Id")]
        public async Task<IActionResult> Update(Guid Id, [FromBody] BusinessAttendanceRecordCreationDto businessAttendanceRecordCreationDto)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            if (!HttpContext.IsBusinessUser())
            {
                return Forbid();
            }
            if (businessAttendanceRecordCreationDto == null)
                return BadRequest("Exercise data is missing.");

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            BusinessAttendanceRecord businessAttendanceRecord = await _dataService.Get(Id);
            BusinessAttendanceRecord newBusinessAttendanceRecord = _mapperExtension.FromCreationDto(businessAttendanceRecordCreationDto);
            newBusinessAttendanceRecord.NID = businessAttendanceRecord.NID;
            //newPlayerModel.UserId = userId;
            await _dataService.Update(newBusinessAttendanceRecord);
            return Ok("Updated successfully");
        }
        [HttpDelete("id")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _dataService.Delete(id);
            return Ok("Deleted successfully");
        }
    }
}
