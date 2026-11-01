using Microsoft.AspNetCore.Identity;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class SubmitEmailHandler
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        private readonly ICashRequest _cashRequest;
        public SubmitEmailHandler(UserManager<AppUser> userManager, ITelegramUserStateDataService<TelegramUserState> stateService, ICashRequest cashRequest)
        {
            _userManager = userManager;
            _stateService = stateService;
            _cashRequest = cashRequest;
        }

        public async Task<bool> HandleAsync(string email, TelegramUserState userState)
        {
            if (!IsValidEmail(email)) return false;

            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) return false;
           
            userState.Email = email;
            userState.UserId = appUser.Id;

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.EmailSubmitted))
            {
                workflow.Fire(BotTrigger.EmailSubmitted); // ينتقل تلقائياً لـ ChoosingPlan
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
        public async Task<bool> HasPendingCashRequests( TelegramUserState userState)
        {
           
            bool hasPendingRequests = await _cashRequest.UserHasPendingRequests(userState.UserId);
            if (!hasPendingRequests) return false;
         
            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.Cancel))
            {
                workflow.Fire(BotTrigger.Cancel); // ينتقل تلقائياً لـ ChoosingPlan
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
        private bool IsValidEmail(string email)
        {
            try { _ = new System.Net.Mail.MailAddress(email); return true; }
            catch { return false; }
        }
    }
}
