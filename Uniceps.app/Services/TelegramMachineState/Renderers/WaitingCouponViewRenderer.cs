using Telegram.Bot;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class WaitingCouponViewRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;

        public BotStep TargetStep => BotStep.WaitingCoupon;

        public WaitingCouponViewRenderer(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task RenderAsync(TelegramUserState userState)
        {
            string couponMessage =
    @"هل لديك كود حسم ؟ ... ادخل الكود
- /no_coupon : اكمل بدون كوبون";

            await _bot.SendMessage(userState.ChatId, couponMessage);
        }
    }
}
