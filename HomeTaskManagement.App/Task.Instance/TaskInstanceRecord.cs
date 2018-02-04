using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Assignment;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class TaskInstanceRecord
    {
        public string Id => new Sha256Hash($"{TaskId}-{UserId}-{Due.Millis.ToString()}");
        public string Description { get; set; }
        public TaskInstanceStatus Status { get; set; }
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public UnixUtcTime Due { get; set; }
        public int Price { get; set; }

        public bool IsFunded { get; set; }
        public UnixUtcTime FundedOn { get; set; }
        public string FundedByUserId { get; set; }

        public string UpdatedStatusByUserId { get; set; }
        public UnixUtcTime UpdatedStatusAt { get; set; }

        public TaskInstanceRecord()
        {
        }

        public static TaskInstanceRecord From(ProposedTaskInstance proposed)
        {
            return New(proposed.TaskId, proposed.TaskDescription, proposed.UserId, proposed.Due, proposed.Price);
        }

        public static TaskInstanceRecord New(string taskId, string taskDescription, string userId, UnixUtcTime due, int price)
        {
            return new TaskInstanceRecord
            {
                Status = TaskInstanceStatus.Scheduled,
                Description = taskDescription,
                TaskId = taskId,
                UserId = userId,
                Due = due,
                Price = price,

                IsFunded = false,
                FundedOn = new UnixUtcTime(0),
                FundedByUserId = new DefaultUser(),

                UpdatedStatusByUserId = new DefaultUser(),
                UpdatedStatusAt = new UnixUtcTime(0),
            };
        }
    }
}
