namespace NotificationService.Messages
{
    public class SmsNotification : NotificationMessage
    {
        public string PhoneNumber { get; set; }
        public string Body { get; set; }
        public bool IsOtp { get; set; } = false;
        public int TtlSeconds { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.MinValue;
    }
}
