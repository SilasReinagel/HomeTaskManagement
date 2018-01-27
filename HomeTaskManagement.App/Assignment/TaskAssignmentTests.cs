using Microsoft.VisualStudio.TestTools.UnitTesting;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task;
using HomeTaskManagement.App.User;
using System;

namespace HomeTaskManagement.App.Assignment
{
    [TestClass]
    public sealed class TaskAssignmentTests
    {
        private TaskAssignments _assignments;
        private Tasks _tasks;
        private Users _users;

        private string WeeklyTaskId = "1";
        private string DailyTaskId = "2";
        private string User1 = "User1";
        private string User2 = "User2";

        [TestInitialize]
        public void Init()
        {
            _tasks = new Tasks(new InMemoryEntityStore<TaskRecord>());
            _users = new Users(new InMemoryEntityStore<UserRecord>());
            _assignments = new TaskAssignments(_tasks, _users, new InMemoryEventStore());
        }

        [TestMethod]
        public void TaskAssignments_AssignUnknownTask_UnknownEntity()
        {
            var resp = _assignments.Apply(new AssignTaskRequest("unknownTaskId", User1, Clock.UnixUtcNow));

            resp.AssertStatusIs(ResponseStatus.UnknownEntity);
        }

        [TestMethod]
        public void TaskAssignments_AssignToUnknownUser_UnknownEntity()
        {
            SetupSampleTasks();

            var resp = _assignments.Apply(new AssignTaskRequest(WeeklyTaskId, "unknownUserId", Clock.UnixUtcNow));

            resp.AssertStatusIs(ResponseStatus.UnknownEntity);
        }

        [TestMethod]
        public void TaskAssignments_AssignTaskBeforeNow_InvalidState()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            var resp = _assignments.Apply(new AssignTaskRequest(WeeklyTaskId, User1, Clock.UnixUtcNow.Minus(TimeSpan.FromHours(1))));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void TaskAssignments_AssignTask_TaskAssigned()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            var resp = _assignments.Apply(new AssignTaskRequest(WeeklyTaskId, User1, Clock.UnixUtcNow.Plus(TimeSpan.FromDays(2))));

            Assert.IsTrue(resp.Succeeded);
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).At(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(2))));
        }

        [TestMethod]
        public void TaskAssignments_AssignTaskToOneUserAndThenAnother_TasksCorrectAtEachDate()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            _assignments.Apply(new AssignTaskRequest(WeeklyTaskId, User1, Clock.UnixUtcNow.Plus(TimeSpan.FromDays(2))));
            _assignments.Apply(new AssignTaskRequest(WeeklyTaskId, User2, Clock.UnixUtcNow.Plus(TimeSpan.FromDays(5))));
            
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).At(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(2))));
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).At(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(3))));
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).At(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(4))));
            Assert.AreEqual(User2, _assignments.ForTask(WeeklyTaskId).At(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(5))));
            Assert.AreEqual(User2, _assignments.ForTask(WeeklyTaskId).At(Clock.UnixUtcNow.Plus(TimeSpan.FromDays(6))));
        }

        private void SetupSampleUsers()
        {
            _users.Create(new CreateUser(User1, "username1", "user1"));
            _users.Create(new CreateUser(User2, "username2", "user2"));
        }

        private void SetupSampleTasks()
        {
            _tasks.Create(new CreateTask(DailyTaskId, "DailyTask", 2, Importance.Normal, TaskFrequency.Daily));
            _tasks.Create(new CreateTask(WeeklyTaskId, "WeeklyTask", 2, Importance.Normal, TaskFrequency.Weekly));
        }
    }
}
