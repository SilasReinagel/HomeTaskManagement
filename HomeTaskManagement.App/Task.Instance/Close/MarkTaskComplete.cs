using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class MarkTaskComplete
    {
        public string Id { get; set; }
        public string ApproverUserId { get; set; }
        public UnixUtcTime At { get; set; }
    }
}
