using HomeTaskManagement.App.Task;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace HomeTaskManagement.Sql.Tasks
{
    [Ignore]
    [TestClass]
    public sealed class TasksTableTests
    {
        private readonly string SampleTaskId = Guid.Empty.ToString();

        private TasksTable _tasks;

        [TestInitialize]
        public void Init()
        {
            _tasks = new TasksTable(new SqlDatabase());
        }

        [TestCleanup]
        public void CleanUp()
        {
            _tasks.Remove(SampleTaskId);
        }

        [TestMethod]
        public void TasksTable_PutGet_Works()
        {
            _tasks.Put(SampleTaskId, new TaskRecord
                {
                    Id = SampleTaskId,
                    Name = "SampleTask",
                    UnitsOfWork = 3,                  
                    Importance = TaskImportance.Normal,
                    Frequency = TaskFrequency.Daily
                });

            var task = _tasks.Get(SampleTaskId);

            Assert.AreEqual(SampleTaskId, task.Id);
            Assert.AreEqual("SampleTask", task.Name);
            Assert.AreEqual(3, task.UnitsOfWork);
            Assert.AreEqual(TaskImportance.Normal, task.Importance);
            Assert.AreEqual(TaskFrequency.Daily, task.Frequency);
        }
    }
}
