namespace NotificationService.Services.Sms
{
    public class TwilioSmsProvider : ISmsProvider
    {
        public async Task<bool> SendSmsAsync(string data)
        {
            Console.WriteLine("Twilio: trying to send SMS...");
            await Task.Delay(200);
            return false; // simulate failure
        }
    }
}
