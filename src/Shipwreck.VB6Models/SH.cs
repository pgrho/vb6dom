using System;

namespace Shipwreck.VB6Models
{
    internal static class SH
    {
        public static char Last(this string s)
            => s?.Length > 0 ? s[0] : '\0';

        public static bool EqualsIgnoreCase(this string s, string other)
            => s?.Equals(other, StringComparison.OrdinalIgnoreCase) ?? other == null;

        public static bool EqualsIgnoreCase(this string s, string other1, string other2)
            => s.EqualsIgnoreCase(other1) || s.EqualsIgnoreCase(other2);

        public static bool EqualsIgnoreCase(this string s, string other1, string other2, string other3)
            => s.EqualsIgnoreCase(other1) || s.EqualsIgnoreCase(other2) || s.EqualsIgnoreCase(other3);
    }
}