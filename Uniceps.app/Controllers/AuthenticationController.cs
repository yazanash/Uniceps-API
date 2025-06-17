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
        public AuthenticationController(AppDbContext appDbContext, UserManager<AppUser> userManager, IConfiguration configuration, EmailService emailService)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _configuration = configuration;
            _emailService = emailService;
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
                    if (user != null)
                    {
                        IList<string> roles = await _userManager.GetRolesAsync(user);
                        List<Claim> claims = new();
                        //claims.Add(new Claim("token_no", "75"));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName!));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id!));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id!));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]!));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: _configuration["JWT:Issuer"],
                            audience: _configuration["JWT:Audience"],
                            expires: DateTime.Now.AddMonths(1),
                            signingCredentials: sc
                        );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user_type = user.UserType

                        };
                        return Ok(_token);
                    }
                    else
                    {
                        AppUser newUser = new()
                        {
                            Email = oTPDto.Email,
                            UserName = oTPDto.Email!.Split('@')[0],
                            UserType = UserType.Normal
                        };
                        IdentityResult result = await _userManager.CreateAsync(newUser);
                        IList<string> roles = await _userManager.GetRolesAsync(newUser!);
                        List<Claim> claims = new();
                        //claims.Add(new Claim("token_no", "75"));
                        claims.Add(new Claim(ClaimTypes.Name, newUser!.UserName!));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, newUser.Id!));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, newUser.Id!));
                        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

                        foreach (var role in roles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
                        }
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]!));
                        var sc = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        var token = new JwtSecurityToken(
                            claims: claims,
                            issuer: _configuration["Issuer"],
                            audience: _configuration["Audience"],
                            expires: DateTime.Now.AddMonths(1),
                            signingCredentials: sc
                        );
                        var _token = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo,
                            user_type = newUser.UserType
                        };

                        return Created("created", _token);
                    }
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
