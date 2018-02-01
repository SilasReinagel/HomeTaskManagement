using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Task.Assignment
{
    public sealed class AssignedTask
    {
        private readonly TaskRecord _task;
        private readonly TaskRates _rates;
        private readonly AssignmentSettings _settings;
        private readonly History<string> _assignedUsers = new History<string>(new DefaultUser());
        
        public AssignedTask(TaskRecord task, TaskRates rates, AssignmentSettings settings, IEnumerable<Event> events)
        {
            _task = task;
            _rates = rates;
            _settings = settings;
            events.ForEach(Apply);
        }

        public string ToUser(UnixUtcTime time)
        {
            return _assignedUsers.At(time);
        }

        public IEnumerable<ProposedAssignedTaskInstance> FutureInstancesThrough(UnixUtcTime time)
        {
            var from = _task.Frequency == TaskFrequency.Weekly
                ? Clock.UnixUtcNow.Next(_settings.WeekEndDeadline)
                : Clock.UnixUtcNow;
            return from
                .StartOfDay()
                .Every(TimeSpan.FromDays((int)_task.Frequency))
                .Where(x => x.IsAfter(Clock.UnixUtcNow))
                .Until(time)
                .Select(due => new ProposedAssignedTaskInstance
                {
                    TaskId = _task.Id,
                    UserId = _assignedUsers.At(time),
                    Price = _rates.GetInstanceRate(_task),
                    Due = due.Plus(_settings.TaskInstanceDeadlineUtcOffset)
                });
        }

        private void Apply(Event e)
        {
            if (e.Name.Matches(nameof(TaskAssigned)))
                Update(e.PayloadAs<TaskAssigned>());
        }

        private void Update(TaskAssigned taskAssigned)
        {
            _assignedUsers.Add(new UnixUtcTime(taskAssigned.AssignmentStart), taskAssigned.UserId);
        }
    }
}
