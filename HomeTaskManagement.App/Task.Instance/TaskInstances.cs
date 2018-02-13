using System;
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
        private readonly Messages _messages;

        public TaskInstances(ITaskInstanceStore store, TaskAssignments assignments, Messages messages)
        {
            _store = store;
            _assignments = assignments;
            _messages = messages;
        }

        public TaskInstanceRecord Get(string id)
        {
            return _store.Get(id).Value;
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

            return UpdateTask(task, req);
        }

        public Response Apply(MarkTaskNotComplete req)
        {
            var task = Get(req.Id);
            if (task.Status != TaskInstanceStatus.Scheduled)
                return Response.Errored(ResponseStatus.InvalidState, $"Task has already been closed.");

            return UpdateTask(task, req);
        }

        public Response Apply(WaiveTask req)
        {
            var task = Get(req.Id);
            if (task.Status == TaskInstanceStatus.Waived || task.Status == TaskInstanceStatus.Completed)
                return Response.Success();            
            if (task.UserId == req.ApproverUserId)
                return Response.Errored(ResponseStatus.InvalidState, $"Task cannot be waived by the assignee.");

            return UpdateTask(task, req);
        }

        private Response UpdateTask(TaskInstanceRecord task, ChangeTaskStatus req)
        {
            var changeMsg = new TaskInstanceStatusChanged { Id = task.Id, PreviousStatus = task.Status, CurrentStatus = req.NewStatus };

            task.Status = req.NewStatus;
            task.UpdatedStatusByUserId = req.ApproverUserId;
            task.UpdatedStatusAt = req.At;
            _store.Put(req.Id, task);

            _messages.Publish(changeMsg);

            return Response.Success();
        }

        private Response Schedule(ProposedTaskInstance task)
        {
            var record = TaskInstanceRecord.From(task);
            _store.Put(task.Id, record);
            _messages.Publish(new TaskInstanceScheduled { Record = record });
            return Response.Success();
        }

        public Response Apply(MarkTaskFunded req)
        {
            var task = Get(req.Id);
            task.FundedByUserId = req.ByUserId;
            task.FundedOn = req.At;
            _store.Put(task.Id, task);
            return Response.Success();
        }
    }
}
