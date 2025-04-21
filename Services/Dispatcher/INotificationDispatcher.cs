using NotificationService.Models;

namespace NotificationService.Services.Dispatcher
{
    public interface INotificationDispatcher
    {
        Task DispatchAsync(NotificationRequest request);
    }
}
