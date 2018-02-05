using HomeTaskManagement.App.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.Sql.EventStore
{
    [Ignore]
    [TestClass]
    public sealed class SqlEventStoreTests
    {
        private SqlEventStore _eventStore;

        [TestInitialize]
        public void Init()
        {
            _eventStore = new SqlEventStore(new SqlDatabase(), "HomeTask.Events");
        }

        [TestMethod]
        public void SqlEventStore_CommitAndGet_Works()
        {
            var id = Guid.NewGuid().ToString();
            var now = Clock.UnixUtcNow;
            _eventStore.Commit(new List<Event> { new Event(nameof(Int32), id, "SampleEvent", 1, "{}", now) });

            var events = _eventStore.GetEvents<Int32>(id).Single();

            Assert.AreEqual(nameof(Int32), events.EntityType);
            Assert.AreEqual(id, events.EntityId);
            Assert.AreEqual("SampleEvent", events.Name);
            Assert.AreEqual(1, events.Version);
            Assert.AreEqual("{}", events.JsonPayload);
            Assert.AreEqual(now.Millis, events.OccurredAt);
        }
    }
}
