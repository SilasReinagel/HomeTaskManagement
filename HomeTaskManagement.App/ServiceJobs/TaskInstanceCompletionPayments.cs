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
        private readonly Accounts _accounts;
        private readonly Messages _messages;

        public HandleTaskInstanceCompletionPayments(TaskInstances taskInstances, Accounts accounts, Messages messages)
        {
            _taskInstances = taskInstances;
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

            if (msg.PreviousStatus == TaskInstanceStatus.Scheduled && msg.CurrentStatus == TaskInstanceStatus.Completed)
                _accounts.Apply(new TransferRequest(PoolAccounts.TaskFundingAccountId, taskInstance.UserId,
                    $"Payment for {taskInstance.Description} - {taskInstance.Due}", taskInstance.Price));
            else if (msg.PreviousStatus == TaskInstanceStatus.Scheduled && msg.CurrentStatus == TaskInstanceStatus.Waived)
                _accounts.Apply(new TransferRequest(PoolAccounts.TaskFundingAccountId, taskInstance.UserId,
                    $"Waived fee for {taskInstance.Description} - {taskInstance.Due}", taskInstance.Price));
            else if (msg.PreviousStatus == TaskInstanceStatus.NotCompleted && msg.CurrentStatus == TaskInstanceStatus.Waived)
                _accounts.Apply(new TransferRequest(PoolAccounts.OutsourceAccountId, taskInstance.UserId,
                    $"Waived fee for not completed {taskInstance.Description} - {taskInstance.Due}", taskInstance.Price));
            else if (msg.PreviousStatus == TaskInstanceStatus.Scheduled && msg.CurrentStatus == TaskInstanceStatus.NotCompleted)
                _accounts.Apply(new TransferRequest(PoolAccounts.TaskFundingAccountId, PoolAccounts.OutsourceAccountId,
                    $"Outsourced {taskInstance.Description} - {taskInstance.Due}", taskInstance.Price));
        }
    }
}
