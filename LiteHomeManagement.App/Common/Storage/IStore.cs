using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public interface IStore<T>
    {
        IEnumerable<T> GetAll();
        string Put(T obj);
        T Get(string id);
        void Remove(string id);
    }
}
