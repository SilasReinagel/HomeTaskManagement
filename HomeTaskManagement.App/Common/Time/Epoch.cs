using System;

namespace HomeTaskManagement.App.Common
{
    public struct Epoch
    {
        private readonly UnixUtcTime _time;

        public static Epoch Start => new Epoch(new UnixUtcTime(0));

        private Epoch(UnixUtcTime time)
        {
            _time = time;
        }

        public static implicit operator DateTime(Epoch epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .Add(TimeSpan.FromMilliseconds(epoch._time));
        }

        public static implicit operator UnixUtcTime(Epoch epoch)
        {
            return epoch._time;
        }
    }
}
