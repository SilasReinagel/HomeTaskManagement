
namespace HomeTaskManagement.App.Commands
{
    public sealed class CommandParams
    {
        public AppActor Actor { get; }
        public string Name { get; }
        public string JsonRequest { get; }

        public CommandParams(AppActor actor, string name, string jsonRequest)
        {
            Actor = actor;
            Name = name;
            JsonRequest = jsonRequest;
        }
    }
}
