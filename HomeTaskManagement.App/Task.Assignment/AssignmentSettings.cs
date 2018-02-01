using System;

namespace HomeTaskManagement.App.Task.Assignment
{
    public sealed class AssignmentSettings
    {
        public TimeSpan TaskInstanceDeadlineUtcOffset { get; } = TimeSpan.FromHours(14);
        public DayOfWeek WeekEndDeadline { get; } = DayOfWeek.Monday;
    }
}
