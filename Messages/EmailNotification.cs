namespace NotificationService.Messages
{

    public class EmailNotification : NotificationMessage
    {
        public string? To { get; set; }
        public string? Subject { get; set; }
        public string? Body { get; set; }
    }
}
