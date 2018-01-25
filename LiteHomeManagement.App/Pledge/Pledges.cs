using LiteHomeManagement.App.Accounting;
using LiteHomeManagement.App.Common;
using LiteHomeManagement.App.User;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class Pledges
    {
        private readonly IEventStore _eventStore;
        private readonly Users _users;
        private readonly Accounts _accounts;

        public Pledges(IEventStore eventStore, Users users, Accounts accounts)
        {
            _eventStore = eventStore;
            _users = users;
            _accounts = accounts;
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
    }
}
