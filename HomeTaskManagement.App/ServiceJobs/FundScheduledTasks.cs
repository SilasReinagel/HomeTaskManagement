using HomeTaskManagement.App.Accounting;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Instance;

namespace HomeTaskManagement.App.ServiceJobs
{
    public sealed class FundScheduledTasks
    {
        private readonly TaskInstances _taskInstances;
        private readonly Accounts _accounts;
        private readonly Messages _messages;

        public FundScheduledTasks(TaskInstances taskInstances, Accounts accounts, Messages messages)
        {
            _taskInstances = taskInstances;
            _accounts = accounts;
            _messages = messages;
        }

        public void Start()
        {
            _messages.Subscribe<TaskInstanceScheduled>(OnTaskInstanceScheduled, this);
        }

        public void Dispose()
        {
            _messages.Unsubscribe(this);
        }

        private void OnTaskInstanceScheduled(TaskInstanceScheduled msg)
        {
            var task = msg.Record;
            _accounts.Apply(new TransferRequest(task.UserId, PoolAccounts.TaskFundingAccountId,
                    $"Funding for {task.Description} - {task.Due}", task.Price));
            _taskInstances.Apply(new MarkTaskFunded { Id = task.Id, At = Clock.UnixUtcNow, ByUserId = task.UserId });
        }
    }
}
