using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class PaymentFreeConfirmedHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        private readonly IMembershipDataService _membershipDataService;
        private readonly INotificationDataService _notificationDataService;
        public PaymentFreeConfirmedHandler(
            ITelegramUserStateDataService<TelegramUserState> stateService,
            IMembershipDataService membershipDataService,
            INotificationDataService notificationDataService)
        {
            _stateService = stateService;
            _membershipDataService = membershipDataService;
            _notificationDataService = notificationDataService;
        }

        public async Task<bool> ConfirmAndSubmitAsync(TelegramUserState userState)
        {

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.ConfirmPayment))
            {
               var sub = await _membershipDataService.CreateSubscriptionAsync(userState.PlanId,null, userState.UserId);
                if (sub.UserId != null)
                {
                    await _notificationDataService.CreateAsync(new Notification
                    {
                        UserId = sub.UserId,
                        Title = $"اهلا وسهلا فيك بعالم uniceps",
                        Body = "تم تفعيل الاشتراك الخاص بك بنجاح . خلينا نشوف النتائج الحلوة يابطل ",
                        ScheduledTime = DateTime.UtcNow
                    });
                }
              
                workflow.Fire(BotTrigger.ConfirmPayment);
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
