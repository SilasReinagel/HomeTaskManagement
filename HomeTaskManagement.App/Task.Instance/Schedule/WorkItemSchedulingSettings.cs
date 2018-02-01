using System;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class WorkItemSchedulingSettings
    {
        public TimeSpan WorkItemDeadlineUtcOffset { get; } = TimeSpan.FromHours(-7);
    }
}
