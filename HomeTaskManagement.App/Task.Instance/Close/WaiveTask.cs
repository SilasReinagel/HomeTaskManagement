
namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class WaiveTask : ChangeTaskStatus
    {
        public override TaskInstanceStatus NewStatus { get; set; } = TaskInstanceStatus.Waived;
    }
}
