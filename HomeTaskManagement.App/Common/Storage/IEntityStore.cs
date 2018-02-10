using System.Collections.Generic;

namespace HomeTaskManagement.App.Common
{
    public interface IEntityStore<T>
    {
        IEnumerable<T> GetAll();
        void Put(string id, T obj);
        Maybe<T> Get(string id);
        void Remove(string id);
    }
}
