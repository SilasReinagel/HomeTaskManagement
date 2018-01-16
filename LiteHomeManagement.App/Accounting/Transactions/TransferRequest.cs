
namespace LiteHomeManagement.App.Accounting.Transactions
{
    public sealed class TransferRequest
    {
        public string SourceAccountId { get; set; }
        public string TargetAccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public TransferRequest(string src, string dest, string description, decimal amount)
        {
            SourceAccountId = src;
            TargetAccountId = dest;
            Description = description;
            Amount = amount;
        }
    }
}
