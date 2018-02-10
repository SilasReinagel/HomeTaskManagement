using HomeTaskManagement.App.Common;
using System.Collections.Generic;

namespace HomeTaskManagement.App.Task.Instance
{
    public sealed class InMemoryTaskInstanceStore : ITaskInstanceStore
    {
        private readonly InMemoryEntityStore<TaskInstanceRecord> _store = new InMemoryEntityStore<TaskInstanceRecord>();
            
        public Maybe<TaskInstanceRecord> Get(string id)
        {
            return _store.Get(id);
        }

        public IEnumerable<TaskInstanceRecord> GetAll()
        {
            return _store.GetAll();
        }

        public void Put(string id, TaskInstanceRecord obj)
        {
            _store.Put(id, obj);
        }

        public void Remove(string id)
        {
            _store.Remove(id);
        }
    }
}
