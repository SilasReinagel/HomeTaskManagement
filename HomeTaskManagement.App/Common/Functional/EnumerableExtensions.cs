using System;
using System.Collections.Generic;
using System.Linq;

namespace HomeTaskManagement.App.Common
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            items.ToList().ForEach(action);
        }

        public static bool None<T>(this IEnumerable<T> items)
        {
            return !items.Any();
        }

        public static T LastOrDefault<T>(this IEnumerable<T> items, T defaultValue)
        {
            return items.Any() 
                ? items.Last() 
                : defaultValue;
        }

        public static IEnumerable<T> Finalize<T>(this IEnumerable<T> items)
        {
            return items.ToList();
        }
    }
}
