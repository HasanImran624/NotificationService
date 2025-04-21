namespace NotificationService.Services.Push
{
    public class FirebasePushProvider : IPushProvider
    {
        public async Task<bool> SendPushAsync(string data)
        {
            Console.WriteLine("Firebase: push attempt failed...");
            await Task.Delay(200);
            return false;
        }
    }
}
