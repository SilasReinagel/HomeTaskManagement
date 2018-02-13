using System;
using System.Collections.Generic;
using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Requests
{
    public abstract class AppRequests
    {
        private readonly Dictionary<string, IRequest> _requests;
        private string RequestType { get; }

        protected AppRequests(Dictionary<string, IRequest> requests, string requestType)
        {
            _requests = requests;
            RequestType = requestType;
        }

        public Response Execute(RequestParams requestParams)
        {
            if (!_requests.ContainsKey(requestParams.Name))
                return Response.Errored(ResponseStatus.BadRequest, $"Unknown {RequestType} '{requestParams.Name}'");

            try
            {
                return _requests[requestParams.Name].GetResponse(requestParams);
            }
            catch (ArgumentException e)
            {
                return Response.Errored(ResponseStatus.BadRequest, e.Message);
            }
        }
    }

    public sealed class AppCommands : AppRequests
    {
        public AppCommands(Dictionary<string, IRequest> requests) 
            : base(requests, "command") { }
    }

    public sealed class AppQueries : AppRequests
    {
        public AppQueries(Dictionary<string, IRequest> requests) 
            : base(requests, "query") { }
    }
}
