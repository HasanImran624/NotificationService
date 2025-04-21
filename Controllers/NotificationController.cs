using Microsoft.AspNetCore.Mvc;
using NotificationService.Models;
using NotificationService.Services.Dispatcher;



[ApiController]
[Route("api/notifications")]
public class NotificationController : ControllerBase
{
    private readonly INotificationDispatcher _notificationDispatcher;

    public NotificationController(INotificationDispatcher notificationDispatcher)
    {
        _notificationDispatcher = notificationDispatcher;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        await _notificationDispatcher.DispatchAsync(request);
        return Ok("Notification queued successfully");
    }
}