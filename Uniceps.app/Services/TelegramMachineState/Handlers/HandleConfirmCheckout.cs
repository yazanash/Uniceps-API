using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class HandleConfirmCheckout
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;

        public HandleConfirmCheckout(
            ITelegramUserStateDataService<TelegramUserState> stateService)
        {
            _stateService = stateService;
        }

        public async Task<bool> ConfirmCheckoutAsync(TelegramUserState userState)
        {

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.ConfirmCheckout))
            {
                workflow.Fire(BotTrigger.ConfirmCheckout); // ينتقل إلى Done
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
        public async Task<bool> CancelCheckoutAsync(TelegramUserState userState)
        {

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.Cancel))
            {
                workflow.Fire(BotTrigger.Cancel); // ينتقل إلى Done
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }

    }
}
