using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public sealed class InMemoryStore<T> : IStore<T>
    {
        private int _nextId = 0;
        private readonly Dictionary<string, T> _items = new Dictionary<string, T>();

        public T Get(string id)
        {
            if (!_items.ContainsKey(id))
                throw new KeyNotFoundException($"Unknown Entity {typeof(T).Name} with Id: {id}");
            return _items[id];
        }

        public IEnumerable<T> GetAll()
        {
            return _items.Values;
        }

        public string Put(T obj)
        {
            var id = (_nextId++).ToString();
            obj.SetPropertyValue("Id", id);
            _items[id] = obj;
            return id;
        }

        public void Remove(string id)
        {
            _items.Remove(id);
        }
    }
}
