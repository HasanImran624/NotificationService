namespace NotificationService.Services.Sms
{
    public class NexmoSmsProvider : ISmsProvider
    {
        public async Task<bool> SendSmsAsync(string data)
        {
            Console.WriteLine("Nexmo: fallback SMS working...");
            await Task.Delay(200);
            return true;
        }
    }
}
