using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System;
using System.Linq;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class MarkNotCompletedTasksDaily : RecurringScheduledTask
    {
        public MarkNotCompletedTasksDaily(TaskInstances taskInstances)
            : this(Clock.UnixUtcNow.StartOfDay().Plus(TimeSpan.FromDays(1)), 
                  () => taskInstances
                        .ActiveItemsDueBefore(Clock.UnixUtcNow.Minus(TimeSpan.FromDays(2)))
                        .Select(x => taskInstances.Apply(
                            new MarkTaskNotComplete { Id = x.Id, At = Clock.UnixUtcNow, ApproverUserId = new ServiceUser() }))) { }

        public MarkNotCompletedTasksDaily(UnixUtcTime firstExecution, Action task) 
            : base(firstExecution, TimeSpan.FromDays(1), task) { }        
    }
}
