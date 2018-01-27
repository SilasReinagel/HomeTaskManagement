using HomeTaskManagement.App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HomeTaskManagement.App.Task
{
    [TestClass]
    public class TaskAdministrationTests
    {
        private const string TaskName = "taskname";

        private IEntityStore<TaskRecord> _taskRecords;
        private Tasks _task;

        [TestInitialize]
        public void Init()
        {
            _taskRecords = new InMemoryEntityStore<TaskRecord>();
            _task = new Tasks(_taskRecords);
        }

        [TestMethod]
        public void Tasks_CreateTask_TaskCreated()
        {
            var resp = CreateTask();

            Assert.IsTrue(resp.Succeeded);
            Assert.IsTrue(_taskRecords.Contains(x => x.Name.Equals(TaskName)));
        }

        [TestMethod]
        public void Tasks_DeleteTask_TaskDeleted()
        {
            CreateTask();

            var resp = _task.Delete(new DeleteTask(_taskRecords.GetAll().First().Id));

            Assert.IsTrue(resp.Succeeded);
            Assert.IsFalse(_taskRecords.Contains(x => x.Name.Equals(TaskName)));
        }

        private Response CreateTask()
        {
            return _task.Create(new CreateTask("id", TaskName, 5, Importance.Normal, TaskFrequency.Daily));
        }
    }
}
