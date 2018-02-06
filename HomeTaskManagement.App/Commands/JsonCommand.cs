using System;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Commands
{
    public sealed class JsonCommand<T> : ICommand
    {
        private readonly Func<T, Response> _getResponse;

        public JsonCommand(Func<T, Response> getResponse)
        {
            _getResponse = getResponse;
        }

        public Response Execute(CommandParams commandParams)
        {
            var req = Json.ToObject<T>(commandParams.JsonRequest);
            return _getResponse(req);
        }
    }
}
