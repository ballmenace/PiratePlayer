using System;

namespace PiratePlayer.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool ContainsIgnoreCase(this string source, string value)
        {
            return source.Contains(value.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
