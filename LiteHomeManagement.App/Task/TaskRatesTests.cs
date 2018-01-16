using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LiteHomeManagement.App.Task
{
    [TestClass]
    public sealed class TaskRatesTests
    {
        private TaskRates _rates;

        [TestInitialize]
        public void Init()
        {
            _rates = new TaskRates();
        }

        [TestMethod]
        public void TaskRates_ForWeekly_IsCorrect()
        {
            var stove = new TaskRecord { Frequency = TaskFrequency.Weekly, Name = "Stove", UnitsOfWork = 1 };

            var rate = _rates.GetInstanceRate(stove);

            Assert.AreEqual(13, rate);
        }

        [TestMethod]
        public void TaskRates_ForCritical_IsCorrect()
        {
            var shopping = new TaskRecord { Frequency = TaskFrequency.Weekly, Name = "Shopping", UnitsOfWork = 2, Importance = Importance.Critical };

            var rate = _rates.GetInstanceRate(shopping);

            Assert.AreEqual(52, rate);
        }

        [TestMethod]
        public void TaskRates_ForDaily_IsCorrect()
        {
            var fridge = new TaskRecord { Frequency = TaskFrequency.Daily, Name = "Fridge", UnitsOfWork = 3, Importance = Importance.Normal };

            var rate = _rates.GetInstanceRate(fridge);

            Assert.AreEqual(6, rate);
        }
    }
}
