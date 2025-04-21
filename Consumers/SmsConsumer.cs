namespace NotificationService.Consumers
{
    using MassTransit;
    using NotificationService.Messages;
    using NotificationService.Services.Email;
    using NotificationService.Services.Idempotency;
    using NotificationService.Services.Sms;
    using System;
    using System.Threading.Tasks;

    public class SmsConsumer : IConsumer<SmsNotification>
    {

        private readonly SmsProviderManager _smsManager;
        private readonly IIdempotencyService _idempotencyService;
        public SmsConsumer(SmsProviderManager smsManager, IIdempotencyService idempotencyService)
        {
            _smsManager = smsManager;
            _idempotencyService = idempotencyService;
        }
        public async Task Consume(ConsumeContext<SmsNotification> context)
        {
            var msg = context.Message;

            if (msg.IsOtp)
            {
                var age = DateTime.UtcNow - msg.CreatedAt;
                if (age.TotalSeconds > msg.TtlSeconds)
                {
                    Console.WriteLine("OTP expired. Skipping...");
                    return;
                }
            }


            if (_idempotencyService.IsDuplicate(msg.MessageId.ToString()))
            {
                Console.WriteLine($"Duplicate message skipped: {msg.MessageId}");
                return;
            }
            var sent = await _smsManager.SendSmsAsync(msg.Body);

            if (sent)
            {
                _idempotencyService.MarkProcessed(msg.MessageId.ToString());
            }

        }
    }

}
