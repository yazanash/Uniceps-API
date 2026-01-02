using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Uniceps.app.DTOs.AuthenticationDtos;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AdminController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("CheckAdminStatus")]
        public IActionResult VerifyAdminStatus()
        {
            // نتحقق من الـ Claims الموجودة في التوكن
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin) return Forbid(); // 403 ارجع خطأ صلاحيات

            return Ok(new { status = "Authorized", role = "Admin" });
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAdmins()
        {
            // جلب المستخدمين الذين لديهم رول "Admin"
            var adminUsers = await _userManager.GetUsersInRoleAsync("Admin");

            var result = adminUsers.Select(u => new {
                id = u.Id,
                email = u.Email,
                name = u.UserName
            }).ToList();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")] 
        [HttpPost("[action]")]
        public async Task<IActionResult> AddAdminRole([FromBody] AdminRequest model)
        {
            if(string.IsNullOrEmpty(model.Email))
                return BadRequest("Empty Email not allowed");

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return NotFound(new { message = "User not found" });

            // 2. التحقق هل الرول (Admin) موجود أصلاً في النظام، وإن لم يكن ننشئه
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            // 3. إضافة المستخدم إلى الرول
            var result = await _userManager.AddToRoleAsync(user, "Admin");

            if (result.Succeeded)
            {
                return Ok(new { message = $"User {model.Email} is now an Admin" });
            }

            return BadRequest(result.Errors);
        }
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> RevokeAdmin(string id)
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == id)
                return Forbid("you can't remove yourself");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            // سحب الرول
            var result = await _userManager.RemoveFromRoleAsync(user, "Admin");

            if (result.Succeeded)
                return Ok(new { message = "تم سحب الصلاحية بنجاح" });

            return BadRequest(result.Errors);
        }
    }
}
