using System.Linq;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Requests
{
    public sealed class ServiceOrAdmin : IRequest
    {
        private readonly IRequest _request;

        public ServiceOrAdmin(IRequest request)
        {
            _request = request;
        }

        public Response GetResponse(RequestParams requestParams)
        {
            if (!requestParams.Actor.Roles.Contains(UserRoles.Service) && !requestParams.Actor.Roles.Contains(UserRoles.Admin))
                return Response.Errored(ResponseStatus.Unauthorized, "Requires Service or Admin role user");
            return _request.GetResponse(requestParams);
        }
    }
}
