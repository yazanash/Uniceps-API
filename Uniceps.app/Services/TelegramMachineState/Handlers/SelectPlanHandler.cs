using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class SelectPlanHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        public SelectPlanHandler(ITelegramUserStateDataService<TelegramUserState> stateService)
        {
            _stateService = stateService;
        }

        public async Task<bool> HandleAsync(int planId, TelegramUserState userState)
        {
            userState.PlanId = planId;

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.PlanSelected))
            {
                workflow.Fire(BotTrigger.PlanSelected); // ينتقل تلقائياً إلى WaitingCoupon
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
    }
}
