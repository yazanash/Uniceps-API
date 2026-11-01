using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class EmailStepProcessor : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly SubmitEmailHandler _submitEmailHandler;

        public EmailStepProcessor(ITelegramBotClient bot, SubmitEmailHandler submitEmailHandler)
        {
            _bot = bot;
            _submitEmailHandler = submitEmailHandler;
        }

        public BotStep TargetStep => BotStep.WaitingEmail;

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query) => await Task.CompletedTask;

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            bool isSuccess = await _submitEmailHandler.HandleAsync(message.Text?.Trim() ?? "", user);
            if (isSuccess)
            {
                bool hasPending = await _submitEmailHandler.HasPendingCashRequests(user);
                if (hasPending) 
                {
                    await _bot.SendMessage(user.ChatId, "لديك طلبات معلقة يرجى الانتظار حتى يقوم فريقنا بمعالجتها او يمكنك التواصل مع فريق الدعم الخاص بنا");
                }
            }
            else
            {
                await _bot.SendMessage(user.ChatId, "❌ الإيميل غير صحيح أو غير مسجل في النظام. تأكد منه.");
            }
        }
    }
}
