
namespace HomeTaskManagement.App.Common
{
    public sealed class Event
    {
        public string EntityType { get; set; }
        public string EntityId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string JsonPayload { get; set; }
        public long OccurredOn { get; set; }

        public Event(string entityType, string entityId, string name, int version, string jsonPayload, long occurredOn)
        {
            EntityType = entityType;
            EntityId = entityId;
            Name = name;
            Version = version;
            JsonPayload = jsonPayload;
            OccurredOn = occurredOn;
        }

        public T PayloadAs<T>()
        {
            return Json.ToObject<T>(JsonPayload);
        }
    }
}
