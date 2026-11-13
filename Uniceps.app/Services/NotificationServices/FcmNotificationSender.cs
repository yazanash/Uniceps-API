using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Services.NotificationSystemServices;

namespace Uniceps.app.Services.NotificationServices
{
    public class FcmNotificationSender : INotificationSender
    {
        private readonly IUserDeviceDataService _dataService;

        public FcmNotificationSender(IUserDeviceDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task SendAsync(string userId, string title, string body)
        {
            var devices = await _dataService.GetAllByUser(userId);
            if (!devices.Any()) return;

            // 1. تصفية التوكنات الصالحة فقط
            var tokens = devices
                .Where(d => !string.IsNullOrEmpty(d.NotifyToken))
                .Select(x => x.NotifyToken!)
                .Distinct() // تجنب التكرار إن وجد
                .ToList();

            if (!tokens.Any()) return;

            // 2. تقسيم التوكنات إلى دفعات لا تتجاوز 500 (حدود Firebase القصوى)
            int batchSize = 500;
            var tokenBatches = tokens
                .Select((token, index) => new { token, index })
                .GroupBy(x => x.index / batchSize)
                .Select(g => g.Select(x => x.token).ToList())
                .ToList();

            var tasks = tokenBatches.Select(async batchTokens =>
            {
                var message = new MulticastMessage
                {
                    Tokens = batchTokens,
                    Notification = new Notification
                    {
                        Title = title,
                        Body = body
                    },
                    Android = new AndroidConfig
                    {
                        Notification = new AndroidNotification
                        {
                            Icon = "ic_stat",
                            Sound = "default" // يُفضل إضافتها لتأكيد الصوت
                        }
                    },
                };

                var response = await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);

                if (response.FailureCount > 0)
                {
                    var failedTokensToCleanup = new List<string>();
                    for (int i = 0; i < response.Responses.Count; i++)
                    {
                        if (!response.Responses[i].IsSuccess)
                        {
                            var errorCode = response.Responses[i].Exception?.MessagingErrorCode;
                            // الأخطاء التي تعني أن التتوكن لم يعد صالحاً ويجب حذفه
                            if (errorCode == MessagingErrorCode.Unregistered ||
                                errorCode == MessagingErrorCode.InvalidArgument)
                            {
                                failedTokensToCleanup.Add(batchTokens[i]);
                            }
                        }
                    }

                    if (failedTokensToCleanup.Any())
                    {
                        // استدعاء ميثود لحذف أو تعطيل هذه التوكنات من قاعدة البيانات لديك
                        await _dataService.RemoveInvalidTokensAsync(failedTokensToCleanup);
                    }
                }
            });

            // تنفيذ جميع الدفعات بشكل متوازي وبأداء عالٍ
            await Task.WhenAll(tasks);
        }
    }
}
