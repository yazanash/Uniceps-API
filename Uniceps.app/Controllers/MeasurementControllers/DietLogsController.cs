using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.app.DTOs.NutritionDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NutritionSystem;
using Uniceps.Entityframework.Services.NutritionServices;

namespace Uniceps.app.Controllers.MeasurementControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DietLogsController : ControllerBase
    {
        private IDietLogDataService _service;

        public DietLogsController(IDietLogDataService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            IEnumerable<DietLog> logs = await _service.GetAllByUserAsync(userId);
            return Ok(logs.Select(x => new DietLogResponse(x)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> Create(DietLogRequest  dietLogRequest)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }

            if (dietLogRequest == null)
                return BadRequest("diet log data is missing.");

            DietLog dietLog = dietLogRequest.ToModel();
            dietLog.UserId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var result = await _service.UpsertAsync(dietLog);
            return Ok(new { ApiId= result.Id });
        }
    }
}
