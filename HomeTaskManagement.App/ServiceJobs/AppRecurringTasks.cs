using HomeTaskManagement.App.Common;
using System.Collections.Generic;

namespace HomeTaskManagement.App.ServiceJobs
{
    public sealed class AppRecurringTasks
    {
        private readonly IEnumerable<RecurringScheduledTask> _tasks;
        
        public AppRecurringTasks(params RecurringScheduledTask[] tasks)
        {
            _tasks = tasks;
        }

        public void Start()
        {
            _tasks.ForEach(x => x.Start());
        }
    }
}
