using System.Collections.Generic;

namespace HomeTaskManagement.App.Common
{
    public interface IEventStore
    {
        void Commit(IEnumerable<Event> events);
        IEnumerable<Event> GetEvents<T>(string entityId);
        IEnumerable<EventStream> GetEvents<T>();
    }
}
