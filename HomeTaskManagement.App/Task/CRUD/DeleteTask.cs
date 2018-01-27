
namespace HomeTaskManagement.App.Task
{
    public sealed class DeleteTask
    {
        public string Id { get; set; }

        public DeleteTask(string id)
        {
            Id = id;
        }
    }
}
