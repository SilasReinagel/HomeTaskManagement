using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public sealed class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<string, List<Event>> _events = new Dictionary<string, List<Event>>();

        public void Commit(IEnumerable<Event> events)
        {
            events.ForEach(e => Read(e.EntityType, e.EntityId).Add(e));
        }

        public IEnumerable<Event> GetEvents<T>(string entityId)
        {
            return Read(typeof(T).Name, entityId);
        }

        private List<Event> Read(string type, string id)
        {
            var key = $"{type}-{id}";
            if (!_events.ContainsKey(key))
                _events[key] = new List<Event>();
            return _events[key];
        }
    }
}
