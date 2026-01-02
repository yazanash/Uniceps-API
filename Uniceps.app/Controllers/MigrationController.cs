using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uniceps.app.Services;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class MigrationController : ControllerBase
    {
        private readonly DataMigrationService _dataMigrationService;

        public MigrationController(DataMigrationService dataMigrationService)
        {
            _dataMigrationService = dataMigrationService;
        }

        [HttpGet]
        public async Task<IActionResult> Migrate()
        {
           await _dataMigrationService.MigrateData();
            return Ok("Data Migrated sucessfully");
        }
    }
}
