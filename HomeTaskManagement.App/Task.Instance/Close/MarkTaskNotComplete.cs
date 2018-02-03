
namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class MarkTaskNotComplete : ChangeTaskStatus
    {
        public override TaskInstanceStatus NewStatus { get; protected set; } = TaskInstanceStatus.Completed;
    }
}
