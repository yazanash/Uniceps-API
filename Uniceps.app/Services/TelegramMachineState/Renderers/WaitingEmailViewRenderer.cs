using Telegram.Bot;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class WaitingEmailViewRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;

        public BotStep TargetStep => BotStep.WaitingEmail;

        public WaitingEmailViewRenderer(ITelegramBotClient bot)
        {
            _bot = bot;
        }

        public async Task RenderAsync(TelegramUserState userState)
        {
       string welcomeMessage =
 @"أهلاً 👋 أنا بوت الدفع النقدي الخاص ب Uniceps.
الأوامر الرئيسية:
- /start : بدء العملية من جديد
- /cancel : إلغاء العملية الحالية

الرجاء إرسال بريدك الإلكتروني لتأكيد هويتك:";

            await _bot.SendMessage(userState.ChatId, welcomeMessage);
        }
    }
}
