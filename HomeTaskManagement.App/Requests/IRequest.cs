using HomeTaskManagement.App.Common;

namespace HomeTaskManagement.App.Requests
{
    public interface IRequest
    {
        Response GetResponse(RequestParams requestParams);
    }
}
