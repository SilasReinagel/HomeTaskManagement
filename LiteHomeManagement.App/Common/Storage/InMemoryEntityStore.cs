using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public sealed class InMemoryEntityStore<T> : IEntityStore<T>
    {
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        public IEnumerable<T> GetAll()
        {
            return _items.Values;
        }

        public T Get(string id)
        {
            if (!_items.ContainsKey(id))
                throw new KeyNotFoundException($"Unknown Entity {typeof(T).Name} with Id: {id}");
            return _items[id];
        }

        public string Put(string id, T obj)
        {
            _items[id] = obj;
            return id;
        }

        public void Remove(string id)
        {
            _items.Remove(id);
        }
    }
}
