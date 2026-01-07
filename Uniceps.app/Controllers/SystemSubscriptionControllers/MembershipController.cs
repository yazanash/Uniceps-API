using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Security.Claims;
using Telegram.Bot.Types;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.Services.PaymentServices;
using Uniceps.app.Services.TesterServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class MembershipController : ControllerBase
    {
        private readonly IIntDataService<PlanItem> _dataService;
        private readonly IMembershipDataService _subscriptionDataService;
        private readonly IPaymentGateway _paymentGateway;
        private readonly UserManager<AppUser> _userManager;
        private readonly IProductDataService _productDataService;
        private readonly IBypassService _bypassService;
        private readonly INotificationDataService _notificationDataService;
        public MembershipController(IMembershipDataService subscriptionDataService, IPaymentGateway paymentGateway, UserManager<AppUser> userManager, IIntDataService<PlanItem> dataService, IProductDataService productDataService, IBypassService bypassService, INotificationDataService notificationDataService)
        {
            _subscriptionDataService = subscriptionDataService;
            _paymentGateway = paymentGateway;
            _userManager = userManager;
            _dataService = dataService;
            _productDataService = productDataService;
            _bypassService = bypassService;
            _notificationDataService = notificationDataService;
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RequestSubscription(SystemSubscriptionCreationDto request)
        {
            try
            {
                if (!User.Identity!.IsAuthenticated)
                {
                    return Unauthorized();
                }
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
                var user = await _userManager.FindByIdAsync(userId);
                var plan = await _dataService.Get(request.PlanItemId);

                if (user == null || plan == null)
                    return BadRequest("Invalid user or plan");

                MembershipPayDto membershipPayDto = new MembershipPayDto();
                membershipPayDto.RequirePayment = false;
                membershipPayDto.Message = "Membership Created Successfully";
                var sub = new SystemSubscription
                {
                    UserId = user.Id,
                    PlanNID = plan.PlanNID,
                    PlanItemId = plan.Id,
                    ProductId = plan.PlanModel?.ProductId ?? 0,
                    PlanName = plan.PlanModel?.Name ?? "",
                    PlanDaysCount = plan.DaysCount,
                    PlanDuration = plan.DurationString ?? "",
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(plan.DaysCount),
                    Price = plan.Price,
                    IsGift = false,
                    ISPaid = plan.IsFree,
                    IsActive = plan.IsFree

                };


                await _subscriptionDataService.Create(sub);
                if (plan.IsFree)
                {
                    return Ok(membershipPayDto);
                }
                else
                {
                    membershipPayDto.RequirePayment = true;
                    membershipPayDto.Message = "Membership Created Successfully, but require payment";
                    return Ok(membershipPayDto);
                }
                //var sessionUrl = await _paymentGateway.CreateSessionAsync(sub, user, plan);

                //if (!string.IsNullOrEmpty(sessionUrl))
                //{
                //    sub.StripeCheckoutSessionId = sessionUrl.Contains("stripe") ? sessionUrl : null;
                //    await _subscriptionDataService.Update(sub);
                //    membershipPayDto.RequirePayment = true;
                //    membershipPayDto.PaymentUrl = sessionUrl;
                //    membershipPayDto.Message = "Membership Created Successfully, but require payment";
                //    return Ok(membershipPayDto);
                //}
                //else
                //    return BadRequest("Error in payment gate");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];
            var handled = await _paymentGateway.HandleWebhookAsync(json, signature!);

            return handled ? Ok() : BadRequest("Event not handled");
        }

        [HttpGet("{appId}")]
        [Authorize]
        public async Task<IActionResult> GetValidSubscription(int appId)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            try
            {
                string userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
                AppUser? appUser = await _userManager.FindByIdAsync(userId);
                if (appUser == null)
                    return Unauthorized();

                if (_bypassService.IsTester(appUser.Email ?? ""))
                {
                    SystemSubscriptionDto? subsForTest = _bypassService.GetSubscriptionForTester();
                    if (subsForTest != null)
                        return Ok(subsForTest);
                    else
                        return NotFound("You are not Subscriped");
                }
                var product = await _productDataService.GetByAppId(appId);
                SystemSubscription systemSubscription = await _subscriptionDataService.GetActiveSubscriptionByAppId(userId, product.Id);
                SystemSubscriptionDto systemSubscriptionDto = new SystemSubscriptionDto()
                {
                    Id = systemSubscription.NID,
                    Price = systemSubscription.Price,
                    Plan = systemSubscription.PlanName,
                    StartDate = systemSubscription.StartDate,
                    EndDate = systemSubscription.EndDate,
                    IsActive = systemSubscription.IsActive,
                    IsGift = systemSubscription.IsGift,
                };
                appUser.LastLoginAt = DateTime.UtcNow;
                await _userManager.UpdateAsync(appUser);
                return Ok(systemSubscriptionDto);
            }
            catch
            {
                return NotFound("You are not Subscriped");
            }

        }
        [HttpGet("pending-subscriptions")]
        [Authorize(Roles = "Admin")] // حماية الرابط للمسؤولين فقط
        public async Task<IActionResult> GetPendingSubscriptions([FromQuery] string? email)
        {
            List<MembershipDto> membershipDtos = new List<MembershipDto>();
            if (email != null)
            {
                AppUser? appUser = await _userManager.FindByEmailAsync(email);
                if (appUser != null)
                {
                    var userSubs = await _subscriptionDataService.GetByUserIdListAsync(appUser.Id);

                   
                    foreach (var sub in userSubs)
                    {
                        MembershipDto membership = new MembershipDto
                        {
                            Email = appUser.Email,
                            EndDate = sub.EndDate,
                            Id = sub.NID,
                            Plan = sub.PlanName,
                            Price = sub.Price,
                            StartDate = sub.StartDate,
                        };
                        membershipDtos.Add(membership);
                    }
                }
            }
            else
            {
                var subs = await _subscriptionDataService.GetUnPaidSubscription();
                foreach (var sub in subs)
                {

                    AppUser? appUser = await _userManager.FindByIdAsync(sub.UserId ?? "");
                    if (appUser != null)
                    {
                        MembershipDto membership = new MembershipDto
                        {
                            Email = appUser.Email,
                            EndDate = sub.EndDate,
                            Id = sub.NID,
                            Plan = sub.PlanName,
                            Price = sub.Price,
                            StartDate = sub.StartDate,
                        };
                        membershipDtos.Add(membership);
                    }
                }
               
            }
            if (membershipDtos.Count > 0)
                return Ok(membershipDtos);
            else
                return NotFound("No subscriptions");

        }
        [HttpPost("activate-subscription")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ActivateSubscriptions([FromBody] MemberShipIdDto   memberShipIdDto)
        {
            if (memberShipIdDto.MembershipId == Guid.Empty)
                return BadRequest("No subscriptions selected.");
            await _subscriptionDataService.SetSubscriptionAsPaid(memberShipIdDto.MembershipId);
            var subs = await _subscriptionDataService.Get(memberShipIdDto.MembershipId);
            if (subs != null&& subs.UserId!=null)
            {
                await _notificationDataService.CreateAsync(new Notification
                {
                    UserId = subs.UserId,
                    Title = $"اهلا وسهلا فيك بعالم uniceps",
                    Body = "تم تفعيل الاشتراك الخاص بك بنجاح . خلينا نشوف النتائج الحلوة يابطل ",
                    ScheduledTime = DateTime.UtcNow.AddHours(22)
                });
            }
          
            return Ok(new { Message = $" subscription activated successfully." });
        }
        [HttpDelete("delete-subscription")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteSubscriptions([FromBody] MemberShipIdDto memberShipIdDto)
        {
            if (memberShipIdDto.MembershipId == Guid.Empty)
                return BadRequest("No subscriptions selected.");

            await _subscriptionDataService.Delete(memberShipIdDto.MembershipId);
            return Ok(new { Message = $" subscription Deleted successfully." });
        }
    }
}
