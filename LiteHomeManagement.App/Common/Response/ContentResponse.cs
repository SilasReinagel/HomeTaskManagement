
namespace LiteHomeManagement.App.Common
{
    public sealed class ContentResponse<T>
    {
        public T Content { get; }
        public ResponseStatus Status { get; }
        public string ErrorMessage { get; }
        internal bool Succeeded => Status == ResponseStatus.Succeeded;

        private ContentResponse(T content)
        {
            Content = content;
            Status = ResponseStatus.Succeeded;
            ErrorMessage = "";
        }

        private ContentResponse(ResponseStatus status, string errorMessage)
        {
            Status = status;
            ErrorMessage = errorMessage;
        }

        public static implicit operator ContentResponse<T>(T content)
        {
            return Success(content);
        }

        public static ContentResponse<T> Success(T content) => new ContentResponse<T>(content);
        public static ContentResponse<T> Errored(ResponseStatus status, string errorMessage) => new ContentResponse<T>(status, errorMessage);
    }
}
