using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Assignment
{
    public sealed class AssignedTask
    {
        private SortedList<UnixUtcTime, string> _assignedUsers = new SortedList<UnixUtcTime, string>();

        public string Id { get; }

        public AssignedTask(string taskId, IEnumerable<Event> events)
        {
            Id = taskId;
            events.ForEach(Apply);
        }

        public string At(UnixUtcTime time)
        {
            return _assignedUsers.Where(x => !x.Key.IsAfter(time))
                .Select(x => x.Value)
                .LastOrDefault(new DefaultUser());
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
