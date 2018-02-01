using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class ScheduleWorkItemsThrough
    {
        public UnixUtcTime Through { get; }

        public ScheduleWorkItemsThrough(UnixUtcTime through)
        {
            Through = through;
        }
    }
}
