using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task;
using HomeTaskManagement.App.Task.Instance;
using HomeTaskManagement.App.User;
using HomeTaskManagement.Sql.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace HomeTaskManagement.Sql.Tasks.Instances
{
    [Ignore]
    [TestClass]
    public sealed class TaskInstancesTableTests
    {
        private const string SampleTaskId = "taskId";
        private const string SampleUserId = "userId";

        private UsersTable _users;
        private TasksTable _tasks;
        private TaskInstancesTable _taskInstances;

        [TestInitialize]
        public void Init()
        {
            var db = new SqlDatabase();
            _users = new UsersTable(db);
            _tasks = new TasksTable(db);
            _taskInstances = new TaskInstancesTable(db);
            _users.Put(SampleUserId, new UserRecord { Id = SampleUserId, Name = "name", Username = "username", Roles = new DefaultUserRoles() });
            _tasks.Put(SampleTaskId, new TaskRecord { Id = SampleTaskId, Name = "task", Frequency = TaskFrequency.Daily, Importance = TaskImportance.Normal, UnitsOfWork = 3 });
        }

        [TestCleanup]
        public void CleanUp()
        {
            _users.Remove(SampleUserId);
            _tasks.Remove(SampleTaskId);
        }

        [TestMethod]
        public void TaskInstancesTable_PutGet_Works()
        {
            var sampleTask = TaskInstanceRecord.New(SampleTaskId, "Task Description", SampleUserId, Clock.UnixUtcNow, 50);

            _taskInstances.Put(sampleTask.Id, sampleTask);

            Assert.IsTrue(_taskInstances.GetAll().Any(x => x.Id.Matches(sampleTask.Id)));
            var task = _taskInstances.Get(sampleTask.Id).Value;
            Assert.AreEqual(sampleTask.Id, task.Id);
            Assert.AreEqual(sampleTask.IsFunded, task.IsFunded);
            Assert.AreEqual(sampleTask.Price, task.Price);
            Assert.AreEqual(sampleTask.Status, task.Status);
            Assert.AreEqual(sampleTask.TaskId, task.TaskId);
            Assert.AreEqual(sampleTask.UserId, task.UserId);
            Assert.AreEqual(sampleTask.UpdatedStatusAt, task.UpdatedStatusAt);
            Assert.AreEqual(sampleTask.UpdatedStatusByUserId, task.UpdatedStatusByUserId);
            Assert.AreEqual(sampleTask.Due, task.Due);
            Assert.AreEqual(sampleTask.FundedByUserId, task.FundedByUserId);
            Assert.AreEqual(sampleTask.FundedOn, task.FundedOn);

            _taskInstances.Remove(sampleTask.Id);
        }
    }
}
