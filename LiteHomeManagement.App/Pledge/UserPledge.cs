using LiteHomeManagement.App.Common;
using System.Collections.Generic;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class UserPledge
    {
        private TimeOrderedSequence<int> _pledgeAmounts = new TimeOrderedSequence<int>(0);

        public string UserId { get; }

        public UserPledge(string userId, IEnumerable<Event> events)
        {
            UserId = userId;
            events.ForEach(Apply);
        }

        public int AmountAt(UnixUtcTime time)
        {
            return _pledgeAmounts.At(time);
        }

        private void Apply(Event e)
        {
            if (e.Name.Matches(nameof(PledgeAmountSet)))
                Apply(e.PayloadAs<PledgeAmountSet>());
        }

        private void Apply(PledgeAmountSet e)
        {
            _pledgeAmounts.Add(new UnixUtcTime(e.StartsAt), e.Amount);
        }
    }
}
