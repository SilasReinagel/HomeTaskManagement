using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Pledge
{
    public sealed class FundPledges
    {
        public UnixUtcTime Through { get; set; }

        public FundPledges(UnixUtcTime through)
        {
            Through = through;
        }
    }
}
