using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public static class EventStoreExtensions
    {
        public static void Commit(this IEventStore events, Event e)
        {
            events.Commit(new List<Event> { e });
        }
    }
}
