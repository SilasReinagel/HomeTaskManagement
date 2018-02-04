using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Assignment
{
    public sealed class ProposedTaskInstance
    {
        public string Id => new Sha256Hash($"{TaskId}-{UserId}-{Due.Millis.ToString()}");
        public string TaskDescription { get; set; }
        public string TaskId { get; set; }
        public string UserId { get; set; }
        public UnixUtcTime Start { get; set; }
        public UnixUtcTime Due { get; set; }
        public int Price { get; set; }
    }
}
