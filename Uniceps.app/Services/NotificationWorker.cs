
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
                using (var scope = _serviceProvider.CreateScope()) // إنشاء سكوب جديد لكل لفة
                {
                    try
                    {
                        var db = scope.ServiceProvider.GetRequiredService<INotificationDataService>();
                        var sender = scope.ServiceProvider.GetRequiredService<INotificationSender>();

                        // هنا يتم فتح اتصال جديد ونظيف مع قاعدة البيانات الصحيحة
                        var notes = await db.GetNotifications();

                        foreach (var n in notes)
                        {
                            await sender.SendAsync(n.UserId, n.Title, n.Body);
                            await db.RemoveAsync(n.Id);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in Notification Loop");
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
