using System;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public static class StorageExtensions
    {
        public static bool Contains<T>(this IEntityStore<T> store, Func<T, bool> condition)
        {
            return store
                .GetAll()
                .Any(condition);
        }

        public static void Update<T>(this IEntityStore<T> store, string id, Action<T> update)
        {
            var entity = store.Get(id).Value;
            update(entity);
            store.Put(id, entity);
        }
    }
}
