using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class PaymentConfirmedHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        private readonly ICashRequest _paymentRequestDataService; // خدمة حفظ الطلب النهائي

        public PaymentConfirmedHandler(
            ITelegramUserStateDataService<TelegramUserState> stateService,
            ICashRequest paymentRequestDataService)
        {
            _stateService = stateService;
            _paymentRequestDataService = paymentRequestDataService;
        }

        public async Task<bool> ConfirmAndSubmitAsync(TelegramUserState userState)
        {

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.ConfirmPayment))
            {
                // إنشاء كائن طلب الدفع النهائي وإرساله للإدارة أو القاعدة
                await _paymentRequestDataService.Create(new CashPaymentRequest
                {
                    UserId = userState.UserId,
                    PlanItemId = userState.PlanId,
                    CouponId = userState.CouponId,
                    PaymentGatewayId = userState.PaymentGatewayId,
                    CreatedAt = DateTime.UtcNow,
                    ChatId = userState.ChatId,
                    Email = userState.Email!,
                    TransferCode = userState.TransferCode!,
                });

                workflow.Fire(BotTrigger.ConfirmPayment); // ينتقل إلى Done
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }

        public async Task<bool> CancelAsync(TelegramUserState userState)
        {

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.Cancel))
            {
                workflow.Fire(BotTrigger.Cancel); 
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }

    }
}

