using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public interface IEntityStore<T>
    {
        IEnumerable<T> GetAll();
        void Put(string id, T obj);
        T Get(string id);
        void Remove(string id);
    }
}
