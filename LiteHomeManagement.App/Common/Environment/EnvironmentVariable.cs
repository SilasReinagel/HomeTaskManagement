using System;

namespace LiteHomeManagement.App.Common
{
    public sealed class EnvironmentVariable
    {
        private readonly string _name;

        public EnvironmentVariable(string name)
        {
            _name = name;
        }

        public static implicit operator string(EnvironmentVariable var)
        {
            return Environment.GetEnvironmentVariable(var._name);
        }
    }
}
