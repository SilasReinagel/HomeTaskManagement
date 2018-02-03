using System;
using System.Threading;

namespace HomeTaskManagement.App.Common
{
    public abstract class RecurringScheduledTask : IDisposable
    {
        private readonly Timer _timer;
        private readonly UnixUtcTime _firstExecution;
        private readonly TimeSpan _interval;

        public RecurringScheduledTask(UnixUtcTime firstExecution, TimeSpan interval, Action task)
        {
            _timer = new Timer(x => task(), new object(), -1, -1);
            _firstExecution = firstExecution;
            _interval = interval;
        }

        public void Start()
        {
            _timer.Change(TimeSpan.FromMilliseconds(_firstExecution.Minus(Clock.UnixUtcNow)), _interval);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
