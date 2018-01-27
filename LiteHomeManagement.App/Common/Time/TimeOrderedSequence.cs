using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LiteHomeManagement.App.Common
{
    public sealed class History<T> : IEnumerable<KeyValuePair<UnixUtcTime, T>>
    {
        private readonly T _default;
        private readonly List<KeyValuePair<UnixUtcTime, T>> _sequence = new List<KeyValuePair<UnixUtcTime, T>>();

        public History(T defaultValue)
        {
            _default = defaultValue;
        }

        public void Add(UnixUtcTime time, T value)
        {
            _sequence.Add(new KeyValuePair<UnixUtcTime, T>(time, value));
            _sequence.Sort((l, r) => l.Key.CompareTo(r.Key));
        }

        public T At(UnixUtcTime time)
        {
            return _sequence.Where(x => !x.Key.IsAfter(time))
                .Select(x => x.Value)
                .LastOrDefault(_default);
        }

        public IEnumerator<KeyValuePair<UnixUtcTime, T>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<UnixUtcTime, T>>)_sequence).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<UnixUtcTime, T>>)_sequence).GetEnumerator();
        }
    }
}
