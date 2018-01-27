using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Assignment
{
    public sealed class AssignTaskRequest : IConvertToEvent
    {
        public string TaskId { get; }
        public string UserId { get; }
        public UnixUtcTime StartsAt { get; }

        public AssignTaskRequest(string taskId, string userId, UnixUtcTime assignmentStart)
        {
            TaskId = taskId;
            UserId = userId;
            StartsAt = assignmentStart;
        }

        public Event ToEvent()
        {
            return new TaskAssigned(TaskId, UserId, StartsAt).ToEvent();
        }
    }
}
