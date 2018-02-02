
namespace HomeTaskManagement.App.Common
{
    public sealed class Event
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string JsonPayload { get; set; }
        public UnixUtcTime OccurredAt { get; set; }

        public Event(string entityType, string entityId, string name, int version, string jsonPayload, UnixUtcTime occurredAt)
        {
            EntityType = entityType;
            EntityId = entityId;
            Name = name;
            Version = version;
            JsonPayload = jsonPayload;
            OccurredAt = occurredAt;
        }

        public T PayloadAs<T>()
        {
            return Json.ToObject<T>(JsonPayload);
        }
    }
}
