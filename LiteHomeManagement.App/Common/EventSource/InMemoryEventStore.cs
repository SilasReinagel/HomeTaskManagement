using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Common
{
    public sealed class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<string, List<Event>> _events = new Dictionary<string, List<Event>>();

        public void Commit(IEnumerable<Event> events)
        {
            events.ToList().ForEach(e => Read(e.EntityId).Add(e));
        }

        public IEnumerable<Event> GetEvents(string entityId)
        {
            return Read(entityId);
        }

        private List<Event> Read(string id)
        {
            if (!_events.ContainsKey(id))
                _events[id] = new List<Event>();
            return _events[id];
        }
    }
}
