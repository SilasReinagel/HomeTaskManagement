using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Instance
{
    public abstract class ChangeTaskStatus
    {
        public string Id { get; set; }
        public string ApproverUserId { get; set; }
        public UnixUtcTime At { get; set; }

        public abstract TaskInstanceStatus NewStatus { get; set; }        
    }
}
