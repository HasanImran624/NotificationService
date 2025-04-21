namespace NotificationService.Services.Sms
{
    public interface ISmsProvider
    {
        Task<bool> SendSmsAsync(string smsBody);
    }
}
