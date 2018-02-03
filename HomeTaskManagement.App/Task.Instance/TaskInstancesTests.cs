using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.Task.Assignment;
using HomeTaskManagement.App.User;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace HomeTaskManagement.App.Task.Instance
{
    [TestClass]
    public sealed class TaskInstancesTests
    {
        private SampleUsers _users;
        private SampleTasks _tasks;
        private SampleTaskAssignments _assignments;
        private TaskInstances _taskInstances;
        private UnixUtcTime _now;
        private Messages _messages;

        [TestInitialize]
        public void Init()
        {
            Clock.Freeze();
            _now = Clock.UnixUtcNow;
            _users = new SampleUsers();
            _tasks = new SampleTasks();
            _assignments = new SampleTaskAssignments(_tasks.Tasks, _users.Users);
            _messages = new Messages();
            _taskInstances = new TaskInstances(new InMemoryTaskInstanceStore(), _assignments.Assignments, _messages);
        }

        [TestMethod]
        public void TaskInstances_ScheduleTasksForPastDate_InvalidState()
        {
            var resp = _taskInstances.Apply(new ScheduleWorkItemsThrough(_now.Plus(TimeSpan.FromDays(-1))));

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void TaskInstances_ScheduleDailyForNextDay_InstanceCorrect()
        {
            _assignments.Assignments.Apply(new AssignTask(_tasks.DailyTask, _users.User1, _now));

            var resp = _taskInstances.Apply(new ScheduleWorkItemsThrough(_now.Plus(TimeSpan.FromDays(1))));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            var active = _taskInstances.ActiveItemsDueBefore(_now.Plus(TimeSpan.FromDays(2)));
            Assert.AreEqual(1, active.Count());
        }

        [TestMethod]
        public void TaskInstances_ScheduleWeeklyForNextWeek_InstanceCorrect()
        {
            _assignments.Assignments.Apply(new AssignTask(_tasks.WeeklyTask, _users.User1, _now));

            var resp = _taskInstances.Apply(new ScheduleWorkItemsThrough(_now.Plus(TimeSpan.FromDays(7))));

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            var active = _taskInstances.ActiveItemsDueBefore(_now.Plus(TimeSpan.FromDays(8)));
            Assert.AreEqual(1, active.Count());
        }

        [TestMethod]
        public void TaskInstances_MarkClosedTaskComplete_InvalidState()
        {
            var taskInstanceId = GetAssignedScheduledTaskId();
            _taskInstances.Apply(new MarkTaskComplete { Id = taskInstanceId, At = _now, ApproverUserId = _users.User2 });
            
            var resp = _taskInstances.Apply(new MarkTaskComplete { Id = taskInstanceId, At = _now, ApproverUserId = _users.User2 });

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void TaskInstances_MarkOwnTaskComplete_InvalidState()
        {
            var taskInstanceId = GetAssignedScheduledTaskId();

            var resp = _taskInstances.Apply(new MarkTaskComplete { Id = taskInstanceId, At = _now, ApproverUserId = _users.User1 });

            resp.AssertStatusIs(ResponseStatus.InvalidState);
        }

        [TestMethod]
        public void TaskInstances_MarkActiveTaskComplete_InstanceCorrect()
        {
            var taskCompletedMessageReceived = false;
            _messages.Subscribe<TaskInstanceStatusChanged>(x => taskCompletedMessageReceived = true, this);
            var taskInstanceId = GetAssignedScheduledTaskId();

            var resp = _taskInstances.Apply(new MarkTaskComplete { Id = taskInstanceId, At = _now, ApproverUserId = _users.User2 });

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(TaskInstanceStatus.Completed, _taskInstances.Get(taskInstanceId).Status);
            Assert.AreEqual(_users.User2, _taskInstances.Get(taskInstanceId).ApprovedByUserId);
            Assert.AreEqual(_now, _taskInstances.Get(taskInstanceId).ApprovedAt);
            Assert.AreEqual(true, taskCompletedMessageReceived);
        }

        [TestMethod]
        public void TaskInstances_WaiveTaskInstance_InstanceCorrect()
        {
            var taskWaivedMessageReceived = false;
            _messages.Subscribe<TaskInstanceStatusChanged>(x => taskWaivedMessageReceived = true, this);
            var taskInstanceId = GetAssignedScheduledTaskId();

            var resp = _taskInstances.Apply(new WaiveTask { Id = taskInstanceId, At = _now, ApproverUserId = _users.User2 });

            resp.AssertStatusIs(ResponseStatus.Succeeded);
            Assert.AreEqual(TaskInstanceStatus.Waived, _taskInstances.Get(taskInstanceId).Status);
            Assert.AreEqual(_users.User2, _taskInstances.Get(taskInstanceId).ApprovedByUserId);
            Assert.AreEqual(_now, _taskInstances.Get(taskInstanceId).ApprovedAt);
            Assert.AreEqual(true, taskWaivedMessageReceived);
        }

        private string GetAssignedScheduledTaskId()
        {
            _assignments.Assignments.Apply(new AssignTask(_tasks.DailyTask, _users.User1, _now));
            _taskInstances.Apply(new ScheduleWorkItemsThrough(_now.Plus(TimeSpan.FromDays(1))));
            return _taskInstances.ActiveItemsDueBefore(_now.Plus(TimeSpan.FromDays(2))).Single().Id;
        }
    }
}
