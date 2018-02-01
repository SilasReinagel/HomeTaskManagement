using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System.Collections.Generic;

namespace HomeTaskManagement.App.Task.Assignment
{
    public sealed class TaskAssignments
    {
        private readonly IEventStore _eventStore;
        private readonly AssignmentSettings _settings;
        private readonly Tasks _tasks;
        private readonly Users _users;

        public TaskAssignments(IEventStore eventStore, Tasks tasks, Users users, AssignmentSettings settings)
        {
            _eventStore = eventStore;
            _tasks = tasks;
            _users = users;
            _settings = settings;
        }

        public IEnumerable<AssignedTask> GetAll()
        {
            return _eventStore.GetAll(x => new AssignedTask(_tasks.Get(x.Id), _tasks.Rates, _settings, x.Events));
        }

        public AssignedTask ForTask(string taskId)
        {
            return _eventStore.Get(taskId, x => new AssignedTask(_tasks.Get(x.Id), _tasks.Rates, _settings, x.Events));
        }

        public Response Apply(AssignTask req)
        {
            if (!_tasks.Exists(req.TaskId))
                return Response.Errored(ResponseStatus.UnknownEntity, $"Unknown Task {req.TaskId}");
            return _users
                .IfExists(req.UserId)
                .And(req.StartsAt.IsNotPast())
                .Then(() => _eventStore.Commit(req.ToEvent()));
        }
    }
}
