using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class SelectGatewayHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;

        public SelectGatewayHandler(ITelegramUserStateDataService<TelegramUserState> stateService)
        {
            _stateService = stateService;
        }

        public async Task<bool> HandleAsync(int gatewayId, TelegramUserState userState)
        {
            userState.PaymentGatewayId = gatewayId;

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.GatewaySelected))
            {
                workflow.Fire(BotTrigger.GatewaySelected);
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
    }
}
