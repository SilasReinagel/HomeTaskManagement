﻿
namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class MarkTaskComplete : ChangeTaskStatus
    {
        public override TaskInstanceStatus NewStatus { get; protected set; } = TaskInstanceStatus.Completed;
    }
}
