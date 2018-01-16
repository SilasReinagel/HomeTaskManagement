
using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Accounting
{
    public sealed class TransactionRequest : IConvertToEvent
    {
        public string AccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public TransactionRequest(string accountId, string description, decimal amount)
        {
            AccountId = accountId;
            Description = description;
            Amount = amount;
        }

        public Event ToEvent()
        {
            var timestamp = Clock.UnixUtcNow;
            var json = new JsonObjectString()
                .With(nameof(Description), Description)
                .With(nameof(Amount), Amount)
                .With(nameof(timestamp), timestamp);
            return new Event(AccountId, nameof(Transaction), 1, json, timestamp);
        }
    }
}
