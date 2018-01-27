using LiteHomeManagement.App.Common;
using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class UserPledge
    {
        private readonly History<int> _pledgeAmounts = new History<int>(0);
        private readonly History<int> _fundedAmounts = new History<int>(0);
        
        public string UserId { get; }
        public UnixUtcTime FundedThrough => _pledgeAmounts.None() ? Clock.UnixUtcNow
            : _fundedAmounts.None() ? _pledgeAmounts.First().Key
            : _fundedAmounts.Last().Key;

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
            if (e.Name.Matches(nameof(PledgeFundedThrough)))
                Apply(e.PayloadAs<PledgeFundedThrough>());
        }

        private void Apply(PledgeFundedThrough e)
        {
            _fundedAmounts.Add(new UnixUtcTime(e.Timestamp), e.Amount);
        }

        private void Apply(PledgeAmountSet e)
        {
            _pledgeAmounts.Add(new UnixUtcTime(e.StartsAt), e.Amount);
        }
    }
}
