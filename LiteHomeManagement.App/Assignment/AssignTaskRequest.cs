using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Assignment
{
    public sealed class AssignTaskRequest : IConvertToEvent
    {
        public string TaskId { get; }
        public string UserId { get; }
        public UnixUtcTime AssignmentStart { get; }

        public AssignTaskRequest(string taskId, string userId, UnixUtcTime assignmentStart)
        {
            TaskId = taskId;
            UserId = userId;
            AssignmentStart = assignmentStart;
        }

        public Event ToEvent()
        {
            return new TaskAssigned(TaskId, UserId, AssignmentStart).ToEvent();
        }
    }
}
