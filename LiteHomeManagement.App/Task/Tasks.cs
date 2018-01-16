using LiteHomeManagement.App.Common;

namespace LiteHomeManagement.App.Task
{
    public sealed class Tasks
    {
        private readonly IStore<TaskRecord> _taskRecords;

        public Tasks(IStore<TaskRecord> taskRecords)
        {
            _taskRecords = taskRecords;
        }

        public Response Create(CreateTask req)
        {
            _taskRecords.Put(req.ToRecord());
            return Response.Success;
        }

        public Response Delete(DeleteTask req)
        {
            _taskRecords.Remove(req.Id);
            return Response.Success;
        }
    }
}
