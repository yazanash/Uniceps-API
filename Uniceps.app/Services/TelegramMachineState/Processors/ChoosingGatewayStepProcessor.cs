using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class ChoosingGatewayStepProcessor : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly SelectGatewayHandler _gatewayHandler;
        public BotStep TargetStep => BotStep.ChoosingGateway;

        public ChoosingGatewayStepProcessor(ITelegramBotClient bot, SelectGatewayHandler gatewayHandler)
        {
            _bot = bot;
            _gatewayHandler = gatewayHandler;
        }
        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            await _bot.SendMessage(user.ChatId, "الرجاء اختيار بوابة الدفع من الأزرار الظاهرة أمامك.");
        }
        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query)
        {
            if (query.Data?.StartsWith("gateway_") == true)
            {
                var gwIdStr = query.Data.Replace("gateway_", "");
                if (int.TryParse(gwIdStr, out int gatewayId))
                {
                    bool isSuccess = await _gatewayHandler.HandleAsync(gatewayId, user);

                    if (!isSuccess)
                    {
                        await _bot.SendMessage(user.ChatId, "❌ حدث خطأ في اختيار بوابة الدفع، حاول مرة أخرى.");
                    }
                }
            }
        }
    }
}
