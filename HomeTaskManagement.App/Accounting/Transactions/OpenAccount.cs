using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Accounting
{
    public sealed class OpenAccount : IConvertToEvent
    {
        public string AccountId { get; set; }

        public OpenAccount(string accountId)
        {
            AccountId = accountId;
        }

        public Event ToEvent()
        {
            return new TransactionRequest(AccountId, "Opened Account", 0)
                .ToEvent();
        }
    }
}
