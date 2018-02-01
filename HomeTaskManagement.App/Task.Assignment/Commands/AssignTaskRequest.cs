using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Assignment
{
    public sealed class AssignTask : IConvertToEvent
    {
        public string TaskId { get; }
        public string UserId { get; }
        public UnixUtcTime StartsAt { get; }

        public AssignTask(string taskId, string userId, UnixUtcTime assignmentStart)
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
