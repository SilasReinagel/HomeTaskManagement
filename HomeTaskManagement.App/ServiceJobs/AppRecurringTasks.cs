using HomeTaskManagement.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.ServiceJobs
{
    public sealed class AppRecurringTasks : IDisposable
    {
        private readonly List<RecurringScheduledTask> _tasks;
        
        public AppRecurringTasks(params RecurringScheduledTask[] tasks)
        {
            _tasks = tasks.ToList();
        }

        public void Dispose()
        {
            _tasks.ForEach(x => x.Dispose());
            _tasks.Clear();
        }

        public void Start()
        {
            _tasks.ForEach(x => x.Start());
        }
    }
}
