using HomeTaskManagement.App.Common;
using System.Linq;

namespace HomeTaskManagement.App.Task
{
    public sealed class Tasks
    {
        private readonly IEntityStore<TaskRecord> _taskRecords;
        public TaskRates Rates { get; } 

        public Tasks(IEntityStore<TaskRecord> taskRecords)
            : this(taskRecords, new TaskRates()) { }

        public Tasks(IEntityStore<TaskRecord> taskRecords, TaskRates rates)
        {
            _taskRecords = taskRecords;
            Rates = rates;
        }

        public bool Exists(string id)
        {
            return _taskRecords.GetAll().Any(x => x.Id.Matches(id));
        }

        public TaskRecord Get(string id)
        {
            return _taskRecords.Get(id).Value;
        }

        public Response Apply(CreateTask req)
        {
            _taskRecords.Put(req.Id, req.ToRecord());
            return Response.Success;
        }

        public Response Apply(DeleteTask req)
        {
            _taskRecords.Remove(req.Id);
            return Response.Success;
        }
    }
}
