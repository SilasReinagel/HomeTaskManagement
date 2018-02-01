using System.Collections.Generic;
using System.Linq;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Assignment;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class TaskInstances
    {
        private readonly ITaskInstanceStore _store;
        private readonly TaskAssignments _assignments;
        private readonly WorkItemSchedulingSettings _settings;

        public TaskInstances(ITaskInstanceStore store, TaskAssignments assignments, WorkItemSchedulingSettings settings)
        {
            _store = store;
            _assignments = assignments;
            _settings = settings;
        }

        public TaskInstanceRecord Get(string id)
        {
            return _store.Get(id);
        }

        public IEnumerable<TaskInstanceRecord> ActiveItemsDueBefore(UnixUtcTime time)
        {
            return _store.GetAll().Where(x => x.Status == TaskInstanceStatus.Scheduled && x.Due.IsBefore(time));
        }

        public Response Apply(ScheduleWorkItemsThrough req)
        {
            return req.Through.IsNotPast()
                .Then(() => _assignments
                    .GetAll()
                    .SelectMany(a => a.FutureInstancesThrough(req.Through))
                    .Select(i => Schedule(i))
                    .Combine());
        }

        public Response Apply(MarkTaskComplete req)
        {
            var task = Get(req.Id);
            if (task.Status != TaskInstanceStatus.Scheduled)
                return Response.Errored(ResponseStatus.InvalidState, $"Task '{req.Id}' has already been closed.");
            if (task.UserId == req.ApproverUserId)
                return Response.Errored(ResponseStatus.InvalidState, $"Task approver must be different than assignee.");

            task.Status = TaskInstanceStatus.Completed;
            task.ApprovedByUserId = req.ApproverUserId;
            task.ApprovedAt = req.At;
            _store.Put(req.Id, task);
            return Response.Success;
        }

        private Response Schedule(ProposedAssignedTaskInstance task)
        {
            _store.Put(task.Id, TaskInstanceRecord.From(task));
            return Response.Success;
        }
    }
}
