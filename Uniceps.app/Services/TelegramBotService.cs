using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Stripe;
using System.Reflection.Metadata;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Uniceps.app.DTOs.AuthenticationDtos;
using Uniceps.app.Services.TesterServices;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;
using Uniceps.Entityframework.Services;
using Uniceps.Entityframework.Services.ProductServices;
using Uniceps.Entityframework.Services.SystemSubscriptionServices;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _bot;
        private readonly ITelegramUserStateDataService<TelegramUserState> _telegramUserStateDataService;
        private readonly IConfiguration _config;
        private readonly Dictionary<BotStep, IStepProcessor> _processors;
        private readonly Dictionary<BotStep, IStateViewRenderer> _renderers;
        public TelegramBotService(ITelegramUserStateDataService<TelegramUserState> telegramUserStateDataService,
             IConfiguration config,
            IEnumerable<IStepProcessor> processors,
        IEnumerable<IStateViewRenderer> renderers,
        ITelegramBotClient bot)
        {
            _config = config;
            string botToken = _config.GetValue<string>("Telegram:Token")!;
            _bot = bot;
            _telegramUserStateDataService = telegramUserStateDataService;

            _processors = processors.ToDictionary(p => p.TargetStep);
            _renderers = renderers.ToDictionary(r => r.TargetStep);
        }

        public async Task HandleUpdate(Update update)
        {
            try
            {

                if (update.Type == UpdateType.Message)
                {
                    var msg = update.Message!;
                    var chatId = msg.Chat.Id;
                    var user = await _telegramUserStateDataService.GetOrCreateAsync(chatId);
                    if (msg.Text?.Equals("/cancel", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        await ResetUserState(chatId);
                        return;
                    }
                    var previousStep = user.Step;
                    if (_processors.TryGetValue(user.Step, out var processor))
                    {
                        await processor.ProcessMessageAsync(user, msg);
                    }

                    if (user.Step != previousStep)
                    {
                        if (_renderers.TryGetValue(user.Step, out var renderer))
                        {
                            await renderer.RenderAsync(user);
                        }
                    }
                }
                else if (update.Type == UpdateType.CallbackQuery)
                {
                    var query = update.CallbackQuery!;
                    var chatId = query.Message!.Chat.Id;
                    var user = await _telegramUserStateDataService.GetOrCreateAsync(chatId);

                    var previousStep = user.Step;

                    if (_processors.TryGetValue(user.Step, out var processor))
                    {
                        await processor.ProcessCallbackAsync(user, query);
                    }

                    if (user.Step != previousStep)
                    {
                        if (_renderers.TryGetValue(user.Step, out var renderer))
                        {
                            await renderer.RenderAsync(user);
                        }
                    }

                    await _bot.AnswerCallbackQuery(query.Id);
                }
            }
            catch
            {
                var chatId = update.Message!.Chat.Id;
                await _bot.SendMessage(chatId, "حدث خطا غير متوقع");
            }
        }

        private async Task ResetUserState(long chatId)
        {
            var user = await _telegramUserStateDataService.GetOrCreateAsync(chatId);
            user.Step = BotStep.Start;
            user.Email = null;
            user.PaymentGatewayId = 0;
            user.TransferCode = null;
            await _telegramUserStateDataService.UpdateAsync(user);

            await _bot.SendMessage(chatId, "تم إلغاء العملية. أرسل /start لبدء جديد.");
        }

        public async Task HandelRequestDenied(long chatId, string? message)
        {
            await _bot.SendMessage(chatId, message ?? "عذرا تم رفض الطلب حاول مرة اخرى");
        }
        public async Task HandelRequestAccepted(long chatId, string? message)
        {
            await _bot.SendMessage(chatId, message ?? "تم تفعيل الاشتراك .... اهلا بك عالم uniceps");
        }
    }
}
