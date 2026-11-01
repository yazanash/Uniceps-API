using FirebaseAdmin.Messaging;
using Stripe;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Services;
using static Uniceps.app.Services.TelegramMachineState.State.IBotInterfaces;

namespace Uniceps.app.Services.TelegramMachineState.Renderers
{
    public class WaitingTransactionCodeViewRenderer : IStateViewRenderer
    {
        private readonly ITelegramBotClient _bot;
        private readonly IIntDataService<PaymentGateway> _gatewayDataService; // <-- حقن خدمة بوابات الدفع
        private readonly IWebHostEnvironment _env;

        public BotStep TargetStep => BotStep.WaitingTransferCode;

        public WaitingTransactionCodeViewRenderer(
            ITelegramBotClient bot,
             IIntDataService<PaymentGateway> gatewayDataService,
             IWebHostEnvironment env)
        {
            _bot = bot;
            _gatewayDataService = gatewayDataService;
            _env = env;
        }

        public async Task RenderAsync(TelegramUserState userState)
        {
            var gateway = await _gatewayDataService.Get(userState.PaymentGatewayId);

            if (gateway == null)
            {
                await _bot.SendMessage(userState.ChatId, "❌ حدث خطأ، لم يتم العثور على بوابة الدفع. الرجاء البدء من جديد /start");
                return;
            }

            string instructionMessage =
    $@"💳 **تعليمات الدفع عبر: {gateway.Name}**
----------------------------------------
{gateway.TransferInfo}

----------------------------------------
الرجاء إتمام عملية التحويل، ثم إرسال **رقم أو كود الحوالة (Transaction Code)** هنا:";

            if (!string.IsNullOrEmpty(gateway.QrCodeUrl))
            {
                var qrPath = Path.Combine(_env.WebRootPath, gateway.QrCodeUrl.TrimStart('/'));

                if (System.IO.File.Exists(qrPath))
                {
                    using (var stream = System.IO.File.OpenRead(qrPath))
                    {
                        await _bot.SendPhoto(
                            chatId: userState.ChatId,
                            photo: InputFile.FromStream(stream),
                            caption: instructionMessage,
                            parseMode: ParseMode.Markdown
                        );
                    }
                  
                }
                else
                {
                    await _bot.SendMessage(
                                       chatId: userState.ChatId,
                                       text: instructionMessage,
                                       parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                                   );
                }
            }
            else
            {
                await _bot.SendMessage(
                    chatId: userState.ChatId,
                    text: instructionMessage,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
                );
            }
        }
    }
}
