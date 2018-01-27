using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public interface IEventStore
    {
        void Commit(IEnumerable<Event> events);
        IEnumerable<Event> GetEvents<T>(string entityId);
        IDictionary<string, IEnumerable<Event>> GetEvents<T>();
    }
}
