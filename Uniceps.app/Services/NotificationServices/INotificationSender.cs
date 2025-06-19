namespace Uniceps.app.Services.NotificationServices
{
    public interface INotificationSender
    {
        Task SendAsync(string userId, string title, string body);
    }
}
