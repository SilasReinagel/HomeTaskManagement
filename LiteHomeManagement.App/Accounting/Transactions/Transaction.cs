
namespace LiteHomeManagement.App.Accounting
{
    public sealed class Transaction
    {
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public long Timestamp { get; set; }

        public Transaction(string description, decimal amount, long timestamp)
        {
            Description = description;
            Amount = amount;
            Timestamp = timestamp;
        }
    }
}
