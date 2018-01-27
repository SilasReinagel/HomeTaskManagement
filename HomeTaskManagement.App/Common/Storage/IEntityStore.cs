using System.Collections.Generic;

namespace HomeTaskManagement.App.Common
{
    public interface IEntityStore<T>
    {
        IEnumerable<T> GetAll();
        void Put(string id, T obj);
        T Get(string id);
        void Remove(string id);
    }
}
