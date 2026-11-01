using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class WaitingConfirmationViewRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;
        private readonly IPlanDataService _planService;
        private readonly IIntDataService<PaymentGateway> _gatewayService;

        public BotStep TargetStep => BotStep.WaitingConfirmation;

        public WaitingConfirmationViewRenderer(
            ITelegramBotClient bot,
            IPlanDataService planService,
            IIntDataService<PaymentGateway> gatewayService)
        {
            _bot = bot;
            _planService = planService;
            _gatewayService = gatewayService;
        }

        public async Task RenderAsync(TelegramUserState userState)
        {
            var plan = await _planService.Get(userState.PlanId);
            var gateway = await _gatewayService.Get(userState.PaymentGatewayId);

            string summary =
    $@"📋 **ملخص طلب الاشتراك**:
------------------------
📧 **الحساب**: `{userState.Email}`
📦 **الخطة**: {plan?.Name ?? "غير محدد"}
💰 **السعر**: {plan?.Price}$
🎟️ **الخصم**: {userState.Discount}$
💳 **بوابة الدفع**: {gateway?.Name ?? "غير محدد"}
📸 ** كود التحويل **: {userState.TransferCode}

هل ترغب بتأكيد الطلب وإرساله للإدارة؟";

            var keyboard = new InlineKeyboardMarkup(new[]
            {
            new[] { InlineKeyboardButton.WithCallbackData("✅ تأكيد وإرسال الطلب", "confirm_payment") },
            new[] { InlineKeyboardButton.WithCallbackData("الغاء العملية", "cancel_payment") }
        });

            await _bot.SendMessage(userState.ChatId, summary, parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown, replyMarkup: keyboard);
        }
    }
}
