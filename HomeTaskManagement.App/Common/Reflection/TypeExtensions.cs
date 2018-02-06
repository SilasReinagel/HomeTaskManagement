using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    internal static class TypeExtensions
    {
        private static readonly Dictionary<string, Type> _typesByName = new Dictionary<string, Type>();

        public static Type AsType(this string name, string inNameSpace)
        {
            var key = name.ToLowerInvariant();
            if (_typesByName.ContainsKey(key))
                return _typesByName[key];

            var types = AppDomain.CurrentDomain.GetAssemblies()
                                   .Where(x => x.FullName.StartsWith(inNameSpace))
                                   .SelectMany(x => x.GetTypes())
                                   .Where(x => !(x.Name.StartsWith("<>")));
            types.ForEach(x => _typesByName.Add(x.Name.ToLowerInvariant(), x));

            if (!_typesByName.ContainsKey(key))
                throw new KeyNotFoundException($"Type not found in current assemblies: '{name}'");
            return _typesByName[key];
        }

        public static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsDictionary(this Type type)
        {
            var result = type.GetInterfaces()
                .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IDictionary<,>));
            return result;
        }

        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable).IsAssignableFrom(type) ||
                type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static bool IsDateTime(this Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.DateTime || type.IsNullableOf<DateTime>();
        }

        public static bool IsNumeric(this Type type)
        {
            return type == typeof(sbyte)
                    || type == typeof(byte)
                    || type == typeof(short)
                    || type == typeof(ushort)
                    || type == typeof(int)
                    || type == typeof(uint)
                    || type == typeof(long)
                    || type == typeof(ulong)
                    || type == typeof(float)
                    || type == typeof(double)
                    || type == typeof(decimal);
        }

        public static bool IsInt(this Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Int32 || type.IsNullableOf<int>();
        }

        public static bool IsLong(this Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Int64 || type.IsNullableOf<long>();
        }

        public static bool IsDouble(this Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Double || type.IsNullableOf<double>();
        }

        public static bool IsDecimal(this Type type)
        {
            return Type.GetTypeCode(type) == TypeCode.Decimal || type.IsNullableOf<decimal>();
        }

        public static bool IsNullableOf<T>(this Type type)
        {
            return type.IsGenericType &&
                type.GetGenericTypeDefinition() == typeof(Nullable<>) &&
                Nullable.GetUnderlyingType(type) == typeof(T);
        }
    }
}
