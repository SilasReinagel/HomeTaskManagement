using System;

namespace HomeTaskManagement.App.Common
{
    public static class Clock
    {
        private static bool _isFrozen;
        private static Func<DateTimeOffset> _utcClock;

        private static DateTimeOffset UtcNow => _utcClock.Invoke();

        public static UnixUtcTime UnixUtcNow => new UnixUtcTime(UtcNow.ToUnixTimeMilliseconds());

        static Clock()
        {
            _utcClock = () => DateTimeOffset.UtcNow;
        }

        public static void Advance(TimeSpan duration)
        {
            if (!_isFrozen)
                throw new InvalidOperationException("Cannot alter actual system time.");

            var newTime = UtcNow.Add(duration);
            _utcClock = () => newTime;
        }

        public static void Freeze()
        {
            if (_isFrozen)
                return;

            _isFrozen = true;
            var frozenTime = UtcNow;
            _utcClock = () => frozenTime;
        }
    }
}
