using System.Collections.Generic;

namespace HomeTaskManagement.App.Common
{
    public sealed class InMemoryEntityStore<T> : IEntityStore<T>
    {
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        public IEnumerable<T> GetAll()
        {
            return _items.Values;
        }

        public Maybe<T> Get(string id)
        {
            if (!_items.ContainsKey(id))
                return Maybe<T>.Missing;
            return _items[id];
        }

        public void Put(string id, T obj)
        {
            _items[id] = obj;
        }

        public void Remove(string id)
        {
            _items.Remove(id);
        }
    }
}
