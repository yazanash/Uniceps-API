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
using Uniceps.app.DTOs;
using Uniceps.app.DTOs.AuthenticationDtos;
using Uniceps.app.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.AuthenticationModels;
using static System.Net.WebRequestMethods;

namespace Uniceps.app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;
        private readonly IJwtTokenService _tokenService;
        public AuthenticationController(AppDbContext appDbContext, UserManager<AppUser> userManager, IConfiguration configuration, EmailService emailService, IJwtTokenService tokenService)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
            _tokenService = tokenService;
        }
        [HttpPost]
        public async Task<IActionResult> RequestOtp([FromBody] string email)
        {
            try
            {
                List<OTPModel> otps = await _appDbContext.OTPModels.Where(x => x.Email == email).ToListAsync();
                _appDbContext.RemoveRange(otps);
                OTPModel model = new OTPModel();
                model.Email = email;
                model.Otp = new Random().Next(111111, 999999);
                model.ExpireDate = DateTime.Now.AddMinutes(30);
                await _appDbContext.AddAsync(model);
                await _appDbContext.SaveChangesAsync();
                await _emailService.SendEmailAsync(email, model.Otp);
                return Ok("Email sent successfully");
            }
          catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }
        
        [HttpPost("[action]")]
        public async Task<IActionResult> VerifyOtp([FromBody] OTPDto oTPDto)
        {
            try
            {
                OTPModel? oTPModel = await _appDbContext.OTPModels.FirstOrDefaultAsync(x => x.Email == oTPDto.Email);
                if (oTPModel != null && oTPModel.Otp == oTPDto.Otp && oTPModel.ExpireDate.Subtract(DateTime.Now).TotalMinutes > 0)
                {
                    _appDbContext.OTPModels.Remove(oTPModel);
                    await _appDbContext.SaveChangesAsync();
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

                    }
                    IList<string> roles = await _userManager.GetRolesAsync(user!);
                    JwtTokenResult token = _tokenService.GenerateToken(user, roles);
                    return Created("created", token);
                }
                else
                {
                    if (oTPModel != null)
                    {
                        _appDbContext.OTPModels.Remove(oTPModel);
                        await _appDbContext.SaveChangesAsync();
                    }
                    return BadRequest("otp invalid");
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong.");
            }
        }
    }
}
