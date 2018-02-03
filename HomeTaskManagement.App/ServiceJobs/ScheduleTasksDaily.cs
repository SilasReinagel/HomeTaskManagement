using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Instance;
using System;

namespace HomeTaskManagement.App.ServiceJobs
{
    public sealed class ScheduleTasksDaily : RecurringScheduledTask
    {
        public ScheduleTasksDaily(TaskInstances taskInstances)
            : this(Clock.UnixUtcNow.StartOfDay().Plus(TimeSpan.FromDays(1)), taskInstances) { }

        public ScheduleTasksDaily(UnixUtcTime firstExecution, TaskInstances taskInstances)
            : base(firstExecution, TimeSpan.FromDays(1),
                  () => taskInstances.Apply(new ScheduleWorkItemsThrough(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(1))))) { }
    }
}
