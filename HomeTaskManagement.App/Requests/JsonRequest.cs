using System;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Requests
{
    public sealed class JsonRequest<TInput> : IRequest
    {
        private readonly Func<AppActor, TInput, Response> _getResponse;

        public JsonRequest(Func<TInput, Response> getResponse)
            : this((actor, req) => getResponse(req)) { }

        public JsonRequest(Func<AppActor, TInput, Response> getResponse)
        {
            _getResponse = getResponse;
        }

        public Response GetResponse(RequestParams requestParams)
        {
            var req = Json.ToObject<TInput>(requestParams.JsonRequest);
            return _getResponse(requestParams.Actor, req);
        }
    }
}
