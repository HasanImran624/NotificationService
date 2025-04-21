namespace NotificationService.Consumers
{
    using MassTransit;
    using NotificationService.Messages;
    using NotificationService.Services.Idempotency;
    using NotificationService.Services.Push;
    using System;
    using System.Threading.Tasks;

    public class PushConsumer : IConsumer<PushNotification>
    {
        private readonly PushProviderManager _pushProviderManager;
        private readonly IIdempotencyService _idempotencyService;
        public PushConsumer(PushProviderManager pushProviderManager, IIdempotencyService idempotencyService)
        {
            _pushProviderManager = pushProviderManager;
            _idempotencyService = idempotencyService;
        }
        public async Task Consume(ConsumeContext<PushNotification> context)
        {
            var msg = context.Message;

            if (_idempotencyService.IsDuplicate(msg.MessageId.ToString()))
            {
                Console.WriteLine($"Duplicate message skipped: {msg.MessageId}");
                return;
            }
            var sent = await _pushProviderManager.SendPushAsync(msg.Body);

            if (sent)
            {
                _idempotencyService.MarkProcessed(msg.MessageId.ToString());
            }
        }
    }

}
