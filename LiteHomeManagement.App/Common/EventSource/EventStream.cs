using System.Collections.Generic;

namespace LiteHomeManagement.App.Common
{
    public struct EventStream
    {
        public string Id { get; set; }
        public IEnumerable<Event> Events { get; set; }        
    }
}
