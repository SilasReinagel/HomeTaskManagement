using System;

namespace LiteHomeManagement.App.Common
{
    public static class StringExtensions
    {
        public static bool Matches(this string v1, string v2)
        {
            return v1.Equals(v2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string ToCamelCase(this string s)
        {
            return Char.ToLowerInvariant(s[0]) + s.Substring(1);
        }
    }
}
