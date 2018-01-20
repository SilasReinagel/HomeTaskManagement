using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public static class EventStoreExtensions
    {
        public static void Commit(this IEventStore events, params Event[] e)
        {
            events.Commit(e);
        }
    }
}
