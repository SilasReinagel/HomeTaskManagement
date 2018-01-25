using System;

namespace LiteHomeManagement.App.Common
{
    public sealed class Id
    {
        private readonly string _value;

        public Id()
            : this(Guid.NewGuid().ToString()) { }

        public Id(string id)
        {
            _value = id;
        }

        public static implicit operator string(Id id)
        {
            return id._value;
        }
    }
}
