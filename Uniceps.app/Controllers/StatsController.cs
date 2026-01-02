using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.StatsModels;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class StatsController : ControllerBase
    {
        private readonly IStatsDataService<DashboardStats> _statsDataService;

        public StatsController(IStatsDataService<DashboardStats> statsDataService)
        {
            _statsDataService = statsDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetStats()
        {
            var result = await _statsDataService.GetDashboardStatsAsync();
            return Ok(result);
        }
    }
}
