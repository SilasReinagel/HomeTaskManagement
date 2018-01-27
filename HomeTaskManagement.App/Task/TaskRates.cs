using System;

namespace HomeTaskManagement.App.Task
{
    public class TaskRates
    {
        private readonly decimal _perUnitRate;
        private readonly decimal _criticalFactor;

        public TaskRates()
            : this(13, 2) { }

        public TaskRates(int perUnitRate, decimal criticalFactor)
        {
            _perUnitRate = perUnitRate;
            _criticalFactor = criticalFactor;
        }

        public int GetInstanceRate(TaskRecord task)
        {
            var frequencyDivisor = task.Frequency == TaskFrequency.Weekly ? 1 : 7;
            var criticalityFactor = task.Importance == Importance.Critical ? _criticalFactor : 1;
            var rawRate = (_perUnitRate * task.UnitsOfWork * criticalityFactor) / frequencyDivisor;
            return Convert.ToInt32(Math.Round(rawRate, MidpointRounding.AwayFromZero));
        }
    }
}
