using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class WaitingCouponStepProcessor
   : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly ProcessCouponHandler _couponHandler;

        public BotStep TargetStep => BotStep.WaitingCoupon;

        public WaitingCouponStepProcessor(ITelegramBotClient bot, ProcessCouponHandler couponHandler)
        {
            _bot = bot;
            _couponHandler = couponHandler;
        }

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            string text = message.Text?.Trim() ?? string.Empty;

            if (text.Equals("/no_coupon", StringComparison.OrdinalIgnoreCase))
            {
                bool skipped = await _couponHandler.HandleSkipAsync(user);
                if (!skipped)
                {
                    await _bot.SendMessage(user.ChatId, "❌ حدث خطأ أثناء تخطي الكوبون.");
                }
            }
            else
            {
                var result = await _couponHandler.HandleCouponAsync(text, user);
                if (result == CouponResult.Invalid)
                {
                    await _bot.SendMessage(user.ChatId, "❌ الكوبون غير صالح أو منتهي الصلاحية. أعد المحاولة أو أرسل /no_coupon للتخطي.");
                }
                else if (result == CouponResult.Success)
                {
                    await _bot.SendMessage(user.ChatId, "✅ تم تطبيق الكوبون بنجاح!");
                }
            }
        }

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query) => await Task.CompletedTask;
    }
}
