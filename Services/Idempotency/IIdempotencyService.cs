namespace NotificationService.Services.Idempotency
{
    public interface IIdempotencyService
    {
        bool IsDuplicate(string messageId);
        void MarkProcessed(string messageId);
    }
}
