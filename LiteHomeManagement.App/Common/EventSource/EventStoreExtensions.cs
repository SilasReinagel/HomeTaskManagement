
namespace LiteHomeManagement.App.Common
{
    public static class EventStoreExtensions
    {
        public static Response Commit(this IEventStore events, params Event[] e)
        {
            events.Commit(e);
            return Response.Success;
        }
    }
}
