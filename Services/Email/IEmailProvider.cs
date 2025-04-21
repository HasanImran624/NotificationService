namespace NotificationService.Services.Email
{
    public interface IEmailProvider
    {
        Task<bool> SendEmailAsync(string data);
    }
}
