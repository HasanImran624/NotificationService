using MassTransit.Middleware;

namespace NotificationService.Services.Email
{
    public class MailgunEmailProvider : IEmailProvider
    {
        public async Task<bool> SendEmailAsync(string emailBody)
        {
            Console.WriteLine("Mailgun: trying to send...");
            await Task.Delay(200);
            return true;

        }
    }
}
