using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public static class CalendarExtensions
    {
        public static UnixUtcTime Next(this UnixUtcTime start, DayOfWeek day)
        {
            var nextDay = start.Plus(TimeSpan.FromDays(1));
            var daysToAdd = ((int)day - (int)nextDay.DayOfWeek + 7) % 7;
            return nextDay.Plus(TimeSpan.FromDays((daysToAdd)));
        }

        public static IEnumerable<UnixUtcTime> Every(this UnixUtcTime from, TimeSpan interval)
        {
            for (var time = from; time.IsBefore(long.MaxValue); time = time.Plus(interval))
                yield return time;
        }

        public static IEnumerable<UnixUtcTime> Until(this IEnumerable<UnixUtcTime> stream, UnixUtcTime until)
        {
            return stream.TakeWhile(x => x.IsBefore(until));
        }

        public static UnixUtcTime StartOfDay(this UnixUtcTime time)
        {
            var moment = DateTimeOffset.FromUnixTimeMilliseconds(time);
            var startOfDay = new DateTimeOffset(moment.Year, moment.Month, moment.Day, 0, 0, 0, TimeSpan.Zero);
            return new UnixUtcTime(startOfDay.ToUnixTimeMilliseconds());
        }
    }
}
