using System;

namespace HomeTaskManagement.App.Common
{
    public static class CompositeExtensions
    {
        public static Response IsNotPast(this UnixUtcTime time)
        {
            return time.IsBefore(Clock.UnixUtcNow)
                ? Response.Errored(ResponseStatus.InvalidState, "Cannot occur at past point in time.")
                : Response.Success;
        }
    }
}
