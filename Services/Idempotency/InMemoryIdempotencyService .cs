namespace NotificationService.Services.Idempotency
{
    public class InMemoryIdempotencyService : IIdempotencyService
    {
        private static readonly HashSet<string> _processedMessages = new HashSet<string>();
        private static readonly object _lock = new object();
        public bool IsDuplicate(string messageId)
        {
            lock (_lock)
            {
                return _processedMessages.Contains(messageId);
            }
        }
        public void MarkProcessed(string messageId)
        {
            lock (_lock)
            {
                _processedMessages.Add(messageId);
            }
        }
    }
}
