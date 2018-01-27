using LiteHomeManagement.App.Accounting;
using LiteHomeManagement.App.Common;
using LiteHomeManagement.App.User;
using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class Pledges
    {
        private readonly IEventStore _eventStore;
        private readonly Users _users;
        private readonly Accounts _accounts;
        private readonly PledgeFundingSettings _settings;

        public Pledges(IEventStore eventStore, Users users, Accounts accounts, PledgeFundingSettings settings)
        {
            _eventStore = eventStore;
            _users = users;
            _accounts = accounts;
            _settings = settings;
        }

        public Response Set(SetPledge req)
        {
            return _users.IfExists(req.UserId)
                .And(req.StartsAt.IsNotPast())
                .Then(() => _eventStore.Commit(req.ToEvent()));
        }

        public UserPledge Get(string userId)
        {
            return new UserPledge(userId, _eventStore.GetEvents<UserPledge>(userId));
        }

        public Response Fund(FundPledges fundPledges)
        {
            return GetAll()
                .Select(x => Fund(x, fundPledges.Through))
                .Combine();
        }

        private Response Fund(UserPledge pledge, UnixUtcTime through)
        {
            var pledgeAmount = pledge.AmountAt(Clock.UnixUtcNow);
            if (pledgeAmount == 0 || !through.IsAfter(pledge.FundedThrough))
                return Response.Success;

            var currencyAmount = _settings.RatePerUnit * pledgeAmount;
            var funded = _accounts.Apply(new TransferRequest(pledge.UserId, _settings.TargetAccount, $"Funded Treasury Pledge - {pledgeAmount} Units", currencyAmount));
            if (!funded.Succeeded)
                return Response.Errored(ResponseStatus.InvalidState, $"Unable to fund pledge for {pledge.UserId}: {funded.ErrorMessage}");

            var fundedThrough = pledge.FundedThrough.Plus(_settings.Frequency);
            _eventStore.Commit(new PledgeFundedThrough(pledge.UserId, pledgeAmount, fundedThrough).ToEvent());
            if (through.IsAfter(fundedThrough))
                Fund(Get(pledge.UserId), through);

            return Response.Success;
        }

        private IEnumerable<UserPledge> GetAll()
        {
            return _eventStore.GetEvents<UserPledge>().Select(x => new UserPledge(x.Key, x.Value));
        }
    }
}
