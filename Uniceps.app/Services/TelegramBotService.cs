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
using Uniceps.Entityframework.Services.SystemSubscriptionServices;

namespace Uniceps.app.Services
{
    public class TelegramBotService
    {
        private readonly ITelegramBotClient _bot;
        private readonly ITelegramUserStateDataService<TelegramUserState> _telegramUserStateDataService;
        private readonly IIntDataService<PaymentGateway> _gatewayService;
        private readonly ICashRequest _paymentRequestService;
        private readonly IMembershipDataService _getByUserId;
        private readonly UserManager<AppUser> _userManager;
        private readonly EmailService _emailService;
        private readonly IOTPGenerateService<OTPModel> _otpGenerateService;
        private readonly IBypassService _bypassService;
        private readonly IConfiguration _config;
        public TelegramBotService(ITelegramUserStateDataService<TelegramUserState> telegramUserStateDataService, IIntDataService<PaymentGateway> gatewayService, ICashRequest paymentRequestService, IMembershipDataService getByUserId, UserManager<AppUser> userManager, EmailService emailService, IOTPGenerateService<OTPModel> otpGenerateService, IBypassService bypassService, IConfiguration config)
        {
            _config = config;
            string botToken= _config.GetValue<string>("Telegram:Token")!;
            _bot = new TelegramBotClient(botToken);
            _telegramUserStateDataService = telegramUserStateDataService;
            _gatewayService = gatewayService;
            _paymentRequestService = paymentRequestService;
            _getByUserId = getByUserId;
            _userManager = userManager;
            _emailService = emailService;
            _otpGenerateService = otpGenerateService;
            _bypassService = bypassService;
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallback(update.CallbackQuery!);
                return;
            }
            if (update.Type == UpdateType.Message && update.Message!.Photo != null)
            {
                await HandleWaitingReceiptImage(await _telegramUserStateDataService.GetOrCreateAsync(update.Message.Chat.Id), update.Message);
                return;
            }
            // 2) معالجة الرسائل النصية
            if (update.Type == UpdateType.Message && update.Message!.Text != null)
            {
                await HandleMessage(update.Message);
                return;
            }
        }
        private async Task HandleCallback(CallbackQuery query)
        {
            var chatId = query.Message!.Chat.Id;
            var user = await _telegramUserStateDataService.GetOrCreateAsync(chatId);
            if (string.IsNullOrEmpty(query.Data))
            {
                await _bot.AnswerCallbackQuery(query.Id);
                return;
            }
            if (query.Data.StartsWith("sub_"))
            {
                var subIdStr = query.Data.Replace("sub_", "");
                if (Guid.TryParse(subIdStr, out var subscriptionId))
                {
                    user.SubscriptionId = subscriptionId.ToString();
                    user.Step = BotStep.ChoosingGateway;
                    await _telegramUserStateDataService.UpdateAsync(user);

                    var gateways = (await _gatewayService.GetAll()).ToList();
                    var buttons = gateways
                        .Select(g => InlineKeyboardButton.WithCallbackData(g.Name, $"method_{g.Id}"))
                        .Select(b => new[] { b })
                        .ToArray();

                    var keyboard = new InlineKeyboardMarkup(buttons);
                    await _bot.SendMessage(chatId, "اختر بوابة الدفع:", replyMarkup: keyboard);
                }
            }

            if (query.Data.StartsWith("method_"))
            {
                var methodIdStr = query.Data.Replace("method_", "");
                if (int.TryParse(methodIdStr, out var gatewayId))
                {
                    user.PaymentGatewayId = gatewayId;
                    user.Step = BotStep.WaitingTransferCode;
                    await _telegramUserStateDataService.UpdateAsync(user);

                    var gateway = await _gatewayService.Get(gatewayId);
                    await _bot.SendMessage(chatId, $"اخترت: {gateway?.Name}\n\n{gateway?.TransferInfo}\n\nالآن أرسل رقم الحوالة:");
                }
            }

            if (query.Data == "confirm_payment")
            {
                var req = new CashPaymentRequest
                {
                    ChatId = chatId,
                    Email = user.Email!,
                    PaymentGatewayId = user.PaymentGatewayId,
                    SubscriptionId = user.SubscriptionId,
                    TransferCode = user.TransferCode!,
                    Amount = user.Amount,
                    ReceiptFileId = user.ReceiptFileId,
                    CreatedAt = DateTime.UtcNow
                };

                await _paymentRequestService.Create(req);

                user.Step = BotStep.Done;
                await _telegramUserStateDataService.UpdateAsync(user);

                await _bot.SendMessage(chatId, "تم إرسال طلبك بنجاح 👍 سيتم مراجعته قريباً.");
            }
            if (query.Data == "cancel_payment")
            {
                await ResetUserState(chatId);
            }
            await _bot.AnswerCallbackQuery(query.Id); // يخفي الدوّامة عند الضغط

        }
        private async Task HandleMessage(Message message)
        {
            var chatId = message.Chat.Id;
            var text = message.Text?.Trim() ?? string.Empty;
         
            var user = await _telegramUserStateDataService.GetOrCreateAsync(chatId);
            if (text.Equals("/cancel", StringComparison.OrdinalIgnoreCase))
            {
                await ResetUserState(chatId);
                return;
            }
            if (text.Equals("/start", StringComparison.OrdinalIgnoreCase))
            {
                await HandleStartAsync(user);
                return;
            }
            if (text.Equals("/invoice", StringComparison.OrdinalIgnoreCase))
            {
                await ShowUserSubscriptionsOrAskEmail(user);
                return;
            }

            // Timeout
            if (DateTime.UtcNow - user.LastUpdated > TimeSpan.FromMinutes(15))
            {
                await ResetUserState(chatId);
                return;
            }

            // Flow
            switch (user.Step)
            {
                case BotStep.Start:
                    await HandleStartAsync(user);
                    break;

                case BotStep.WaitingEmail:
                    await HandleWaitingEmail(user, text);
                    break;

                case BotStep.WaitingOtp:
                    if (!int.TryParse(text, out _))
                    {
                        await _bot.SendMessage(chatId, "رجاءً أدخل كود تحقق صحيح أو أرسل /cancel.");
                        return;
                    }
                    await HandleEmailVerification(user, text);
                    break;

                case BotStep.WaitingTransferCode:
                    user.TransferCode = text;
                    user.Step = BotStep.WaitingReceiptImage;
                    user.LastUpdated = DateTime.UtcNow;
                    await _telegramUserStateDataService.UpdateAsync(user);
                    await _bot.SendMessage(chatId, "تم استلام رقم الحوالة. أرسل صورة الإيصال الآن.");
                    break;

                case BotStep.Done:
                    await _bot.SendMessage(chatId, "تم استلام طلبك، بانتظار المراجعة.");
                    break;

                default:
                    await _bot.SendMessage(chatId, "أرسل /start للبدء أو /invoice لعرض اشتراكاتك.");
                    break;
            }

        }
        private async Task ShowUserSubscriptionsOrAskEmail(TelegramUserState user)
        {
            // إذا عنده إيميل محفوظ
            if (!string.IsNullOrWhiteSpace(user.Email))
            {
                var appUser = await _userManager.FindByEmailAsync(user.Email);
                if (appUser == null)
                {
                    await _bot.SendMessage(user.ChatId, "لا يمكن إيجاد حسابك. أرسل بريدك مرة أخرى بـ /start.");
                    user.Step = BotStep.Start;
                    user.LastUpdated = DateTime.UtcNow;
                    await _telegramUserStateDataService.UpdateAsync(user);
                    return;
                }

                // جلب الاشتراكات
                var subscriptions = await _getByUserId.GetByUserIdListAsync(appUser.Id);
                if (!subscriptions.Any())
                {
                    await _bot.SendMessage(user.ChatId, "لا يوجد لديك اشتراكات حالياً.");
                    return;
                }

                // بناء أزرار لكل اشتراك
                var buttons = subscriptions
                    .Select(s => InlineKeyboardButton.WithCallbackData(
                        $"{s.PlanName} - {s.Price}$ / {s.PlanDuration}",
                        $"sub_{s.NID}"
                    ))
                    .Select(b => new[] { b })
                    .ToArray();

                var keyboard = new InlineKeyboardMarkup(buttons);
                await _bot.SendMessage(user.ChatId, "اختر الاشتراك الذي تريد الدفع عليه:", replyMarkup: keyboard);

                user.Step = BotStep.ChoosingSubscription;
                user.LastUpdated = DateTime.UtcNow;
                await _telegramUserStateDataService.UpdateAsync(user);
            }
            else
            {
                // إذا ما عنده إيميل
                await _bot.SendMessage(user.ChatId, "أرسل بريدك الإلكتروني أولاً بـ /start لتأكيد هويتك.");
                user.Step = BotStep.WaitingEmail;
                user.LastUpdated = DateTime.UtcNow;
                await _telegramUserStateDataService.UpdateAsync(user);
            }
        }
        public async Task HandleStartAsync(TelegramUserState userState)
        {
            string welcomeMessage =
 @"أهلاً 👋 أنا بوت الدفع النقدي الخاص ب Uniceps.
الأوامر الرئيسية:
- /start : بدء العملية من جديد
- /invoice : عرض اشتراكاتك المتاحة
- /cancel : إلغاء العملية الحالية

الرجاء إرسال بريدك الإلكتروني لتأكيد هويتك:";

            await _bot.SendMessage(userState.ChatId, welcomeMessage);
            userState.Step = BotStep.WaitingEmail;
            userState.LastUpdated = DateTime.UtcNow;
            await _telegramUserStateDataService.UpdateAsync(userState);
        }

        public async Task HandleWaitingEmail(TelegramUserState userState, string text)
        {
            if (!IsValidEmail(text))
            {
                await _bot.SendMessage(userState.ChatId, "رجاءً أدخل بريد إلكتروني صحيح.");
                return;
            }
          
            userState.Email = text;
            if (_bypassService.IsTester(userState.Email))
            {
                await _bot.SendMessage(userState.ChatId, $"لقد قمنا بارسال كود التحقق الى بريدك الالكتروني ارسله هنا");

                userState.Step = BotStep.WaitingOtp;
                await _telegramUserStateDataService.UpdateAsync(userState);
                return;
            }

            var otpmodel = await _otpGenerateService.GenerateAsync(userState.Email);
            AppUser? appUser = await _userManager.FindByEmailAsync(userState.Email);
            if (appUser != null)
            {
                await _emailService.SendEmailAsync(otpmodel.Email!, otpmodel.Otp);
                await _bot.SendMessage(userState.ChatId, $"لقد قمنا بارسال كود التحقق الى بريدك الالكتروني ارسله هنا");

                userState.Step = BotStep.WaitingOtp;
                await _telegramUserStateDataService.UpdateAsync(userState);
            }
            else
            {
                await _bot.SendMessage(userState.ChatId, "هذا الحساب غير مسجل لدينا حاليا");
                userState.Step = BotStep.Start;
                await _telegramUserStateDataService.UpdateAsync(userState);
                return;
            }
        }
        public async Task HandleEmailVerification(TelegramUserState userState, string text)
        {
            if (string.IsNullOrEmpty(userState.Email))
            {
                return;
            }
            try
            {
                int otp = Convert.ToInt32(text);
                AppUser? appUser=null;
                if (_bypassService.IsValidTester(userState.Email, otp.ToString()))
                {
                    appUser = await _userManager.FindByEmailAsync(userState.Email!);
                    return;
                }
                else
                {
                    var otpModel = await _otpGenerateService.VerifyAsync(userState.Email!, otp);
                    appUser = await _userManager.FindByEmailAsync(userState.Email!);
                }
                if (appUser == null)
                {
                    await _bot.SendMessage(userState.ChatId, "الحساب غير موجود.");
                    userState.Step = BotStep.Start;
                    await _telegramUserStateDataService.UpdateAsync(userState);
                    return;
                }

                var subscriptions = await _getByUserId.GetByUserIdListAsync(appUser.Id);
                if (!subscriptions.Any())
                {
                    await _bot.SendMessage(userState.ChatId, "لا يوجد لديك اشتراكات.");
                    return;
                }

                var buttons = subscriptions
                    .Select(s => InlineKeyboardButton.WithCallbackData($"{s.PlanName} - {s.Price}$", $"sub_{s.NID}"))
                    .Select(b => new[] { b })
                    .ToArray();

                var keyboard = new InlineKeyboardMarkup(buttons);
                await _bot.SendMessage(userState.ChatId, "اختر الاشتراك الذي تريد الدفع عليه:", replyMarkup: keyboard);

                userState.Step = BotStep.ChoosingSubscription;
                await _telegramUserStateDataService.UpdateAsync(userState);
            }
            catch(Exception ex)
            {
                await _bot.SendMessage(userState.ChatId, ex.Message);
            }
        }
        public async Task HandleWaitingReceiptImage(TelegramUserState userState, Message message)
        {
            try
            {
                if (userState.Step != BotStep.WaitingReceiptImage)
                    return;
                if (message.Photo == null)
                {
                    await _bot.SendMessage(userState.ChatId, "رجاءً أرسل صورة الإيصال فقط.");
                    return;
                }
                var fileId = message.Photo.Last().FileId;
                userState.ReceiptFileId = fileId;
                userState.Step = BotStep.WaitingConfirmation;
                await _telegramUserStateDataService.UpdateAsync(userState);
                var confirmKeyboard = new InlineKeyboardMarkup(new[]
                {
        new[] { InlineKeyboardButton.WithCallbackData("✔ تأكيد الطلب", "confirm_payment") },
        new[] { InlineKeyboardButton.WithCallbackData("❌ إلغاء", "cancel_payment") }
    });
                await _bot.SendMessage(userState.ChatId, "استلمت صورة الإيصال.\nاضغط تأكيد لإرسال الطلب 🚀", replyMarkup: confirmKeyboard);
            }
            catch (Exception ex)
            {
                await _bot.SendMessage(userState.ChatId, ex.Message);
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var _ = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch { return false; }
        }
        private async Task ResetUserState(long chatId)
        {
            var user = await _telegramUserStateDataService.GetOrCreateAsync(chatId);
            user.Step = BotStep.Start;
            user.Email = null;
            user.PaymentGatewayId = 0;
            user.TransferCode = null;
            user.Amount = null;
            user.ReceiptFileId = null;
            await _telegramUserStateDataService.UpdateAsync(user);

            await _bot.SendMessage(chatId, "تم إلغاء العملية. أرسل /start لبدء جديد.");
        }

        public async Task HandelRequestDenied(long chatId)
        {
            await _bot.SendMessage(chatId, "عذرا تم رفض الطلب حاول مرة اخرى");
        }
        public async Task HandelRequestAccepted(long chatId)
        {
            await _bot.SendMessage(chatId, "تم تفعيل الاشتراك .... اهلا بك عالم uniceps");
        }
    }
}
