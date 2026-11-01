using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Security.Claims;
using Telegram.Bot.Types;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.Services;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Controllers.SystemSubscriptionControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CashRequestController : ControllerBase
    {
        private readonly IIntDataService<PaymentGateway> _gatewayService;
        private readonly ICashRequest _paymentRequestService;
        private readonly IMembershipDataService _subscriptionDataService;
        private readonly TelegramBotService _telegramBotService;
        private readonly INotificationDataService _notificationDataService;
        private readonly IPlanDataService _planDataService;
        private readonly ICouponDataService _couponDataService;
        public CashRequestController(IIntDataService<PaymentGateway> gatewayService, ICashRequest paymentRequestService, IMembershipDataService subscriptionDataService, TelegramBotService telegramBotService, INotificationDataService notificationDataService, IPlanDataService planDataService, ICouponDataService couponDataService)
        {
            _gatewayService = gatewayService;
            _paymentRequestService = paymentRequestService;
            _subscriptionDataService = subscriptionDataService;
            _telegramBotService = telegramBotService;
            _notificationDataService = notificationDataService;
            _planDataService = planDataService;
            _couponDataService = couponDataService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CashRequestStatus cashRequestStatus = CashRequestStatus.Pending)
        {
            try
            {
                IEnumerable<CashPaymentRequest> cashPaymentRequests = await _paymentRequestService.GetAll(cashRequestStatus);
                List<CashRequestDto> requests = new List<CashRequestDto>();
                foreach (CashPaymentRequest cashPaymentRequest in cashPaymentRequests)
                {
                    PlanItem selectedPlan = await _planDataService.Get(cashPaymentRequest.PlanItemId);
                    PaymentGateway paymentGateway = await _gatewayService.Get(cashPaymentRequest.PaymentGatewayId);
                    SubscriptionCoupon? coupon = cashPaymentRequest.CouponId.HasValue? await _couponDataService.Get((int)cashPaymentRequest.CouponId):null;
                    CashRequestDto cashRequestDto = new CashRequestDto();
                    cashRequestDto.Id = cashPaymentRequest.Id;
                    cashRequestDto.Email = cashPaymentRequest.Email;
                    cashRequestDto.TransferCode = cashPaymentRequest.TransferCode;
                    cashRequestDto.Status = cashPaymentRequest.Status.ToString();
                    cashRequestDto.Subscription = selectedPlan.Name;
                    cashRequestDto.SubscriptionPrice = selectedPlan.Price;
                    cashRequestDto.PaymentGateway = paymentGateway.Name;
                    cashRequestDto.CreatedAt = cashPaymentRequest.CreatedAt.ToString();
                    cashRequestDto.CouponCode = coupon?.Code;
                    cashRequestDto.CouponValue = coupon?.CouponValue??0;
                    requests.Add(cashRequestDto);
                }
                return Ok(requests);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
        [HttpPost("[action]")]
        public async Task<IActionResult> ChangeCashRequestStatus([FromBody] ChangeCashRequestStatusDto changeCashRequestStatusDto)
        {
            try
            {
                CashPaymentRequest cashPaymentRequest = await _paymentRequestService.Get(changeCashRequestStatusDto.Id);
                cashPaymentRequest.Status = changeCashRequestStatusDto.CashRequestStatus;
                if (cashPaymentRequest.Status == CashRequestStatus.Accepted)
                {
                    PlanItem selectedPlan = await _planDataService.Get(cashPaymentRequest.PlanItemId);
                 
                    var created = await _subscriptionDataService.CreateSubscriptionAsync(cashPaymentRequest.PlanItemId,cashPaymentRequest.CouponId,cashPaymentRequest.UserId!);
                    if (created!=null&& created.UserId!=null)
                    {
                        await _telegramBotService.HandelRequestAccepted(cashPaymentRequest.ChatId, changeCashRequestStatusDto.Message);
                     
                            await _notificationDataService.CreateAsync(new Notification
                            {
                                UserId = created.UserId,
                                Title = $"اهلا وسهلا فيك بعالم uniceps",
                                Body = "تم تفعيل الاشتراك الخاص بك بنجاح . خلينا نشوف النتائج الحلوة يابطل ",
                                ScheduledTime = DateTime.UtcNow.AddHours(22)
                            });
                        
                    }

                }
                else
                {
                    await _telegramBotService.HandelRequestDenied(cashPaymentRequest.ChatId, changeCashRequestStatusDto.Message);
                }
                await _paymentRequestService.Update(cashPaymentRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

        }
    }
}
