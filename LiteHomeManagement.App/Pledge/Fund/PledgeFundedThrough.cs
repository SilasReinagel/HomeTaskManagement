using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class PledgeFundedThrough : IConvertToEvent
    {
        public string UserId { get; set; }
        public int Amount { get; set; }
        public long Timestamp { get; set; }

        public PledgeFundedThrough(string userId, int amount, long timestamp)
        {
            UserId = userId;
            Amount = amount;
            Timestamp = timestamp;
        }

        public Event ToEvent()
        {
            var json = new JsonObjectString()
                .With(nameof(UserId), UserId)
                .With(nameof(Amount), Amount)
                .With(nameof(Timestamp), Timestamp);
            return new Event(nameof(UserPledge), UserId, nameof(PledgeFundedThrough), 1, json, Clock.UnixUtcNow);
        }
    }
}
