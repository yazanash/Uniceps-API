using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.NotificationModels;

namespace Uniceps.app.Services.NotificationServices
{
    public class FcmNotificationSender : INotificationSender
    {
        private readonly IUserQueryDataService<UserDevice> _dataService;

        public FcmNotificationSender(IUserQueryDataService<UserDevice> dataService)
        {
            _dataService = dataService;
        }

        public async Task SendAsync(string userId, string title, string body)
        {
            IEnumerable<UserDevice> devices = await _dataService.GetAllByUser(userId);
            if (!devices.Any()) return;

            var tokens = devices.Select(x => x.DeviceToken).ToList();

            var message = new MulticastMessage
            {
                Tokens = tokens,
                Notification = new Notification { Title = title, Body = body }
            };

            await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(message);
        }
    }
}
