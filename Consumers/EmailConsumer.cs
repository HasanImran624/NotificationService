namespace NotificationService.Consumers
{
    using MassTransit;
    using NotificationService.Messages;
    using NotificationService.Services.Email;
    using NotificationService.Services.Idempotency;
    using System;
    using System.Threading.Tasks;

    public class EmailConsumer : IConsumer<EmailNotification>
    {
        private readonly EmailProviderManager _emailProviderManager;
        private readonly IIdempotencyService _idempotencyService;
        public EmailConsumer(EmailProviderManager emailProviderManager, IIdempotencyService idempotencyService)
        {
            _emailProviderManager = emailProviderManager;
            _idempotencyService = idempotencyService;
        }
        public async Task Consume(ConsumeContext<EmailNotification> context)
        {
            var msg = context.Message;

            if (_idempotencyService.IsDuplicate(msg.MessageId.ToString()))
            {
                Console.WriteLine($"Duplicate message skipped: {msg.MessageId}");
                return;
            }
            var sent = await _emailProviderManager.SendEmailAsync(msg.Body);

            if (sent)
            {
                _idempotencyService.MarkProcessed(msg.MessageId.ToString());
            }

        }
    }

}
