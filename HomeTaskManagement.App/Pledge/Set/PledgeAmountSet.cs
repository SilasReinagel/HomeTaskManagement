using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Pledge
{
    public sealed class PledgeAmountSet : IConvertToEvent
    {
        public string UserId { get; set; }
        public int Amount { get; set; }
        public long StartsAt { get; set; }

        public PledgeAmountSet(string userId, int amount, long startsAt)
        {
            UserId = userId;
            Amount = amount;
            StartsAt = startsAt;
        }

        public Event ToEvent()
        {
            var now = Clock.UnixUtcNow;
            var json = new JsonObjectString()
                .With(nameof(UserId), UserId)
                .With(nameof(Amount), Amount)
                .With(nameof(StartsAt), StartsAt);
            return new Event(nameof(UserPledge), UserId, nameof(PledgeAmountSet), 1, json, now);
        }
    }
}
