using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task;
using HomeTaskManagement.App.Task.Instance;
using System;

namespace HomeTaskManagement.App.ServiceJobs
{
    public sealed class HandleTaskInstanceCompletionPayments : IDisposable
    {
        private readonly TaskInstances _taskInstances;
        private readonly Tasks _tasks;
        private readonly Accounts _accounts;
        private readonly Messages _messages;

        public HandleTaskInstanceCompletionPayments(TaskInstances taskInstances, Tasks tasks, Accounts accounts, Messages messages)
        {
            _taskInstances = taskInstances;
            _tasks = tasks;
            _accounts = accounts;
            _messages = messages;
        }

        public void Start()
        {
            _messages.Subscribe<TaskInstanceStatusChanged>(OnTaskInstanceStatusChanged, this);
        }

        public void Dispose()
        {
            _messages.Unsubscribe(this);
        }

        private void OnTaskInstanceStatusChanged(TaskInstanceStatusChanged msg)
        {
            var taskInstance = _taskInstances.Get(msg.Id);
            var task = _tasks.Get(taskInstance.TaskId);

            if (msg.PreviousStatus == TaskInstanceStatus.Scheduled && msg.CurrentStatus == TaskInstanceStatus.Completed)
                _accounts.Apply(new TransferRequest(PoolAccounts.TaskFundingAccountId, taskInstance.UserId,
                    $"Payment for {task.Name} - {taskInstance.ApprovedAt}", taskInstance.Price));
            else if (msg.PreviousStatus == TaskInstanceStatus.Scheduled && msg.CurrentStatus == TaskInstanceStatus.Waived)
                _accounts.Apply(new TransferRequest(PoolAccounts.TaskFundingAccountId, taskInstance.UserId,
                    $"Waived fee for {task.Name} - {taskInstance.ApprovedAt}", taskInstance.Price));
            else if (msg.PreviousStatus == TaskInstanceStatus.NotCompleted && msg.CurrentStatus == TaskInstanceStatus.Waived)
                _accounts.Apply(new TransferRequest(PoolAccounts.OutsourceAccountId, taskInstance.UserId,
                    $"Waived fee for not completed {task.Name} - {taskInstance.ApprovedAt}", taskInstance.Price));
            else if (msg.PreviousStatus == TaskInstanceStatus.Scheduled && msg.CurrentStatus == TaskInstanceStatus.NotCompleted)
                _accounts.Apply(new TransferRequest(PoolAccounts.TaskFundingAccountId, PoolAccounts.OutsourceAccountId,
                    $"Outsourced {task.Name} - {taskInstance.ApprovedAt}", taskInstance.Price));
        }
    }
}
