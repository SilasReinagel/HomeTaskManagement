
namespace HomeTaskManagement.App.Task.Instance
{
    public struct TaskInstanceStatusChanged
    {
        public string Id { get; set; }
        public TaskInstanceStatus PreviousStatus { get; set; }
        public TaskInstanceStatus CurrentStatus { get; set; }
    }
}
