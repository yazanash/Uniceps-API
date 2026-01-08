
using Uniceps.app.Services.NotificationServices;
using Uniceps.Entityframework.Services;

namespace Uniceps.app.Services
{
    public class NotificationWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider; 
        private readonly ILogger<NotificationWorker> _logger;

        public NotificationWorker(IServiceProvider serviceProvider, ILogger<NotificationWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Notification Worker started...");

            // حلقة تكرار لضمان بقاء الخدمة تعمل بالخلفية
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // جلب الخدمات داخل السكوب
                        var notificationDataService = scope.ServiceProvider.GetRequiredService<INotificationDataService>();
                        var notificationSender = scope.ServiceProvider.GetRequiredService<INotificationSender>();

                        var notifications = await notificationDataService.GetNotifications();

                        foreach (var notification in notifications)
                        {
                            try
                            {
                                await notificationSender.SendAsync(notification.UserId, notification.Title, notification.Body);
                                await notificationDataService.RemoveAsync(notification.Id);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Failed to send notification to user {UserId}", notification.UserId);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in NotificationWorker loop");
                }

                // الانتظار قبل الجولة القادمة
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
