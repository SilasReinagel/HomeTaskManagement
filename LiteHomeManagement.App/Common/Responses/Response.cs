
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

    public sealed class Response<T>
    {
        public T Content { get; }
        public ResponseStatus Status { get; }
        public string ErrorMessage { get; }
        internal bool Succeeded => Status == ResponseStatus.Succeeded;

        private Response(T content)
        {
            Content = content;
            Status = ResponseStatus.Succeeded;
            ErrorMessage = "";
        }

        private Response(ResponseStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static implicit operator Response<T>(T content)
        {
            return Success(content);
        }

        public static Response<T> Success(T content) => new Response<T>(content);
        public static Response<T> Errored(ResponseStatus status, string errorMessage) => new Response<T>(status, errorMessage);
    }
}
