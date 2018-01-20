using System.Linq;
using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Task
{
    public sealed class Tasks
    {
        private readonly IEntityStore<TaskRecord> _taskRecords;

        public Tasks(IEntityStore<TaskRecord> taskRecords)
        {
            _taskRecords = taskRecords;
        }

        public Response Create(CreateTask req)
        {
            _taskRecords.Put(req.Id, req.ToRecord());
            return Response.Success;
        }

        public Response Delete(DeleteTask req)
        {
            _taskRecords.Remove(req.Id);
            return Response.Success;
        }

        public bool Exists(string taskId)
        {
            return _taskRecords.GetAll().Any(x => x.Id.Matches(taskId));
        }
    }
}
