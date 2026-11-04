using Telegram.Bot.Types;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class SelectPlanHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        private readonly IPlanDataService _planDataService;
        public SelectPlanHandler(ITelegramUserStateDataService<TelegramUserState> stateService, IPlanDataService planDataService)
        {
            _stateService = stateService;
            _planDataService = planDataService;
        }

        public async Task<bool> HandleAsync(int planId, TelegramUserState userState)
        {
            userState.PlanId = planId;
            var plan = await _planDataService.Get(planId);
          
            var workflow = new BotWorkflowManager(userState);
            if (plan != null && plan.IsFree)
            {
                if (workflow.CanFire(BotTrigger.PlanFreeSelected))
                {
                    workflow.Fire(BotTrigger.PlanFreeSelected); 
                    userState.LastUpdated = DateTime.UtcNow;
                    await _stateService.UpdateAsync(userState);
                    return true;
                }
            }
            else  if (workflow.CanFire(BotTrigger.PlanSelected))
            {
                workflow.Fire(BotTrigger.PlanSelected);
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
    }
}
