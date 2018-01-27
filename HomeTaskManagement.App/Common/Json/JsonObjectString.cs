using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public class JsonObjectString
    {
        private readonly List<string> _entries = new List<string>();

        public JsonObjectString With(string name, JsonObjectString jsonObjectString)
        {
            _entries.Add($"\"{name.ToCamelCase()}\":{jsonObjectString.ToString()}");
            return this;
        }

        public JsonObjectString With(string name, IEnumerable<string> strings)
        {
            var items = string.Join(",", strings.Select(x => $"\"{x}\""));
            _entries.Add($"\"{name.ToCamelCase()}\":[{items}]");
            return this;
        }

        public JsonObjectString With<T>(string name, IEnumerable<T> collection, Func<T, JsonObjectString> itemBuilder)
        {
            var results = collection.Select(item => (string)itemBuilder(item)).ToList();

            var listJson = string.Join(",", results);

            _entries.Add($"\"{name.ToCamelCase()}\":[{listJson}]");

            return this;          
        } 

        public JsonObjectString With<TKey, TValue>(string name, IDictionary<TKey, TValue> dictionary, Func<TValue, string> valueFormatter)
        {
            var results = dictionary.Select(kvp => $"\"{kvp.Key}\":{valueFormatter(kvp.Value)}").ToList();

            var dictionaryJson = string.Join(",", results);

            _entries.Add($"\"{name.ToCamelCase()}\":{{{dictionaryJson}}}");

            return this;          
        } 

        public JsonObjectString With(string name, string value)
        {
            _entries.Add($"\"{name.ToCamelCase()}\":\"{value}\"");
            return this;
        }
        
        public JsonObjectString With(string name, decimal number)
        {
            _entries.Add($"\"{name.ToCamelCase()}\":{number}");
            return this;
        }

        public JsonObjectString With(string name, bool condition)
        {
            _entries.Add($"\"{name.ToCamelCase()}\":{condition.ToString().ToLowerInvariant()}");
            return this;
        }

        public new string ToString()
        {
            return $"{{{string.Join(",", _entries)}}}";
        }

        public static implicit operator string(JsonObjectString jsonValue)
        {
            return jsonValue.ToString();
        }
    }
}
