
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Pledge
{
    public sealed class SetPledge : IConvertToEvent
    {
        public string UserId { get; }
        public int Amount { get; }
        public UnixUtcTime StartsAt { get; }

        public SetPledge(string userId, int amount, UnixUtcTime startsAt)
        {
            UserId = userId;
            Amount = amount;
            StartsAt = startsAt;
        }

        public Event ToEvent()
        {
            return new PledgeAmountSet(UserId, Amount, StartsAt).ToEvent();
        }
    }
}
