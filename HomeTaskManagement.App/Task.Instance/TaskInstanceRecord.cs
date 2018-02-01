using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Assignment;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class TaskInstanceRecord
    {
        public string Id => new Sha256Hash($"{TaskId}-{UserId}-{Due.Millis.ToString()}");
        public TaskInstanceStatus Status { get; set; }
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public UnixUtcTime Due { get; set; }
        public decimal Price { get; set; }

        public bool IsFunded { get; set; }
        public UnixUtcTime FundedOn { get; set; }
        public string FundedByUserId { get; set; }

        public string ApprovedByUserId { get; set; }
        public UnixUtcTime ApprovedOn { get; set; }

        public TaskInstanceRecord()
        {
        }

        public static TaskInstanceRecord From(ProposedAssignedTaskInstance proposed)
        {
            return New(proposed.TaskId, proposed.UserId, proposed.Due, proposed.Price);
        }

        public static TaskInstanceRecord New(string taskId, string userId, UnixUtcTime due, decimal price)
        {
            return new TaskInstanceRecord
            {
                Status = TaskInstanceStatus.Scheduled,
                TaskId = taskId,
                UserId = userId,
                Due = due,
                Price = price,

                IsFunded = false,
                FundedOn = new UnixUtcTime(0),
                FundedByUserId = new DefaultUser(),

                ApprovedByUserId = new DefaultUser(),
                ApprovedOn = new UnixUtcTime(0),
            };
        }
    }
}
