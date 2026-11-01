using Telegram.Bot.Types;
using Uniceps.Entityframework.Models;

namespace Uniceps.app.Services.TelegramMachineState.State
{
    public class IBotInterfaces
    {
        public interface IStepProcessor
        {
            BotStep TargetStep { get; }
            Task ProcessMessageAsync(TelegramUserState user, Message message);
            Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query);
        }

        public interface IStateViewRenderer
        {
            BotStep TargetStep { get; }
            Task RenderAsync(TelegramUserState user);
        }
    }
}
