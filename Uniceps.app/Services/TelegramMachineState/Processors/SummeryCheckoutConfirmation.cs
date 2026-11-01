using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class SummeryCheckoutConfirmation : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly HandleConfirmCheckout _handleConfirmCheckout;

        public BotStep TargetStep => BotStep.WaitingCheckoutConfirmation;

        public SummeryCheckoutConfirmation(ITelegramBotClient bot, HandleConfirmCheckout handleConfirmCheckout)
        {
            _bot = bot;
            _handleConfirmCheckout = handleConfirmCheckout;
        }

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            await _bot.SendMessage(user.ChatId, "الرجاء اضغط على زر (المتابعة) الموضح بالأسفل لإتمام العملية.");
        }

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query)
        {
            if (query.Data == "continue")
            {
                bool success = await _handleConfirmCheckout.ConfirmCheckoutAsync(user);
                if (!success)
                {
                    await _bot.SendMessage(user.ChatId, "❌ حدث خطأ أثناء تأكيد الطلب.");
                }

            }
            else if (query.Data == "cancel")
            {
                bool success = await _handleConfirmCheckout.CancelCheckoutAsync(user);
                if (success)
                {
                    await _bot.SendMessage(user.ChatId, "تم الغاء الطلب");
                }
                else
                {
                    await _bot.SendMessage(user.ChatId, "❌ حدث خطأ أثناء الغاء الطلب.");
                }

            }
        }
    }
}
