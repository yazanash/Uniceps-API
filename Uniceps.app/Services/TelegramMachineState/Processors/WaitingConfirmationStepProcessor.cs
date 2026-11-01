using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class WaitingConfirmationStepProcessor : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly PaymentConfirmedHandler _receiptHandler;

        public BotStep TargetStep => BotStep.WaitingConfirmation;

        public WaitingConfirmationStepProcessor(ITelegramBotClient bot, PaymentConfirmedHandler receiptHandler)
        {
            _bot = bot;
            _receiptHandler = receiptHandler;
        }

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            await _bot.SendMessage(user.ChatId, "الرجاء اضغط على زر (تأكيد الطلب) الموضح بالأسفل لإتمام العملية.");
        }

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query)
        {
            if (query.Data == "confirm_payment")
            {
                bool success = await _receiptHandler.ConfirmAndSubmitAsync(user);
                if (success)
                {
                    await _bot.SendMessage(user.ChatId, "🎉 تم استلام طلبك بنجاح! سيتم مراجعة الإيصال وتفعيل الاشتراك في أقرب وقت.");
                }
                else
                {
                    await _bot.SendMessage(user.ChatId, "❌ حدث خطأ أثناء تأكيد الطلب.");
                }

            }
            if (query.Data == "cancel_payment")
            {
                bool success = await _receiptHandler.CancelAsync(user);
                if (success)
                {
                    await _bot.SendMessage(user.ChatId, "تم الغاء الطلب ");
                }
                else
                {
                    await _bot.SendMessage(user.ChatId, "❌ حدث خطأ أثناء الغاء الطلب.");
                }

            }
        }
    }
}
