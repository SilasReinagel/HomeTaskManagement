using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Task.Assignment
{
    internal sealed class SampleTaskAssignments
    {
        public readonly TaskAssignments Assignments;

        public SampleTaskAssignments()
            : this(new SampleTasks().Tasks, new SampleUsers().Users) { }

        public SampleTaskAssignments(Tasks tasks, Users users)
        {
            Assignments = new TaskAssignments(new InMemoryEventStore(), tasks, users, new AssignmentSettings());
        }
    }
}
