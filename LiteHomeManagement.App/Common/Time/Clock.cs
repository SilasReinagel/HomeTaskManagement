﻿using System;

namespace LiteHomeManagement.App.Common
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
