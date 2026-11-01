using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class SummeryCheckoutRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;
        private readonly IPlanDataService _planService;
        public BotStep TargetStep => BotStep.WaitingCheckoutConfirmation;
        public SummeryCheckoutRenderer(
            ITelegramBotClient bot,
            IPlanDataService planService
           )
        {
            _bot = bot;
            _planService = planService;
        }

        public async Task RenderAsync(TelegramUserState userState)
        {
           
            var plan = await _planService.Get(userState.PlanId);
        
            string summary =
    $@"📋 **ملخص طلب الاشتراك**:
------------------------
📧 **الحساب**: `{userState.Email}`
📦 **الخطة**: {plan?.Name ?? "غير محدد"}
💰 **السعر**: {plan?.Price}$
🎟️ **الخصم**: {userState.Discount}$
هل ترغب بالمتابعة ؟ ";

            var keyboard = new InlineKeyboardMarkup(new[]
            {
            new[] { InlineKeyboardButton.WithCallbackData("✅ تأكيد وإرسال الطلب", "continue") },
            new[] { InlineKeyboardButton.WithCallbackData("الغاء العملية", "cancel") }
        });

            await _bot.SendMessage(userState.ChatId, summary, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: keyboard);
           
        }
    }
}
