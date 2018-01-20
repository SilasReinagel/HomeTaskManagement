using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Accounting.Transactions
{
    public sealed class TransferRequest
    {
        public string SourceAccountId { get; set; }
        public string DestinationAccountId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }

        public TransferRequest(string src, string dest, string description, decimal amount)
        {
            SourceAccountId = src;
            DestinationAccountId = dest;
            Description = description;
            Amount = amount;
        }

        public Event ToSourceAccountTransaction()
        {
            return new TransactionRequest(SourceAccountId, Description, -Amount).ToEvent();
        }

        public Event ToDestinationAccountTransaction()
        {
            return new TransactionRequest(DestinationAccountId, Description, Amount).ToEvent();
        }
    }
}
