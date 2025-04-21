using Polly.CircuitBreaker;
using Polly;
using NotificationService.Services.Email;

namespace NotificationService.Services.Sms
{
    public class SmsProviderManager
    {
        private readonly List<ISmsProvider> _smsProviders;
        private readonly AsyncPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;


        public SmsProviderManager(IEnumerable<ISmsProvider> smsProviders)
        {
            _smsProviders = smsProviders.ToList();

            _retryPolicy = Policy.
                Handle<TimeoutException>()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAtmp => TimeSpan.FromSeconds(2));

            _circuitBreakerPolicy = Policy.
             Handle<Exception>()
             .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));

        }

        public async Task<bool> SendSmsAsync(string smsBody)
        {

            var result = await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                return await _retryPolicy.ExecuteAsync(async () =>
                {
                    //Provider Fallback :  if Provider A fails, you try Provider B.
                    foreach (var provider in _smsProviders)
                    {
                        try
                        {
                            if (await provider.SendSmsAsync(smsBody))
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
