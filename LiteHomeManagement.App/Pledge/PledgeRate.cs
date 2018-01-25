using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class PledgeRate
    {
        private readonly int _usd;

        public PledgeRate()
            : this(11) { }

        public PledgeRate(int usd)
        {
            _usd = usd;
        }

        public int GetInstanceRate(UserPledge pledge, UnixUtcTime time)
        {
            return pledge.AmountAt(time) * _usd;
        }
    }
}
