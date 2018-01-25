using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Common
{
    public sealed class TimeOrderedSequence<T>
    {
        private readonly T _default;
        private SortedList<UnixUtcTime, T> _sequence = new SortedList<UnixUtcTime, T>();

        public TimeOrderedSequence(T defaultValue)
        {
            _default = defaultValue;
        }

        public void Add(UnixUtcTime time, T value)
        {
            _sequence.Add(time, value);
        }

        public T At(UnixUtcTime time)
        {
            return _sequence.Where(x => !x.Key.IsAfter(time))
                .Select(x => x.Value)
                .LastOrDefault(_default);
        }
    }
}
