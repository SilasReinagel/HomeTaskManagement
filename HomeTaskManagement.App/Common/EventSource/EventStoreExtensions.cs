
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public static class EventStoreExtensions
    {
        public static Response Commit(this IEventStore events, params Event[] e)
        {
            events.Commit(e);
            return Response.Success;
        }

        public static IEnumerable<T> GetAll<T>(this IEventStore eventStore, Func<EventStream, T> createEntity)
        {
            return eventStore.GetEvents<T>()
                .Select(createEntity)
                .Finalize();
        }

        public static T Get<T>(this IEventStore eventStore, string id, Func<EventStream, T> createEntity)
        {
            return createEntity(new EventStream { Id = id, Events = eventStore.GetEvents<T>(id) });
        }
    }
}
