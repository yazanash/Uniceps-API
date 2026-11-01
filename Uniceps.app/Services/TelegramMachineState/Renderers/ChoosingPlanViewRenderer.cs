using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class ChoosingPlanViewRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;
        private readonly IProductDataService _productDataService;
        private readonly IPlanDataService _planDataService;
        private readonly IMembershipDataService _membershipDataService;

        public ChoosingPlanViewRenderer(ITelegramBotClient bot, IProductDataService productDataService,
            IPlanDataService planDataService, IMembershipDataService membershipDataService)
        {
            _bot = bot;
            _productDataService = productDataService;
            _planDataService = planDataService;
            _membershipDataService = membershipDataService;
        }

        public BotStep TargetStep => BotStep.ChoosingPlan;

        public async Task RenderAsync(TelegramUserState userState)
        {
            var product = await _productDataService.GetByAppId(0);
            var plans = await _planDataService.GetPlansForApp(product.Id);
            bool hasHistory = await _membershipDataService.HasUsedTrialForProduct(userState.UserId, product.Id);

            if (hasHistory) plans = plans.Where(p => !p.IsFree).ToList();

            if (!plans.Any())
            {
                await _bot.SendMessage(userState.ChatId, "لايوجد خطط متاحة حاليا");
                return;
            }

            var buttons = plans
                .Select(s => InlineKeyboardButton.WithCallbackData($"{s.Name} - {s.Price}$ ", $"plan_{s.Id}"))
                .Select(b => new[] { b })
                .ToArray();

            var keyboard = new InlineKeyboardMarkup(buttons);
            await _bot.SendMessage(userState.ChatId, "تم العثور على الحساب. خطة الاشتراك :", replyMarkup: keyboard);
        }
    }
}
