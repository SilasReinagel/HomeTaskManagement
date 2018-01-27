using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace HomeTaskManagement.App.Common
{
    internal static class ReflectionExtensions
    {
        private static readonly Dictionary<Type, Dictionary<string, PropertyInfo>> PropertiesCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();

        public static bool HasProperty(this Type type, string propertyName)
        {
            return GetProperties(type).ContainsKey(propertyName);
        }

        public static Maybe<PropertyInfo> GetProperty(this object obj, string propertyName)
        {
            return GetProperty(obj.GetType(), propertyName);
        }

        public static Maybe<PropertyInfo> GetProperty(this Type type, string propertyName)
        {
            return !type.HasProperty(propertyName)
                ? Maybe<PropertyInfo>.Missing
                : GetProperties(type)[propertyName];
        }

        public static Dictionary<string, PropertyInfo> GetProperties(this object obj)
        {
            return GetProperties(obj.GetType());
        }

        public static Dictionary<string, PropertyInfo> GetProperties(this Type type)
        {
            if (!PropertiesCache.ContainsKey(type))
                InitProperties(type);
            return PropertiesCache[type];
        }

        public static T GetPropertyValue<T>(this object obj, string propertyName)
        {
            return (T)GetPropertyValue(obj, propertyName);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            var prop = obj.GetProperty(propertyName);
            if (prop.IsMissing)
                throw new InvalidOperationException($"Cannot get value for non-existent property {propertyName}");
            return prop.Value.GetValue(obj);
        }

        public static object GetPropertyValueOrDefault(this object obj, string propertyName, object defaultValue)
        {
            var prop = obj.GetProperty(propertyName);
            return prop.IsPresent
                ? prop.Value.GetValue(obj)
                : defaultValue;
        }

        public static void SetPropertyValue(this object obj, string propertyName, object value)
        {
            var currentType = obj.GetType();
            while (currentType != null)
            {
                var prop = GetProperty(currentType, propertyName);
                if (prop.IsPresent && prop.Value.CanWrite)
                {
                    prop.Value.SetValue(obj, value);
                    return;
                }
                currentType = currentType.BaseType;
            }
        }

        public static Type GetItemType(this IEnumerable stream)
        {
            return stream.GetType().GetGenericArguments()[0];
        }

        public static IList<Type> GetDerivedTypes(this Assembly assembly, Type baseType)
        {
            return assembly.GetTypes().Where(baseType.IsAssignableFrom).ToList();
        }

        public static bool HasAttribute(this MemberInfo member, Type attributeType)
        {
            return Attribute.IsDefined(member, attributeType);
        }

        private static void InitProperties(Type type)
        {
            PropertiesCache[type] = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .ToDictionary(x => x.Name, x => x);
        }
    }
}
