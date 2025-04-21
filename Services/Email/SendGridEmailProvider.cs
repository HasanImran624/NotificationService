namespace NotificationService.Services.Email
{
    public class SendGridEmailProvider : IEmailProvider
    {
        public async Task<bool> SendEmailAsync(string data)
        {
            Console.WriteLine("SendGrid: trying to send...");
            await Task.Delay(200);
            return false;

        }
    }
}
