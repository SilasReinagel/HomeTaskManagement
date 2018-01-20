using LiteHomeManagement.App.Accounting.Transactions;
using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Accounting
{
    public sealed class Accounts
    {
        private readonly IEventStore _eventStore;

        public Accounts(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public Account Get(string userId)
        {
            return new Account(userId, _eventStore.GetEvents<Account>(userId));
        }

        public Response Apply(TransactionRequest req)
        {
            return Apply(req.AccountId, req);
        }

        public Response Apply(SetOverdraftPolicy req)
        {
            return Apply(req.AccountId, req);
        }

        public Response Apply(TransferRequest req)
        {
            var srcAccount = Get(req.SourceAccountId);
            var destAccount = Get(req.DestinationAccountId);
            var srcEvent = req.ToSourceAccountTransaction();
            var destEvent = req.ToDestinationAccountTransaction();

            var srcEventValid = srcAccount.ValidateProposed(srcEvent);
            var destEventValid = destAccount.ValidateProposed(destEvent);
            if (!srcEventValid || !destEventValid)
                return Response.Errored(ResponseStatus.InvalidState, $"{srcEventValid.IssuesMessage} {destEventValid.IssuesMessage}");

            _eventStore.Commit(srcEvent, destEvent);
            return Response.Success;
        }

        private Response Apply(string accountId, IConvertToEvent req)
        {
            var acc = Get(accountId);
            var e = req.ToEvent();

            var eventValid = acc.ValidateProposed(e);
            if (!eventValid.IsValid)
                return Response.Errored(ResponseStatus.InvalidState, eventValid.IssuesMessage);

            _eventStore.Commit(e);
            return Response.Success;
        }
    }
}
