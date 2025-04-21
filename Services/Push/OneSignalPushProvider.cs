namespace NotificationService.Services.Push
{
    public class OneSignalPushProvider : IPushProvider
    {
        public async Task<bool> SendPushAsync(string data)
        {
            Console.WriteLine("OneSignal: fallback push sent.");
            await Task.Delay(200);
            return true;
        }
    }
}
