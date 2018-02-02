using HomeTaskManagement.App.Common;
using System;

namespace HomeTaskManagement.App.Pledge
{
    public sealed class FundPledgesDaily : RecurringScheduledTask
    {
        public FundPledgesDaily(Pledges pledges)
            : this(Clock.UnixUtcNow.StartOfDay().Plus(TimeSpan.FromDays(1)), pledges) { }

        public FundPledgesDaily(UnixUtcTime firstExecution, Pledges pledges)
            : base(firstExecution, TimeSpan.FromDays(1), 
                  () => pledges.Apply(new FundPledges(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(1))))) { }
    }
}
