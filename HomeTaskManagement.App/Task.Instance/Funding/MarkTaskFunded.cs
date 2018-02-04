using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class MarkTaskFunded
    {
        public string Id { get; set; }
        public UnixUtcTime At { get; set; }
        public string ByUserId { get; set; }
    }
}
