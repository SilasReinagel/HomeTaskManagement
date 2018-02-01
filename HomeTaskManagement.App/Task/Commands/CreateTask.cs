
namespace HomeTaskManagement.App.Task
{
    public sealed class CreateTask
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int UnitsOfWork { get; set; }
        public TaskImportance Importance { get; set; }
        public TaskFrequency Frequency { get; set; }

        public CreateTask(string id, string name, int unitsOfWork, TaskImportance importance, TaskFrequency frequency)
        {
            Id = id;
            Name = name;
            UnitsOfWork = unitsOfWork;
            Importance = importance;
            Frequency = frequency;
        }

        public TaskRecord ToRecord()
        {
            return new TaskRecord { Id = Id, Name = Name, UnitsOfWork = UnitsOfWork, Frequency = Frequency, Importance = Importance };
        }
    }
}
