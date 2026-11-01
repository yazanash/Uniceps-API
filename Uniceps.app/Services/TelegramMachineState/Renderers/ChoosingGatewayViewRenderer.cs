using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Uniceps.app.Services.PaymentServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class ChoosingGatewayViewRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;
        private readonly IIntDataService<PaymentGateway> _gatewayDataService;

        public BotStep TargetStep => BotStep.ChoosingGateway;

        public ChoosingGatewayViewRenderer(ITelegramBotClient bot, IIntDataService<PaymentGateway> gatewayDataService)
        {
            _bot = bot;
            _gatewayDataService = gatewayDataService;
        }

        public async Task RenderAsync(TelegramUserState userState)
        {
            var gateways = await _gatewayDataService.GetAll();

            if (!gateways.Any())
            {
                await _bot.SendMessage(userState.ChatId, "لا توجد بوابات دفع متاحة حالياً.");
                return;
            }

            var buttons = gateways
                .Select(g => InlineKeyboardButton.WithCallbackData(g.Name, $"gateway_{g.Id}"))
                .Select(b => new[] { b })
                .ToArray();

            var keyboard = new InlineKeyboardMarkup(buttons);
            await _bot.SendMessage(userState.ChatId, "اختر بوابة الدفع المناسبة لك:", replyMarkup: keyboard);
        }
    }
}
