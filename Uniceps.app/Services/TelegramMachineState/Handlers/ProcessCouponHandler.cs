using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services.TelegramMachineState.Handlers
{
    public class ProcessCouponHandler
    {
        private readonly ITelegramUserStateDataService<TelegramUserState> _stateService;
        private readonly ICouponDataService _couponDataService; 

        public ProcessCouponHandler(
            ITelegramUserStateDataService<TelegramUserState> stateService,
            ICouponDataService couponDataService)
        {
            _stateService = stateService;
            _couponDataService = couponDataService;
        }

        // أ) في حال أرسل كود حسم
        public async Task<CouponResult> HandleCouponAsync(string couponCode, TelegramUserState userState)
        {
            try
            {
                var coupon = await _couponDataService.GetByCodeAsync(couponCode);
                if (coupon == null) return CouponResult.Invalid;

                userState.CouponId = coupon.Id;
                userState.Discount = coupon.CouponValue;

                var workflow = new BotWorkflowManager(userState);
                if (workflow.CanFire(BotTrigger.CouponSubmitted))
                {
                    workflow.Fire(BotTrigger.CouponSubmitted);
                    userState.LastUpdated = DateTime.UtcNow;
                    await _stateService.UpdateAsync(userState);
                    return CouponResult.Success;
                }

                return CouponResult.Error;
            }
            catch
            {
                return CouponResult.Invalid;
            }
        }

        public async Task<bool> HandleSkipAsync(TelegramUserState userState)
        {
            userState.CouponId = null;
            userState.Discount = 0;

            var workflow = new BotWorkflowManager(userState);
            if (workflow.CanFire(BotTrigger.CouponSkipped))
            {
                workflow.Fire(BotTrigger.CouponSkipped); 
                userState.LastUpdated = DateTime.UtcNow;
                await _stateService.UpdateAsync(userState);
                return true;
            }

            return false;
        }
    }

    public enum CouponResult { Success, Invalid, Error }
}

