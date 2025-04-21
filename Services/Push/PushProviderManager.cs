using NotificationService.Services.Sms;
using Polly;
using Polly.CircuitBreaker;

namespace NotificationService.Services.Push
{
    public class PushProviderManager
    {
        private readonly List<IPushProvider> _pushProviders;
        private readonly AsyncPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;

        public PushProviderManager(IEnumerable<IPushProvider> pushProviders)
        {
            _pushProviders = pushProviders.ToList();

            _retryPolicy = Policy.
              Handle<TimeoutException>()
              .Or<HttpRequestException>()
              .WaitAndRetryAsync(3, retryAtmp => TimeSpan.FromSeconds(2));

            _circuitBreakerPolicy = Policy.
             Handle<Exception>()
             .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }

        public async Task<bool> SendPushAsync(string data)
        {
            var result = await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    //Provider Fallback :  if Provider A fails, you try Provider B.
                    foreach (var provider in _pushProviders)
                    {
                        try
                        {
                            if (await provider.SendPushAsync(data))
                            {
                                return true;
                            }
                        }
                        catch (BrokenCircuitException)
                        {
                            Console.WriteLine("Circuit is broken, retrying after a delay.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Provider failed: {ex.Message}");
                        }
                    }
                    return false;
                });
            });

            if (!result)
            {
                Console.WriteLine("All SMS providers failed.");
                return false;
            }

            return result;
        }
    }
}
