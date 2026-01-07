using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public CashRequestController(IIntDataService<PaymentGateway> gatewayService, ICashRequest paymentRequestService, IMembershipDataService subscriptionDataService, TelegramBotService telegramBotService, INotificationDataService notificationDataService)
        {
            _gatewayService = gatewayService;
            _paymentRequestService = paymentRequestService;
            _subscriptionDataService = subscriptionDataService;
            _telegramBotService = telegramBotService;
            _notificationDataService = notificationDataService;
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
                    SystemSubscription systemSubscription = await _subscriptionDataService.Get(Guid.Parse(cashPaymentRequest.SubscriptionId!));
                    PaymentGateway paymentGateway = await _gatewayService.Get(cashPaymentRequest.PaymentGatewayId);
                    CashRequestDto cashRequestDto = new CashRequestDto();
                    cashRequestDto.Id = cashPaymentRequest.Id;
                    cashRequestDto.Email = cashPaymentRequest.Email;
                    cashRequestDto.TransferCode = cashPaymentRequest.TransferCode;
                    cashRequestDto.Amount = cashPaymentRequest.Amount;
                    cashRequestDto.Status = cashPaymentRequest.Status.ToString();
                    cashRequestDto.Subscription = systemSubscription.PlanName.ToString();
                    cashRequestDto.SubscriptionPrice = systemSubscription.Price.ToString();
                    cashRequestDto.PaymentGateway = paymentGateway.Name;
                    cashRequestDto.CreatedAt = cashPaymentRequest.CreatedAt.ToString();
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
                    bool systemSubscription = await _subscriptionDataService.SetSubscriptionAsPaid(Guid.Parse(cashPaymentRequest.SubscriptionId!));
                    if (systemSubscription)
                    {
                        await _telegramBotService.HandelRequestAccepted(cashPaymentRequest.ChatId);
                        var subs = await _subscriptionDataService.Get(Guid.Parse(cashPaymentRequest.SubscriptionId!));
                        if (subs != null && subs.UserId != null)
                        {
                            await _notificationDataService.CreateAsync(new Notification
                            {
                                UserId = subs.UserId,
                                Title = $"اهلا وسهلا فيك بعالم uniceps",
                                Body = "تم تفعيل الاشتراك الخاص بك بنجاح . خلينا نشوف النتائج الحلوة يابطل ",
                                ScheduledTime = DateTime.UtcNow.AddHours(22)
                            });
                        }
                    }

                }
                else
                {
                    await _telegramBotService.HandelRequestDenied(cashPaymentRequest.ChatId);
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
