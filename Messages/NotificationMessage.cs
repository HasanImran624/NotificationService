using System.Text.Json;

namespace NotificationService.Messages
{
    public abstract class NotificationMessage
    {
        public string MessageId { get; set; }
    }
}
