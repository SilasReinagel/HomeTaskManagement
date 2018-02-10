using System;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Commands
{
    public sealed class JsonCommand<T> : ICommand
    {
        private readonly Func<AppActor, T, Response> _getResponse;

        public JsonCommand(Func<T, Response> getResponse)
            : this ((actor, req) => getResponse(req)) { }

        public JsonCommand(Func<AppActor, T, Response> getResponse)
        {
            _getResponse = getResponse;
        }

        public Response Execute(CommandParams commandParams)
        {
            var req = Json.ToObject<T>(commandParams.JsonRequest);
            return _getResponse(commandParams.Actor, req);
        }
    }
}
