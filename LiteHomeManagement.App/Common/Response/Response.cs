
namespace LiteHomeManagement.App.Common
{
    public sealed class Response
    {
        public ResponseStatus Status { get; }
        public string ErrorMessage { get; }
        internal bool Succeeded => Status == ResponseStatus.Succeeded;

        public Response(ResponseStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static Response Success => new Response(ResponseStatus.Succeeded, "");
        public static Response Errored(ResponseStatus status, string errorMessage) => new Response(status, errorMessage);
    }
}
