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

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSubscriptionController : ControllerBase
    {
        private readonly IDataService<PlanModel> _planDataService;
        private readonly IDataService<SystemSubscription> _subscriptionDataService;
        private readonly IPaymentGateway _paymentGateway;
        private readonly UserManager<AppUser> _userManager;
        public SystemSubscriptionController(IDataService<PlanModel> planDataService, IDataService<SystemSubscription> subscriptionDataService, IPaymentGateway paymentGateway, UserManager<AppUser> userManager)
        {
            _planDataService = planDataService;
            _subscriptionDataService = subscriptionDataService;
            _paymentGateway = paymentGateway;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> RequestSubscription(SystemSubscriptionCreationDto request)
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return Unauthorized();
            }
            string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;
            var user = await _userManager.FindByIdAsync(userId);
            var plan = await _planDataService.Get(request.PlanId);

            if (user == null || plan == null)
                return BadRequest("Invalid user or plan");

            var sub = new SystemSubscription
            {
                UserId = user.Id,
                PlanId = plan.Id,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(plan.DurationInDays),
                Price = plan.Price,
                IsGift = false,
                ISPaid = false,
                IsActive = false
            };

            

           
            var sessionUrl = await _paymentGateway.CreateSessionAsync(sub, user, plan);

            if (!string.IsNullOrEmpty(sessionUrl))
            {
                sub.StripeCheckoutSessionId = sessionUrl.Contains("stripe") ? sessionUrl : null;
                await _subscriptionDataService.Create(sub);
                return Ok(new { sessionUrl });
            }
            else
                return BadRequest();
        }
        [HttpPost("stripe")]
        public async Task<IActionResult> StripeWebhook()
        {
            Console.WriteLine("handled");
            var json = await new StreamReader(Request.Body).ReadToEndAsync();
            var signature = Request.Headers["Stripe-Signature"];
            var handled = await _paymentGateway.HandleWebhookAsync(json, signature!);

            return handled ? Ok() : BadRequest("Event not handled");
        }
    }
}
