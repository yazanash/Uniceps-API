using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class StartFlowHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;

        public StartFlowHandler(ITelegramUserStateDataService<TelegramUserState> stateService)
        {
            _stateService = stateService;
        }

        public async Task<bool> HandleStartAsync(TelegramUserState userState)
        {
            // 1. تصفير البيانات السابقة للحساب لتفادي أي تداخل بالطلب القديم
            userState.Email = string.Empty;
            userState.UserId = string.Empty;
            userState.PlanId = 0;
            userState.CouponId = null;
            userState.Discount = 0;
            userState.PaymentGatewayId = 0;

            // 2. استخدام محرك الحالة لنقله من Start إلى WaitingEmail
            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.Start))
            {
                workflow.Fire(BotTrigger.Start); // يتحول تلقائياً إلى WaitingEmail
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
    }
}
