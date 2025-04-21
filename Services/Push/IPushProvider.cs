namespace NotificationService.Services.Push
{
    public interface IPushProvider
    {
        Task<bool> SendPushAsync(string data);
    }
}
