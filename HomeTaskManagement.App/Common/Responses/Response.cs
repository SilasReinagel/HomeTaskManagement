
namespace HomeTaskManagement.App.Common
{
    public sealed class Response : Response<byte[]>
    {
        private new byte[] Content { get; }

        private Response(ResponseStatus status, string errorMessage)
            : base(new byte[0], status, errorMessage) { }

        public new static Response Success => new Response(ResponseStatus.Succeeded, "");
        public new static Response Errored(ResponseStatus status, string errorMessage) => new Response(status, errorMessage);
    }

    public sealed class ContentResponse<T> : Response<T>
    {
        private ContentResponse(T content)
            : base(content) { }

        private ContentResponse(ResponseStatus status, string errorMessage)
            : base(status, errorMessage) { }

        public new static Response<T> Success(T content) => new ContentResponse<T>(content);
        public new static Response<T> Errored(ResponseStatus status, string errorMessage) => new ContentResponse<T>(status, errorMessage);
    }

    public abstract class Response<T>
    {
        public T Content { get; }
        public ResponseStatus Status { get; }
        public string ErrorMessage { get; }
        internal bool Succeeded => Status == ResponseStatus.Succeeded;

        protected Response(T content)
        {
            Content = content;
            Status = ResponseStatus.Succeeded;
            ErrorMessage = "";
        }

        protected Response(ResponseStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        protected Response(T content, ResponseStatus status, string errorMessage)
        {
            Content = content;
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static implicit operator Response<T>(T content)
        {
            return Success(content);
        }

        public static Response<T> Success(T content) => ContentResponse<T>.Success(content);
        public static Response<T> Errored(ResponseStatus status, string errorMessage) => ContentResponse<T>.Errored(status, errorMessage);
    }
}
