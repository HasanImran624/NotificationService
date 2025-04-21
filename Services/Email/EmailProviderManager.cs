using Polly;
using Polly.CircuitBreaker;


namespace NotificationService.Services.Email
{
  
    public class EmailProviderManager
    {
        private readonly List<IEmailProvider> _emailProviders;
        private readonly AsyncPolicy _retryPolicy;
        private readonly AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        public EmailProviderManager(IEnumerable<IEmailProvider> emailProviders)
        {
            _emailProviders = emailProviders.ToList();

            // retry policy : 3 times retry, with 2 seconds delay between attempts
            _retryPolicy = Policy.
                Handle<TimeoutException>()
                .Or<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAtmp => TimeSpan.FromSeconds(2));

            // circuitbreaker policy : break after 3 consective failure and after that delay for 30 secs
            _circuitBreakerPolicy = Policy.
                Handle<Exception>()
                .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30));
        }

        public async Task<bool> SendEmailAsync(string emailBody)
        {
            var result = await _circuitBreakerPolicy.ExecuteAsync(async () =>
            {
                return await _retryPolicy.ExecuteAsync(async() =>
                {
                    //Provider Fallback :  if Provider A fails, you try Provider B.
                    foreach (var provider in _emailProviders)
                    {
                        try
                        {
                            if (await provider.SendEmailAsync(emailBody))
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
                Console.WriteLine("All providers failed.");
                return false;
            }

            return result;
        }
    }
}
