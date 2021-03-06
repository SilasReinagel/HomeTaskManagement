﻿using System;

namespace HomeTaskManagement.App.Common
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
            return Environment.GetEnvironmentVariable(var._name).Required($"Environment Variable {var._name}");
        }
    }
}
