using Microsoft.VisualStudio.TestTools.UnitTesting;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;
using System;
using System.Linq;

namespace HomeTaskManagement.App.Task.Assignment
{
    [TestClass]
    public sealed class TaskAssignmentTests
    {
        private TaskAssignments _assignments;
        private AssignmentSettings _settings;
        private Tasks _tasks;
        private Users _users;
        private UnixUtcTime _now;

        private const string WeeklyTaskId = "1";
        private const string DailyTaskId = "2";
        private const string User1 = "User1";
        private const string User2 = "User2";

        [TestInitialize]
        public void Init()
        {
            Clock.Freeze();
            _now = Clock.UnixUtcNow;
            _tasks = new Tasks(new InMemoryEntityStore<TaskRecord>());
            _users = new Users(new InMemoryEntityStore<UserRecord>());
            _settings = new AssignmentSettings();
            _assignments = new TaskAssignments(new InMemoryEventStore(), _tasks, _users, _settings);
        }

        [TestMethod]
        public void TaskAssignments_AssignUnknownTask_UnknownEntity()
        {
            var resp = _assignments.Apply(new AssignTask("unknownTaskId", User1, _now));

            resp.AssertStatusIs(ResponseStatus.UnknownEntity);
        }

        [TestMethod]
        public void TaskAssignments_AssignToUnknownUser_UnknownEntity()
        {
            SetupSampleTasks();

            var resp = _assignments.Apply(new AssignTask(WeeklyTaskId, "unknownUserId", _now));

            resp.AssertStatusIs(ResponseStatus.UnknownEntity);
        }

        [TestMethod]
        public void TaskAssignments_AssignTaskBeforeNow_InvalidState()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            var resp = _assignments.Apply(new AssignTask(WeeklyTaskId, User1, _now.Minus(TimeSpan.FromHours(1))));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void TaskAssignments_AssignTask_TaskAssigned()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            var resp = _assignments.Apply(new AssignTask(WeeklyTaskId, User1, _now.Plus(TimeSpan.FromDays(2))));

            Assert.IsTrue(resp.Succeeded);
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).ToUser(_now.Plus(TimeSpan.FromDays(2))));
        }

        [TestMethod]
        public void TaskAssignments_AssignTaskToOneUserAndThenAnother_TasksCorrectAtEachDate()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            _assignments.Apply(new AssignTask(WeeklyTaskId, User1, _now.Plus(TimeSpan.FromDays(2))));
            _assignments.Apply(new AssignTask(WeeklyTaskId, User2, _now.Plus(TimeSpan.FromDays(5))));
            
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).ToUser(_now.Plus(TimeSpan.FromDays(2))));
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).ToUser(_now.Plus(TimeSpan.FromDays(3))));
            Assert.AreEqual(User1, _assignments.ForTask(WeeklyTaskId).ToUser(_now.Plus(TimeSpan.FromDays(4))));
            Assert.AreEqual(User2, _assignments.ForTask(WeeklyTaskId).ToUser(_now.Plus(TimeSpan.FromDays(5))));
            Assert.AreEqual(User2, _assignments.ForTask(WeeklyTaskId).ToUser(_now.Plus(TimeSpan.FromDays(6))));
        }

        [TestMethod]
        public void TaskAssignments_GetWeeklyInstancesThroughDate_InstanceDetailsCorrect()
        {
            SetupSampleTasks();
            SetupSampleUsers();

            _assignments.Apply(new AssignTask(WeeklyTaskId, User1, _now));

            var assignment = _assignments.ForTask(WeeklyTaskId);

            var instance = assignment.FutureInstancesThrough(_now.Plus(TimeSpan.FromDays(7))).Single();

            Assert.AreEqual(WeeklyTaskId, instance.TaskId);
            Assert.AreEqual("WeeklyTask", instance.TaskDescription);
            Assert.AreEqual(User1, instance.UserId);
            Assert.AreEqual(_tasks.Rates.GetInstanceRate(_tasks.Get(WeeklyTaskId)), instance.Price);
            Assert.AreEqual(_now.Next(_settings.WeekEndDeadline).StartOfDay().Plus(_settings.TaskInstanceDeadlineUtcOffset), instance.Due);
            Assert.AreEqual(_now.Next(_settings.WeekEndDeadline).StartOfDay().Plus(_settings.TaskInstanceDeadlineUtcOffset).Minus(TimeSpan.FromDays(7)), instance.Start);
        }

        [TestMethod]
        public void TaskAssignments_GetDailyInstancesThroughDate_InstanceDetailsCorrect()
        {
            SetupSampleTasks();
            SetupSampleUsers();
            var taskId = DailyTaskId;

            _assignments.Apply(new AssignTask(taskId, User1, _now));

            var assignment = _assignments.ForTask(taskId);

            var instance = assignment.FutureInstancesThrough(_now.Plus(TimeSpan.FromDays(1))).Single();

            Assert.AreEqual(taskId, instance.TaskId);
            Assert.AreEqual(User1, instance.UserId);
            Assert.AreEqual(_tasks.Rates.GetInstanceRate(_tasks.Get(taskId)), instance.Price);
            Assert.AreEqual(_now.Plus(TimeSpan.FromDays(1)).StartOfDay().Plus(_settings.TaskInstanceDeadlineUtcOffset), instance.Due);
        }

        private void SetupSampleUsers()
        {
            _users.Apply(new CreateUser(User1, "username1", "user1"));
            _users.Apply(new CreateUser(User2, "username2", "user2"));
        }

        private void SetupSampleTasks()
        {
            _tasks.Apply(new CreateTask(DailyTaskId, "DailyTask", 2, TaskImportance.Normal, TaskFrequency.Daily));
            _tasks.Apply(new CreateTask(WeeklyTaskId, "WeeklyTask", 2, TaskImportance.Normal, TaskFrequency.Weekly));
        }
    }
}
