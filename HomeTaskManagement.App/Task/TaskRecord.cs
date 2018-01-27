
namespace HomeTaskManagement.App.Task
{
    public sealed class TaskRecord
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UnitsOfWork { get; set; }
        public TaskFrequency Frequency { get; set; }
        public Importance Importance { get; set; }
    }
}
