
namespace HomeTaskManagement.App.Requests
{
    public sealed class RequestParams
    {
        public AppActor Actor { get; }
        public string Name { get; }
        public string JsonRequest { get; }

        public RequestParams(AppActor actor, string name, string jsonRequest)
        {
            Actor = actor;
            Name = name;
            JsonRequest = jsonRequest;
        }
    }
}
