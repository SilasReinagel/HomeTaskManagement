using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Assignment
{
    public sealed class ProposedAssignedTaskInstance
    {
        public string Id => new Sha256Hash($"{TaskId}-{UserId}-{Due.Millis.ToString()}");
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public UnixUtcTime Due { get; set; }
        public decimal Price { get; set; }
    }
}
