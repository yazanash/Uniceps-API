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
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public TelegramBotService(ITelegramUserStateDataService<TelegramUserState> telegramUserStateDataService, IIntDataService<PaymentGateway> gatewayService, ICashRequest paymentRequestService, IMembershipDataService getByUserId, UserManager<AppUser> userManager, IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            string botToken = _config.GetValue<string>("Telegram:Token")!;
            _bot = new TelegramBotClient(botToken);
            _telegramUserStateDataService = telegramUserStateDataService;
            _gatewayService = gatewayService;
            _paymentRequestService = paymentRequestService;
            _getByUserId = getByUserId;
            _userManager = userManager;
            _env = env;
        }

        public async Task HandleUpdate(Update update)
        {
            if (update.Type == UpdateType.CallbackQuery)
            {
                await HandleCallback(update.CallbackQuery!);
                return;
            }
            if (update.Type == UpdateType.Message)
            {
                var msg = update.Message!;
                var user = await _telegramUserStateDataService.GetOrCreateAsync(msg.Chat.Id);

                // إذا أرسل المستخدم صورة وكان في مرحلة انتظار الوصل
                if (msg.Photo != null && user.Step == BotStep.WaitingReceiptImage)
                {
                    await HandleWaitingReceiptImage(user, msg);
                    return;
                }

                // إذا أرسل نصاً (مثل /start أو البريد الإلكتروني)
                if (msg.Text != null)
                {
                    await HandleMessage(msg);
                    return;
                }
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
                    user.Step = BotStep.WaitingReceiptImage;
                    await _telegramUserStateDataService.UpdateAsync(user);

                    var gateway = await _gatewayService.Get(gatewayId);

                    string message = $"💎 بوابة {gateway?.Name}\n\n" +
                                     $"{gateway?.TransferInfo}\n\n" +
                                     "✅ لإتمام الطلب، يرجى إرسال **صورة الوصل** الآن:";

                    if (!string.IsNullOrEmpty(gateway?.QrCodeUrl))
                    {
                        var qrPath = Path.Combine(_env.WebRootPath, gateway.QrCodeUrl.TrimStart('/'));

                        if (System.IO.File.Exists(qrPath))
                        {
                            using (var stream = System.IO.File.OpenRead(qrPath))
                            {
                                await _bot.SendPhoto(
                                    chatId: chatId,
                                    photo: InputFile.FromStream(stream),
                                    caption: message,
                                    parseMode: ParseMode.Markdown
                                );
                            }
                            return; 
                        }
                    }

                    await _bot.SendMessage(chatId, message, parseMode: ParseMode.Markdown);
                }
            }

            if (query.Data == "confirm_payment")
            {
                string localPath = "";
                if (!string.IsNullOrEmpty(user.ReceiptFileId))
                {
                    var file = await _bot.GetFile(user.ReceiptFileId);

                    var fileName = $"{user.ReceiptFileId}.jpg";
                    var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "receipts");

                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var saveFileStream = System.IO.File.OpenWrite(filePath))
                    {
                        await _bot.DownloadFile(file.FilePath!, saveFileStream);
                    }

                    localPath = $"/uploads/receipts/{fileName}";
                }
                var req = new CashPaymentRequest
                {
                    ChatId = chatId,
                    Email = user.Email!,
                    PaymentGatewayId = user.PaymentGatewayId,
                    SubscriptionId = user.SubscriptionId,
                    TransferCode = user.TransferCode ?? "تم ارسال صورة التحويل",
                    Amount = user.Amount,
                    ReceiptFileId = localPath,
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
            if (DateTime.UtcNow - user.LastUpdated > TimeSpan.FromMinutes(15))
            {
                await ResetUserState(chatId);
                return;
            }

            switch (user.Step)
            {
                case BotStep.Start:
                    await HandleStartAsync(user);
                    break;

                case BotStep.WaitingEmail:
                    await HandleWaitingEmail(user, text);
                    break;


                case BotStep.WaitingReceiptImage:
                    // إذا أرسل نصاً بدلاً من صورة، نكتفي بالتنبيه فقط
                    await _bot.SendMessage(chatId, "⚠️ عذراً، يجب إرسال **صورة الوصل** (Attachment) لإتمام العملية، الرسائل النصية غير مقبولة في هذه الخطوة.");
                    break;

                case BotStep.Done:
                    await _bot.SendMessage(chatId, "تم استلام طلبك، بانتظار المراجعة.");
                    break;

                default:
                    await _bot.SendMessage(chatId, "أرسل /start للبدء أو /invoice لعرض اشتراكاتك.");
                    break;
            }

        }
        public async Task HandleStartAsync(TelegramUserState userState)
        {
            string welcomeMessage =
 @"أهلاً 👋 أنا بوت الدفع النقدي الخاص ب Uniceps.
الأوامر الرئيسية:
- /start : بدء العملية من جديد
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
            AppUser? appUser = await _userManager.FindByEmailAsync(userState.Email);

            if (appUser != null)
            {
                var subscriptions = await _getByUserId.GetByUserIdListAsync(appUser.Id);

                if (!subscriptions.Any())
                {
                    await _bot.SendMessage(userState.ChatId, "هذا الحساب لا يملك اشتراكات معلقة حالياً.");
                    userState.Step = BotStep.Start;
                }
                else
                {
                    var buttons = subscriptions
                        .Select(s => InlineKeyboardButton.WithCallbackData($"{s.PlanName} - {s.Price}$ -( {s.StartDate.ToString("MM-yyyy")})", $"sub_{s.NID}"))
                        .Select(b => new[] { b })
                        .ToArray();

                    var keyboard = new InlineKeyboardMarkup(buttons);
                    await _bot.SendMessage(userState.ChatId, "تم العثور على الحساب. اختر الاشتراك المراد تسديده:", replyMarkup: keyboard);
                    userState.Step = BotStep.ChoosingSubscription;
                }
            }
            else
            {
                await _bot.SendMessage(userState.ChatId, "هذا الإيميل غير مسجل في Uniceps. تأكد من الإيميل المحفوظ في تطبيقك.");
                userState.Step = BotStep.Start;
            }
            await _telegramUserStateDataService.UpdateAsync(userState);
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
