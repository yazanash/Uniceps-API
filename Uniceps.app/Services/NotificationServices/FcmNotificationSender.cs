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
            IEnumerable<UserDevice> devices = await _dataService.GetAllByUser(userId);
            if (!devices.Any()) return;

            var tokens = devices.Where(d=>!string.IsNullOrEmpty(d.NotifyToken)).Select(x => x.NotifyToken).ToList();

            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification { Title = title, Body = body },
                Android = new AndroidConfig
                {
                    Notification = new AndroidNotification
                    {
                        Icon = "ic_stat"
                    }
                }
            };

            await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
    }
}
