using LiteHomeManagement.App.Common;
using LiteHomeManagement.App.Task;
using LiteHomeManagement.App.User;

namespace LiteHomeManagement.App.Assignment
{
    public sealed class TaskAssignments
    {
        private readonly Tasks _tasks;
        private readonly Users _users;
        private readonly IEventStore _eventStore;

        public TaskAssignments(Tasks tasks, Users users, IEventStore eventStore)
        {
            _tasks = tasks;
            _users = users;
            _eventStore = eventStore;
        }

        public Response Apply(AssignTaskRequest req)
        {
            if (!_tasks.Exists(req.TaskId))
                return Response.Errored(ResponseStatus.UnknownEntity, $"Unknown Task {req.TaskId}");
            return _users
                .IfExists(req.UserId)
                .And(req.StartsAt.IsNotPast())
                .Then(() => _eventStore.Commit(req.ToEvent()));
        }

        public AssignedTask ForTask(string taskId)
        {
            return new AssignedTask(taskId, _eventStore.GetEvents<AssignedTask>(taskId));
        }
    }
}
