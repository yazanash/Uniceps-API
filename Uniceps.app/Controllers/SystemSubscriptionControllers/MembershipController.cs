using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.Services.PaymentServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
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
        public MembershipController(IMembershipDataService subscriptionDataService, IPaymentGateway paymentGateway, UserManager<AppUser> userManager, IIntDataService<PlanItem> dataService, IProductDataService productDataService)
        {
            _subscriptionDataService = subscriptionDataService;
            _paymentGateway = paymentGateway;
            _userManager = userManager;
            _dataService = dataService;
            _productDataService = productDataService;
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
                    ProductId = plan.PlanModel?.ProductId??0,
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
                if(appUser == null)
                    return Unauthorized();

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
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
