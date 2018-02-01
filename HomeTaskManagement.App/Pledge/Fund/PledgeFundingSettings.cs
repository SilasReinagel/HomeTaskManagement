using System;

namespace HomeTaskManagement.App.Pledge
{
    public sealed class PledgeFundingSettings
    {
        public TimeSpan Frequency { get; } = TimeSpan.FromDays(7);
        public int RatePerUnit { get; } = 11;
        public string TargetAccount { get; } = PoolAccounts.TreasuryAccountId;
    }
}
