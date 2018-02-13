using System;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Requests
{
    public sealed class ParameterlessRequest : IRequest
    {
        private readonly Func<AppActor, Response> _getResponse;

        public ParameterlessRequest(Func<object> getContent) 
            : this(() => Response.Success(getContent())) {}

        public ParameterlessRequest(Func<Response> getResponse)
            : this(actor => getResponse()) { }

        public ParameterlessRequest(Func<AppActor, Response> getResponse)
        {
            _getResponse = getResponse;
        }

        public Response GetResponse(RequestParams requestParams)
        {
            return _getResponse(requestParams.Actor);
        }
    }
}
