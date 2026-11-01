using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class WaitingTransactionCodeStepProcessor : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly ProcessTransactionCodeHandler _handler;

        public BotStep TargetStep => BotStep.WaitingTransferCode;

        public WaitingTransactionCodeStepProcessor(ITelegramBotClient bot, ProcessTransactionCodeHandler handler)
        {
            _bot = bot;
            _handler = handler;
        }

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            string text = message.Text?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(text) || text.StartsWith("/"))
            {
                await _bot.SendMessage(user.ChatId, "❌ الرجاء إرسال **رقم/كود الحوالة** الصحيح كتابةً.");
                return;
            }

            bool success = await _handler.SaveCodeAsync(text, user);
            if (!success)
            {
                await _bot.SendMessage(user.ChatId, "❌ حدث خطأ أثناء حفظ الكود، حاول مرة أخرى. وتاكد ان رقم الحوالة صحيح");
            }
        }

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query) => await Task.CompletedTask;
    }
}
