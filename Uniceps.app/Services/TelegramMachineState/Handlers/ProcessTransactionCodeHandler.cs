using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class ProcessTransactionCodeHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        private readonly ICashRequest _cashRequest;

        public ProcessTransactionCodeHandler(
            ITelegramUserStateDataService<TelegramUserState> stateService, ICashRequest cashRequest)
        {
            _stateService = stateService;
            _cashRequest = cashRequest;
        }

        public async Task<bool> SaveCodeAsync(string transactionCode, TelegramUserState userState)
        {
            if (string.IsNullOrWhiteSpace(transactionCode)) return false;
            bool exist = await _cashRequest.TransfercodeExist(transactionCode.Trim());
            if (exist) return false;
            userState.TransferCode = transactionCode.Trim();

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.TransferCodeSubmitted))
            {
                workflow.Fire(BotTrigger.TransferCodeSubmitted);
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }

    }
    }
