using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Shopping
{
    public sealed class Treasury
    {
        private readonly string AccountId = PoolAccounts.TreasuryAccountId;
        private IBlobStore _blobStore;
        private Accounts _accounts;

        public Treasury(IBlobStore blobStore, Accounts accounts)
        {
            _blobStore = blobStore;
            _accounts = accounts;
        }

        public Response Apply(RecordExpenditure req)
        {
            _blobStore.Put($"{Clock.UnixUtcNow.Millis}-{req.DocumentName}", req.DocumentBytes());
            return _accounts.Apply(new TransactionRequest(AccountId, $"Expenditure - {req.Description}", -req.Amount));
        }
    }
}
