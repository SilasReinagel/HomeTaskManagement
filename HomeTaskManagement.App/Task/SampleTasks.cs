using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Task
{
    internal sealed class SampleTasks
    {
        public readonly Tasks Tasks;
        public readonly string UnknownTask = new Id();
        public readonly string WeeklyTask = new Id();
        public readonly string DailyTask = new Id();

        public SampleTasks()
        {
            Tasks = new Tasks(new InMemoryEntityStore<TaskRecord>());
            Tasks.Apply(new CreateTask(WeeklyTask, "WeeklyTask", 4, TaskImportance.Normal, TaskFrequency.Weekly));
            Tasks.Apply(new CreateTask(DailyTask, "DailyTask", 2, TaskImportance.Normal, TaskFrequency.Daily));
        }
    }
}
