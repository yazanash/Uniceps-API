using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.AuthenticationDtos;
using Uniceps.app.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {

        private readonly UserManager<AppUser> _userManager;

        public BusinessController( UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> UpgradeToBusinessAccount([FromBody] string email)
        {
            if (User.Identity!=null && User.Identity.IsAuthenticated)
            {
                string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId != null) {
                    AppUser? user = await _userManager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        user.UserType = UserType.Business;
                        await _userManager.UpdateAsync(user);
                        return Ok("account Upgraded susccessfully");
                    }
                }
            }
            return NotFound("This account is not exist");
        }
    }
}
