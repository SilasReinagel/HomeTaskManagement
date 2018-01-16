
namespace LiteHomeManagement.App.Task
{
    public sealed class CreateTask
    {
        public string Name { get; set; }
        public int UnitsOfWork { get; set; }
        public Importance Importance { get; set; }
        public TaskFrequency Frequency { get; set; }

        public CreateTask(string name, int unitsOfWork, Importance importance, TaskFrequency frequency)
        {
            Name = name;
            UnitsOfWork = unitsOfWork;
            Importance = importance;
            Frequency = frequency;
        }

        public TaskRecord ToRecord()
        {
            return new TaskRecord { Name = Name, UnitsOfWork = UnitsOfWork, Frequency = Frequency, Importance = Importance };
        }
    }
}
