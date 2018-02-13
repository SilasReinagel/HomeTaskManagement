using System.Linq;
using HomeTaskManagement.App.Common;
using HomeTaskManagement.App.User;

namespace HomeTaskManagement.App.Requests
{
    public sealed class ApproverOnly : IRequest
    {
        private readonly IRequest _request;

        public ApproverOnly(IRequest request)
        {
            _request = request;
        }

        public Response GetResponse(RequestParams requestParams)
        {
            if (!requestParams.Actor.Roles.Contains(UserRoles.Approver))
                return Response.Errored(ResponseStatus.Unauthorized, "Requires Approver role user");
            return _request.GetResponse(requestParams);
        }
    }
}
