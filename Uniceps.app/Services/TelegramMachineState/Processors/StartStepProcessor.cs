using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class StartStepProcessor : IStepProcessor
    {
        private readonly StartFlowHandler _startFlowHandler;

        public BotStep TargetStep => BotStep.Start;

        public StartStepProcessor(StartFlowHandler startFlowHandler)
        {
            _startFlowHandler = startFlowHandler;
        }

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            await _startFlowHandler.HandleStartAsync(user);
        }

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query) => await Task.CompletedTask;
    }
}
