namespace NotificationService.Messages
{
    public class PushNotification : NotificationMessage
    {
        public string DeviceToken { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
