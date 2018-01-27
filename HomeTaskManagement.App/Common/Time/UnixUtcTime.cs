using System;

namespace HomeTaskManagement.App.Common
{
    public struct UnixUtcTime : IComparable
    {
        public long Millis { get; }

        public UnixUtcTime(long unixMillis)
        {
            Millis = unixMillis;
        }

        public bool IsBefore(long unixMillis)
        {
            return Millis < unixMillis;
        }

        public bool IsAfter(long unixMillis)
        {
            return Millis > unixMillis;
        }

        public UnixUtcTime Plus(TimeSpan timeSpan)
        {
            return new UnixUtcTime(Millis + ToLong(timeSpan));
        }

        public UnixUtcTime Minus(TimeSpan timeSpan)
        {
            return new UnixUtcTime(Millis - ToLong(timeSpan));
        }

        public override string ToString()
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(Millis).ToString();
        }

        public static UnixUtcTime operator +(UnixUtcTime time, long millis)
        {
            return new UnixUtcTime(time.Millis + millis);
        }

        public static UnixUtcTime operator -(UnixUtcTime time, long millis)
        {
            return new UnixUtcTime(time.Millis - millis);
        }

        public static implicit operator long(UnixUtcTime time)
        {
            return time.Millis;
        }

        public static UnixUtcTime From(DateTime time)
        {
            return new UnixUtcTime(ToLong(time.Subtract(Epoch.Start)));
        }

        public static UnixUtcTime From(DateTimeOffset time)
        {
            return new UnixUtcTime(ToLong(time.DateTime.Subtract(Epoch.Start)));
        }

        public static DateTime ToDateTime(UnixUtcTime time)
        {
            return DateTimeOffset.FromUnixTimeMilliseconds(time.Millis).UtcDateTime;
        }

        private static long ToLong(TimeSpan duration)
        {
            return Convert.ToInt64(Math.Round(duration.TotalMilliseconds));
        }

        public int CompareTo(object obj)
        {
            if (!(obj is UnixUtcTime))
                throw new ArgumentException($"Cannot compare {nameof(UnixUtcTime)} to {obj.GetType().Name}");
            return Millis.CompareTo(((UnixUtcTime)obj).Millis);
        }
    }
}
