
namespace HomeTaskManagement.App.Common
{
    public sealed class Response<T> : Response
    {
        public T Content { get; }

        public Response(T content)
        {
            Content = content;
        }

        public Response(ResponseStatus status, string errorMessage)
            : base(status, errorMessage) { }

        public static implicit operator Response<T>(T content)
        {
            return new Response<T>(content);
        } 
    }

    public abstract class Response
    {
        public ResponseStatus Status { get; }
        public string ErrorMessage { get; }
        internal bool Succeeded => Status == ResponseStatus.Succeeded;

        protected Response()
            : this(ResponseStatus.Succeeded, "") { }

        protected Response(ResponseStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static Response Success() => new Response<string>("No Content");
        public static Response Errored(ResponseStatus status, string errorMessage) => new Response<string>(status, errorMessage);
        public static Response<T> Success<T>(T content) => new Response<T>(content);
        public static Response<T> Errored<T>(ResponseStatus status, string errorMessage) => new Response<T>(status, errorMessage);
    }
}
