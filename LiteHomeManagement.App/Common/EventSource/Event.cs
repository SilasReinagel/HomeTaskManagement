
namespace LiteHomeManagement.App.Common
{
    public sealed class Event
    {
        public string EntityId { get; set; }
        public string Name { get; set; }
        public int Version { get; set; }
        public string JsonPayload { get; set; }
        public UnixUtcTime OccurredOn { get; set; }

        public Event(string entityId, string name, int version, string jsonPayload, UnixUtcTime occurredOn)
        {
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
