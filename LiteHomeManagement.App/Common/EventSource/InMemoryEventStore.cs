using System.Collections.Generic;
using System.Linq;

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

        public IDictionary<string, IEnumerable<Event>> GetEvents<T>()
        {
            var keyPrefix = $"{typeof(T).Name}-";
            return _events.Where(x => x.Key.StartsWith(keyPrefix))                
                .ToDictionary(x => x.Key.Replace(keyPrefix, ""), x => (IEnumerable<Event>)x.Value);
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
