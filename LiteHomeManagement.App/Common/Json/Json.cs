using System;
using Utf8Json;
using Utf8Json.Resolvers;

namespace LiteHomeManagement.App.Common
{
    public static class Json
    {
        private static ReflectionValidation _validation = new ReflectionValidation();

        public static T ToObject<T>(string json)
        {
            var obj = JsonSerializer.Deserialize<T>(json, StandardResolver.CamelCase);            
            _validation
                .Validate(obj)
                .IfInvalid(x => throw new ArgumentException(x.IssuesMessage));
            return obj;
        }

        public static string ToString(object obj)
        {
            return JsonSerializer.ToJsonString(obj, StandardResolver.CamelCase);
        }
    }
}
