using System;

namespace PiratePlayer.Extensions
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool SearchFinds(this string source, string search)
        {
            return source.Contains(search.Trim(), StringComparison.OrdinalIgnoreCase);
        }
    }
}
