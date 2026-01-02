using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Telegram.Bot.Types;
using Uniceps.app.DTOs;
using Uniceps.app.DTOs.AuthenticationDtos;
using Uniceps.app.Services;
using Uniceps.app.Services.TesterServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Services.NotificationSystemServices;
using static System.Net.WebRequestMethods;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;
        private readonly IJwtTokenService _tokenService;
        private readonly IOTPGenerateService<OTPModel> _otpGenerateService;
        private readonly IBypassService _bypassService;
        private readonly IUserDeviceDataService _userDeviceDataService;
        public AuthenticationController(UserManager<AppUser> userManager, EmailService emailService, IJwtTokenService tokenService, IOTPGenerateService<OTPModel> otpGenerateService, IBypassService bypassService, IUserDeviceDataService userDeviceDataService)
        {
            _userManager = userManager;
            _emailService = emailService;
            _tokenService = tokenService;
            _otpGenerateService = otpGenerateService;
            _bypassService = bypassService;
            _userDeviceDataService = userDeviceDataService;
        }
        [HttpPost]
        public async Task<IActionResult> RequestOtp([FromBody] EmailDto emailDto)
        {
            try
            {
                if (string.IsNullOrEmpty(emailDto.Email?.Trim()) || !IsValidEmail(emailDto.Email))
                {
                    return BadRequest("Invalid Email");
                }
                if (_bypassService.IsTester(emailDto.Email))
                    return Ok("Email sent successfully");
                OTPModel model = await _otpGenerateService.GenerateAsync(emailDto.Email.Trim());
                await _emailService.SendEmailAsync(emailDto.Email.Trim(), model.Otp);
                return Ok("Email sent successfully");
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong");
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch { return false; }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyOtp([FromBody] OTPDto oTPDto)
        {
            try
            {
                if (string.IsNullOrEmpty(oTPDto.Email?.Trim()) || !IsValidEmail(oTPDto.Email))
                {
                    return BadRequest("Invalid Email");
                }
                bool isAuthorized = _bypassService.IsValidTester(oTPDto.Email, oTPDto.Otp.ToString());

                if (!isAuthorized)
                {
                    var oTPModel = await _otpGenerateService.VerifyAsync(oTPDto.Email, oTPDto.Otp);
                    if (oTPModel == null) return Unauthorized("Invalid OTP");
                }

                AppUser? user = await _userManager.FindByEmailAsync(oTPDto.Email!);
                if (user == null)
                {
                    user = new()
                    {
                        Email = oTPDto.Email,
                        UserName = oTPDto.Email!.Split('@')[0],
                        UserType = UserType.Normal
                    };
                    IdentityResult result = await _userManager.CreateAsync(user);
                    await _userManager.AddToRoleAsync(user, "User");
                }
                IList<string> roles = await _userManager.GetRolesAsync(user!);
                JwtTokenResult token = _tokenService.GenerateToken(user, roles);
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // على الإنتاج لازم يكون https
                    SameSite = SameSiteMode.None,
                    Expires = token.ExpiresAt,
                    Path = "/"
                };
                if (!string.IsNullOrEmpty(oTPDto.DeviceId))
                {
                    UserDevice userDevice = new();
                    userDevice.AppVersion = oTPDto.AppVersion;
                    userDevice.OsVersion = oTPDto.OsVersion;
                    userDevice.UserId = user.Id;
                    userDevice.DeviceId = oTPDto.DeviceId;
                    userDevice.DeviceToken = oTPDto.DeviceToken;
                    userDevice.Platform = oTPDto.Platform;
                    userDevice.DeviceModel = oTPDto.DeviceModel;
                    userDevice.NotifyToken = oTPDto.NotifyToken;
                    await _userDeviceDataService.UpsertUserDeviceAsync(userDevice);
                }
             


                Response.Cookies.Append("auth", token.Token, cookieOptions);

                return Created("created", token);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> VerifyConnection()
        {
            if (!User!.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    user.LastLoginAt = DateTime.Now;
                    await _userManager.UpdateAsync(user);
                    return Ok();
                }
            }
            return Forbid();
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> RefreshToken([FromQuery] string? notify, [FromQuery] string? deviceId)
        {
            if (!User!.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId != null)
            {
                AppUser? user = await _userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    IList<string> roles = await _userManager.GetRolesAsync(user!);
                    JwtTokenResult token = _tokenService.GenerateToken(user, roles);
                    var cookieOptions = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // على الإنتاج لازم يكون https
                        SameSite = SameSiteMode.None,
                        Expires = token.ExpiresAt,
                        Path = "/"
                    };
                    if (!string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(notify))
                    {
                        var deviceData = new UserDevice
                        {
                            UserId = user.Id,
                            DeviceId = deviceId,
                            NotifyToken = notify ,
                        };

                        await _userDeviceDataService.UpsertUserDeviceAsync(deviceData);
                    }
                  
                 


                    Response.Cookies.Append("auth", token.Token, cookieOptions);
                    user.LastLoginAt = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    return Ok(token);
                }
            }
            return Forbid();
        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            // مسح الكوكي عن طريق إرسال كوكي بنفس الاسم وتاريخ منتهي
            Response.Cookies.Delete("auth", new CookieOptions
            {
                Path = "/",
                HttpOnly = true,
                Secure = true, // مهم إذا كنت تستخدم HTTPS
                SameSite = SameSiteMode.None
            });

            return Ok(new { message = "Logged out successfully" });
        }
    }
}
