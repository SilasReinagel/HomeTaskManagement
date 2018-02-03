
namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class WaiveTask : ChangeTaskStatus
    {
        public override TaskInstanceStatus NewStatus { get; protected set; } = TaskInstanceStatus.Waived;
    }
}
