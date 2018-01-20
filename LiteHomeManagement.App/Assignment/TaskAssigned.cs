using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Assignment
{
    public sealed class TaskAssigned : IConvertToEvent
    {
        public string TaskId { get; set; }
        public string UserId { get; set;  }
        public long AssignmentStart { get; set; }

        public TaskAssigned(string taskId, string userId, long assignmentStart)
        {
            TaskId = taskId;
            UserId = userId;
            AssignmentStart = assignmentStart;
        }

        public Event ToEvent()
        {
            var json = new JsonObjectString()
                .With(nameof(TaskId), TaskId)
                .With(nameof(UserId), UserId)
                .With(nameof(AssignmentStart), AssignmentStart);
            return new Event(nameof(AssignedTask), TaskId, nameof(TaskAssigned), 1, json, Clock.UnixUtcNow);
        }
    }
}
