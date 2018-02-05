using Dapper;
using HomeTaskManagement.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.Sql.EventStore
{
    public sealed class SqlEventStore : IEventStore
    {
        private readonly SqlDatabase _db;
        private readonly string _tableName;

        public SqlEventStore(SqlDatabase db, string tableName)
        {
            _db = db;
            _tableName = tableName;
        }

        public void Commit(IEnumerable<Event> events)
        {
            var sql = $@"INSERT INTO {_tableName} (EntityType, EntityId, Name, Version, JsonPayload, OccurredAt)
                values(@entityType, @entityId, @name, @version, @jsonPayload, @occurredAt)";

            var items = events.Select(x => new InsertEvent
            {
                EntityId = x.EntityId,
                EntityType = x.EntityType,
                Name = x.Name,
                Version = x.Version,
                JsonPayload = x.JsonPayload,
                OccurredAt = UnixUtcTime.ToDateTime(x.OccurredAt)
            });

            _db.UsingConnection(x =>
            {
                var t = x.BeginTransaction();
                x.Execute(sql, items, t);
                t.Commit();
            });
        }

        public IEnumerable<Event> GetEvents<T>(string entityId)
        {
            var sql = $@"SELECT * 
                        FROM {_tableName}
                        WHERE EntityType = @entityType AND EntityId = @entityId";

            return _db.Query<Event>(sql, new { entityType = typeof(T).Name, entityId });
        }

        public IEnumerable<EventStream> GetEvents<T>()
        {
            var sql = $@"SELECT * 
                        FROM {_tableName}
                        WHERE EntityType = @entityType";

            return _db.Query<Event>(sql, new { entityType = typeof(T).Name })
                .GroupBy(x => x.EntityId)
                .Select(g => new EventStream { Id = g.Key, Events = g });
        }

        private sealed class InsertEvent
        {
            public string EntityType { get; set; }
            public string EntityId { get; set; }
            public string Name { get; set; }
            public int Version { get; set; }
            public string JsonPayload { get; set; }
            public DateTime OccurredAt { get; set; }
        }
    }
}
