using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Stripe;
using Telegram.Bot;
using Telegram.Bot.Types;
using Uniceps.app.Services.TelegramMachineState.Handlers;
using Uniceps.Entityframework.Models;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Processors
{
    public class ChoosingPlanStepProcessor : IStepProcessor
    {
        private readonly ITelegramBotClient _bot;
        private readonly SelectPlanHandler _selectPlanHandler;

        public BotStep TargetStep => BotStep.ChoosingPlan;

        public ChoosingPlanStepProcessor(ITelegramBotClient bot, SelectPlanHandler selectPlanHandler)
        {
            _bot = bot;
            _selectPlanHandler = selectPlanHandler;
        }

        public async Task ProcessMessageAsync(TelegramUserState user, Message message)
        {
            await _bot.SendMessage(user.ChatId, "الرجاء اختيار الخطة من الأزرار الظاهرة أمامك.");
        }

        public async Task ProcessCallbackAsync(TelegramUserState user, CallbackQuery query)
        {
            if (query.Data?.StartsWith("plan_") == true)
            {
                var subIdStr = query.Data.Replace("plan_", "");
                if (int.TryParse(subIdStr, out int planId))
                {
                    bool isSuccess = await _selectPlanHandler.HandleAsync(planId, user);
                    await _bot.SendMessage(user.ChatId, $"test {isSuccess.ToString()}");
                    if (!isSuccess)
                    {
                        await _bot.SendMessage(user.ChatId, "❌ حدث خطأ في اختيار الخطة، حاول مرة أخرى.");
                    }
                }
            }
        }
    }
}
